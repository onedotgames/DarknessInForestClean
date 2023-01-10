using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponBaseV2 : CustomBehaviour
{
    protected delegate void WeaponAttackMethod();
    protected WeaponAttackMethod AttackMethodOne;
    protected ProjectileSpawner Pooler;
    private WaitForSeconds mWait;
    private Coroutine mAttackRoutine;
    private float _timeValue;
    private bool _timerOn;
    private List<Task> _tasks;
    private Transform _target = null;
    private List<ChestnutHammer> ActiveChestnuts;
    private List<BeeShotProjectile> ActiveBeeShots;

    public int BeeIndex = 0;

    public SkillSO SkillSO;
    public int UpgradeLevel = 0;
    [HideInInspector]
    public bool IsActivated = false;
    [HideInInspector]
    public bool IsInitialized = false;
    public bool IsEvolved = false;
    [HideInInspector]
    public Transform Target;
    public Vector3 playerCurLook;
    protected Vector3 mDirection;
    public ParticleSystem HitEffect;
    public bool isMiniGameComplete = false;
    public GameObject[] minigames;
    public GameObject[] bananas;
    public SelectSkill SelectSkillPanel;
    #region Local Stats
    [HideInInspector]
    public PoolerBase PoolerBase;
    [HideInInspector]
    public PoolerBase BarrelPooler;
    [HideInInspector]
    public float BaseDamage;
    [HideInInspector]
    public float BaseSpeed;
    [HideInInspector]
    public float AttackRange;
    [HideInInspector]
    public float Cooldown;
    [HideInInspector]
    public int Count;
    [HideInInspector]
    public Vector3 Size;
    [HideInInspector]
    public int Guardian;
    public GameObject poisonArea;
    private int controlInt = 0;
# endregion
    protected Player mPlayer;

    public StatList StatList = new StatList();
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (gameManager != null)
        {
            //GameManager.OnMiniGame += OnMiniGame;
            //GameManager.OnStartGame += OnGameStart;
            //GameManager.OnLevelCompleted += OnGameCompleted;
            //GameManager.OnLevelFailed += OnGameFailed;

        }
        SelectSkillPanel = gameManager.UIManager.GetPanel(Panels.SelectSkill).GetComponent<SelectSkill>();
        mPlayer = gameManager.PlayerManager.CurrentPlayer;
        PoolerBase = gameManager.PoolingManager.WeaponPooler[(int)SkillSO.PoolerType]; 
        Pooler = GameManager.PoolingManager.ProjectileSpawners[(int)SkillSO.PoolerType];
        Debug.Log(Pooler.gameObject.name);
        SetStats();
        mWait = new WaitForSeconds(Cooldown);
        _tasks = new List<Task>(1);
        IsActivated = true;
        //SetAttackMethod();
    }
    private async void OnEnable()
    {
        SetSkill(GameManager.AIManager.EnemyList);
        await SetAttackMethod();
    }

    private void OnDisable()
    {
        _timerOn = false;
    }
    public void SetStats()
    {
        if (UpgradeLevel == 0)
        {
            BaseDamage = SkillSO.BaseDamage;
            BaseSpeed = SkillSO.BaseSpeed;
            AttackRange = SkillSO.AttackRange;
            Cooldown = SkillSO.Cooldown;
            Count = SkillSO.Count;
            Size = SkillSO.Size;
            Guardian = SkillSO.Guardian;
        }
        else
        {
            BaseDamage = StatList.BaseDamage;
            BaseSpeed = StatList.BaseSpeed;
            Count = StatList.Count;
            AttackRange = StatList.AttackRange;
            Cooldown = StatList.Cooldown;
            Size = SkillSO.Size;
            Guardian = SkillSO.Guardian;
        }

    }
    public virtual void SetSkill(List<Transform> enemies)
    {
        GetClosestEnemy(enemies);

        if (_target != null)
        {
            mDirection = Vector3.Normalize(_target.position - transform.position);

        }
        else
        {
            var x = UnityEngine.Random.Range(-1f, 1f);
            var y = UnityEngine.Random.Range(-1f, 1f);
            mDirection = (new Vector3(x, y)).normalized;
        }
        if (this.SkillSO.DamagePattern == DamagePattern.Yoyo)
        {
            var x = UnityEngine.Random.Range(-1f, 1f);
            var y = UnityEngine.Random.Range(-1f, 1f);
            mDirection = (new Vector3(x, y)).normalized;
        }
        if (this.SkillSO.PoolerType == PoolerType.BeeShotPooler)
        {
            //var x = UnityEngine.Random.Range(-1f, 1f);
            //var y = UnityEngine.Random.Range(-1f, 1f);
            //mDirection = (new Vector3(x, y)).normalized;
            mDirection = Vector2.left;
        }
    }
    

    private async void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted && _timerOn)
        {
            _timeValue += Time.deltaTime;
            if (_timeValue >= Cooldown)
            {
                _timerOn = false;

                await SetAttackMethod();

                //await Task.WhenAny(_tasks);

                _tasks.Clear();
                _timeValue = 0;
                _timerOn = true;
            }
        }

    }


    public void UpdateWeapon()
    {
        if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyToChange == PropertyToChange.Evolve)
        {
            Debug.Log("Should play minigame");
            PlayMinigame(UnityEngine.Random.Range(0, GameManager.SkillManager.Minigames.Length));
        }
        else
        {
            if (UpgradeLevel < 4)
            {
                MakePropertyReadyForChange(SkillSO.UpgradeDatas[UpgradeLevel].PropertyToChange);
            }
        }
        
    }

    private void UpdateStats()
    {

        StatList.BaseDamage = BaseDamage;
        StatList.BaseSpeed = BaseSpeed;
        StatList.Count = Count;
        StatList.Cooldown = Cooldown;
        StatList.AttackRange = AttackRange;
        StatList.Size = Size;
        StatList.Guardian = Guardian;
    }

    private void CloseSkillPanel()
    {
        if (SelectSkillPanel.gameObject.activeInHierarchy)
        {
            SelectSkillPanel.ClosePanel();
        }
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
    }
    public void PlayMinigame(int gameIndex)
    {
        //Time.timeScale = 0;
        //SelectSkillPanel.ClosePanel();
        var miniGameObject = GameManager.SkillManager.Minigames[gameIndex];
        var miniGame = miniGameObject.GetComponent<MiniGameBase>();
        if (gameIndex == 0)
        {
            miniGame.TargetImage = SkillSO.Icon;
            miniGame.TargetBox.sprite = SkillSO.Icon;
            for (int i = 0; i < miniGame.Weapons.Count; i++)
            {
                if (miniGame.Weapons[i].name == SkillSO.Icon.name)
                {
                    controlInt++;
                }

            }
            if (controlInt == 0)
            {
                //resim yoktur..
                miniGame.Weapons[0] = SkillSO.Icon;
            }
        }
        miniGameObject.SetActive(true);
    }
    public void MakePropertyReadyForChange(PropertyToChange propertyToChange)
    {
        switch (propertyToChange)
        {
            case PropertyToChange.Damage:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    BaseDamage *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    BaseDamage += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                UpdateStats();
                CloseSkillPanel();
                

                break;
            case PropertyToChange.Speed:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    BaseSpeed *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    BaseSpeed += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }

                UpdateStats();
                CloseSkillPanel();

                break;
            case PropertyToChange.Count:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    Count *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    Count += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }

                UpdateStats();
                CloseSkillPanel();

                break;
            case PropertyToChange.Cooldown:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    Cooldown *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    Cooldown += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }

                UpdateStats();
                CloseSkillPanel();

                break;
            case PropertyToChange.Range:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    AttackRange *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    AttackRange += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }

                UpdateStats();
                CloseSkillPanel();

                break;
            case PropertyToChange.Size:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    Size *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    Size *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                UpdateStats();

                if (poisonArea != null)
                    poisonArea.transform.localScale = Size;

                CloseSkillPanel();

                break;
            case PropertyToChange.Evolve:
                EvolveWeapon();
                break;
            case PropertyToChange.Guardian:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    Guardian *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    Guardian += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }


                UpdateStats();

                //Guardian da yeni bir muz a?.
                for (int i = 0; i <= StatList.Guardian; i++)
                {
                    bananas[i].SetActive(true);
                }

                CloseSkillPanel();
                break;
        }
        UpgradeLevel++;

    }
    public virtual void EvolveWeapon()
    {
        if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
        {
            if (this.SkillSO.DamagePattern == DamagePattern.SkunkGas)
            {
                this.Size *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.BaseDamage *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
            }
            else if (this.SkillSO.DamagePattern == DamagePattern.BananaGuardian)
            {
                this.BaseDamage *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
            }
            else
            {
                this.AttackRange *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Count *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
            }

        }
        else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
        {
            if (this.SkillSO.DamagePattern == DamagePattern.SkunkGas)
            {
                this.Size *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.BaseDamage *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                GameManager.SkillManager.AllWeaponsV2.Remove(this);
            }
            else if (this.SkillSO.DamagePattern == DamagePattern.BananaGuardian)
            {
                this.BaseDamage *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                GameManager.SkillManager.AllWeaponsV2.Remove(this);

            }
            else
            {
                this.AttackRange *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Count *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                GameManager.SkillManager.AllWeaponsV2.Remove(this);

            }
        }


        UpdateStats();

        IsEvolved = true;
        GameManager.SkillManager.AllWeaponsV2.Remove(this);
        GameManager.SkillManager.AllWeaponsV2.Remove(this);
    }
    private async Task SetAttackMethod()
    {
        switch (SkillSO.PoolerType)
        {
            case PoolerType.ChestnutPooler:

                break;
            case PoolerType.CloverPooler:
                await Clover();
                break;
            case PoolerType.PoisonDartPooler:

                break;
            case PoolerType.SlingPooler:

                break;
            case PoolerType.WhipPooler:
                mAttackRoutine = StartCoroutine(IvyWhip());
                break;
            case PoolerType.SpiderWebPooler:

                break;
            case PoolerType.SpiderPoisonPooler:

                break;
            case PoolerType.BeeShotPooler:

                ActiveBeeShots = new List<BeeShotProjectile>(Count);

                await BeeShot();
                break;
            case PoolerType.BirdBomb:

                break;
            case PoolerType.SkunGasPooler:

                break;
            case PoolerType.BananaPooler:

                break;
        }
        _timerOn = true;

    }
    /*private IEnumerator Clover()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Coroutine Started");
        IsActivated = true;
        Debug.Log("Is Activated?: " + IsActivated);
        if (IsActivated)
        {
            Debug.Log(gameObject.name + " Activated");
            Debug.Log(GameManager.IsGamePaused);
            Debug.Log(GameManager.IsGameStarted);
            while (!GameManager.IsGamePaused && GameManager.IsGameStarted)
            {
                var target = GetClosestEnemy(GameManager.AIManager.EnemyList);
                Debug.Log("Inside while loop");
                if (target != null)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        var obj = Pooler.GetFromPool();
                        obj.transform.position = this.transform.position;

                        Debug.Log(obj.name);
                        var clover = obj.gameObject.GetComponent<CloverProjectile>();
                        SetSkill(GameManager.AIManager.EnemyList);
                        clover.Initialize(GameManager);
                        clover.Pooler = Pooler;
                        clover.Speed = BaseSpeed;
                        clover.Damage = BaseDamage;
                        clover.Direction = mDirection;
                        clover.IsAoE = false;
                        clover.IsReady = true;
                        obj.gameObject.SetActive(true);
                        GameManager.AIManager.EnemyList.Remove(target);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else
                {
                    yield return null;
                }
                yield return mWait;

            }
        }
    }*/
    private async Task Clover()
    {
        GetClosestEnemy(GameManager.AIManager.EnemyList);

        if (_target != null)
        {
            for (int i = 0; i < Count; i++)
            {
                var obj = Pooler.GetFromPool();
                obj.transform.position = this.transform.position;


                var clover = obj.gameObject.GetComponent<CloverProjectile>();
                SetSkill(GameManager.AIManager.EnemyList);

                SetProjectile(clover);

                obj.gameObject.SetActive(true);
                GameManager.AIManager.EnemyList.Remove(_target);
                await Delay(0.25f);
            }
        }
        else
        {
            await Task.Yield();
        }
        _target = null;
    }

    

    private async Task BeeShot()
    {
        ActiveBeeShots.Clear();
        BeeIndex = 0;
        for (int i = 0; i < Count; i++)
        {
            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;


            var mBeeShot = obj.gameObject.GetComponent<BeeShotProjectile>();
            //SetSkill(GameManager.AIManager.EnemyList);
            mDirection = GameManager.JoystickManager.variableJoystick.LastDirection.normalized;
            if(mDirection == Vector3.zero)
            {
                SetSkill(GameManager.AIManager.EnemyList);
            }
            ActiveBeeShots.Add(mBeeShot);
            mBeeShot.index = BeeIndex;
            BeeIndex++;

            SetProjectile(mBeeShot);
            mBeeShot.GetDirection(mBeeShot.Direction);
                
            obj.gameObject.SetActive(true);
            //GameManager.AIManager.EnemyList.Remove(_target);
            await Delay(0.25f);
        }
    }

    async Task<int> Delay(float delay)
    {
        var mDelay = (int)(delay * 1000);
        await Task.Delay(mDelay);
        return mDelay;
    }
    private void SetProjectile(ProjectileBase projectileBase)
    {
        projectileBase.Initialize(GameManager);
        projectileBase.Pooler = Pooler;
        projectileBase.Speed = BaseSpeed;
        projectileBase.Damage = BaseDamage;
        projectileBase.Direction = mDirection;
        projectileBase.IsAoE = false;
        projectileBase.IsReady = true;
    }
    private IEnumerator IvyWhip()
    {
        //while(!GameManager.IsGamePaused && GameManager.IsGameStarted)
        //{
        //    for (int i = 0; i < Count; i++)
        //    {
        //        var obj = Pooler.GetFromPool();
        //        obj.transform.position = this.transform.position;

        //        Debug.Log(obj.name);
        //        obj.gameObject.SetActive(true);
        //        obj.objectTransform.gameObject.GetComponent<IvyWhip>().WhipAttack();

               yield return new WaitForSeconds(0.25f);
        //    }

        //}
    }

    private Transform GetClosestEnemy(List<Transform> enemies)
    {
        //Transform bestTarget = null;
        
        float closestDistanceSqr = AttackRange;
        Vector3 currentPosition = mPlayer.transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            if (potentialTarget.gameObject.activeInHierarchy)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;

                    _target = potentialTarget;
                }
            }
            
        }

        return _target;
    }
}
