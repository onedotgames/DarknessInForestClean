using DG.Tweening;
using DG.Tweening.Core.Easing;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    public List<CHammerProjectile> ActiveChestnuts { get; private set; }
    private List<BeeShotProjectile> ActiveBeeShots;
    private List<SlingBaseProjectile> SlingProjectileList;
    private HUD mHud;
    public int BeeIndex = 0;
    private float AttackDegree = 30;

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
        SelectSkillPanel = gameManager.UIManager.GetPanel(Panels.SelectSkill).GetComponent<SelectSkill>();
        mHud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        mPlayer = gameManager.PlayerManager.CurrentPlayer;
        //PoolerBase = gameManager.PoolingManager.WeaponPooler[(int)SkillSO.PoolerType]; 
        Pooler = gameManager.PoolingManager.ProjectileSpawners[(int)SkillSO.PoolerType];
        BarrelPooler = gameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
        //Debug.Log(Pooler.gameObject.name);
        SetStats();
        //mWait = new WaitForSeconds(Cooldown);
        _tasks = new List<Task>(1);
        IsActivated = true;
        //SetAttackMethod();
        
        CloseSkillPanel();
    }
    
    private void Assign()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame += OnGameStart;
            GameManager.OnLevelCompleted += OnGameCompleted;
            GameManager.OnLevelFailed += OnGameFailed;
            GameManager.OnRestartGame += RestartGame;
        }
    }

    private void OnGameStart()
    {

    }

    private void UnAssign()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= OnGameStart;
            GameManager.OnLevelCompleted -= OnGameCompleted;
            GameManager.OnLevelFailed -= OnGameFailed;
            GameManager.OnRestartGame -= RestartGame;
        }
    }

    private void OnGameCompleted()
    {
        ResetItemElementsOnEnd();
    }

    private void OnGameFailed()
    {
        ResetItemElementsOnEnd();
    }

    private void RestartGame()
    {
        ResetItemElementsOnEnd();
    }
    private void ResetItemElementsOnEnd()
    {
        IsActivated = false;
        IsEvolved = false;
    }

    private void ResetItemElementsOnStart()
    {
        AttackDegree = 30;
        BeeIndex = 0;
        UpgradeLevel = 0;
    }

    private async void OnEnable()
    {
        Assign();
        SetSkill(GameManager.AIManager.EnemyList);
        ResetItemElementsOnStart();

        await SetAttackMethod();
    }

    private void OnDisable()
    {
        _timerOn = false;
    }

    private void OnDestroy()
    {
        UnAssign();
    }
    public void SetStats()
    {
        if (UpgradeLevel == 0)
        {
            BaseDamage = SkillSO.BaseDamage + GameManager.InventoryManager.GlobalDamageIncrease;
            BaseSpeed = SkillSO.BaseSpeed;
            AttackRange = SkillSO.AttackRange;
            Cooldown = SkillSO.Cooldown;
            Count = SkillSO.Count;
            Size = SkillSO.Size;
            Guardian = SkillSO.Guardian;
        }
        else
        {
            BaseDamage = StatList.BaseDamage + GameManager.InventoryManager.GlobalDamageIncrease;
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
        if (this.SkillSO.PoolerType == PoolerType.PoisonDartPooler
            || this.SkillSO.PoolerType == PoolerType.SlingPooler)
        {
            //var x = UnityEngine.Random.Range(-1f, 1f);
            //var y = UnityEngine.Random.Range(-1f, 1f);
            //mDirection = (new Vector3(x, y)).normalized;
            mDirection = Vector2.left;
        }
        if(this.SkillSO.PoolerType == PoolerType.BeeShotPooler)
        {
            if (GameManager.InputManager.isMovementStop)
            {
                var x = UnityEngine.Random.Range(-1f, 1f);
                var y = UnityEngine.Random.Range(-1f, 1f);
                mDirection = (new Vector3(x, y)).normalized;
            }
            else
            {
                mDirection = Vector2.left;
            }
            
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
            CloseSkillPanel();
            PlayMinigame(UnityEngine.Random.Range(0, GameManager.SkillManager.Minigames.Length));
        }
        else
        {
            MakePropertyReadyForChange(SkillSO.UpgradeDatas[UpgradeLevel].PropertyToChange);
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
        mHud.OpenPanel();
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        GameManager.PlayerLevelManager.CheckExp();
        mHud.SetExpBarFillAmount();
    }
    public void PlayMinigame(int gameIndex)
    {
        Time.timeScale = 0;
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
        Debug.Log("Evolving: " + gameObject.name);
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
        GameManager.SkillManager.WeaponsInUseV2.Remove(this);
    }

    public async Task SetAttackMethod()
    {
        switch (SkillSO.PoolerType)
        {
            case PoolerType.ChestnutPooler:
                ActiveChestnuts = new List<CHammerProjectile>(Count);
                await WallnutHammer();
                break;

            case PoolerType.CloverPooler:
                await Clover();
                _timerOn = true;
                break;

            case PoolerType.PoisonDartPooler:
                await PoisonDart();
                _timerOn = true;
                break;

            case PoolerType.SlingPooler:
                SlingProjectileList = new List<SlingBaseProjectile>(Count);
                await Sling();
                _timerOn = true;
                break;

            case PoolerType.WhipPooler:
                await Whip();
                _timerOn = true;
                break;

            case PoolerType.SpiderWebPooler:
                await SpiderWeb();
                _timerOn = true;
                break;
            case PoolerType.SpiderPoisonPooler:
                await SpiderPoisonWeb();
                _timerOn = true;
                break;

            case PoolerType.BeeShotPooler:

                ActiveBeeShots = new List<BeeShotProjectile>(Count);

                await BeeShot();
                _timerOn = true;
                break;

            case PoolerType.BirdBomb:
                await BirdBomb();
                _timerOn = true;
                break;

            case PoolerType.SkunGasPooler:
                SkunkGas();
                break;

            case PoolerType.BananaPooler:
                BananaGuardian();
                break;
        }
        //_timerOn = true;

    }

    private async Task WallnutHammer()
    {
        for (int i = 0; i < Count; i++)
        {
            if (ActiveChestnuts != null)
            {
                ActiveChestnuts.Clear();
            }

            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;

            var wallnutHammer = obj.gameObject.GetComponent<CHammerProjectile>();
            ActiveChestnuts.Add(wallnutHammer);

            SetSkill(GameManager.AIManager.EnemyList);

            SetProjectile(wallnutHammer, false);
            wallnutHammer.Range = AttackRange;
            wallnutHammer.WeaponBaseV2 = this;
            obj.gameObject.SetActive(true);
            GameManager.AIManager.EnemyList.Remove(_target);
            await Delay(0.25f);
        }
    }

    private async Task Clover()
    {
        for (int i = 0; i < Count; i++)
        {
            GetClosestEnemy(GameManager.AIManager.EnemyList);

            if (_target != null)
            {
                    var obj = Pooler.GetFromPool();
                    obj.transform.position = this.transform.position;


                    var clover = obj.gameObject.GetComponent<CloverProjectile>();
                    SetSkill(GameManager.AIManager.EnemyList);

                    SetProjectile(clover, false);

                    obj.gameObject.SetActive(true);
                    GameManager.AIManager.EnemyList.Remove(_target);
                    await Delay(0.25f);
            }
            else
            {
                await Task.Yield();
            }
        }
        
        _target = null;
    }

    private async Task PoisonDart()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;


            var dart = obj.gameObject.GetComponent<PoisonDartProjectile>();
            //SetSkill(GameManager.AIManager.EnemyList);
            mDirection = GameManager.JoystickManager.variableJoystick.LastDirection.normalized;
            if (mDirection == Vector3.zero)
            {
                SetSkill(GameManager.AIManager.EnemyList);
            }

            SetProjectile(dart, true);
            dart.GetDirection(dart.Direction);

            obj.gameObject.SetActive(true);
            //GameManager.AIManager.EnemyList.Remove(_target);
            await Delay(0.25f);
        }
    }

    private async Task Sling()
    {
        GetClosestEnemy(GameManager.AIManager.EnemyList);

        if (_target != null)
        {
            for (int i = 0; i < Count; i++)
            {
                var obj = Pooler.GetFromPool();
                obj.transform.position = this.transform.position;

                var slingProjectile = obj.gameObject.GetComponent<SlingBaseProjectile>();
                SlingProjectileList.Add(slingProjectile);
            }

            var firstCount = SlingProjectileList.Count;

            float spaceBetweenProjectiles = AttackDegree / (SlingProjectileList.Count - 1);
            int index = 0;
            var count = SlingProjectileList.Count;

            //Bunlar for i?indeydi
            float horizontal = GameManager.JoystickManager.GetHorizontal();
            float vertical = GameManager.JoystickManager.GetVertical();
            float initialRot = 0;
            if (horizontal != 0 || vertical != 0)
            {
                initialRot = GameManager.PlayerManager.CurrentPlayer.Angle - 90;
            }
            else if (horizontal == 0 && vertical == 0)
            {
                //initialRot = GameManager.PlayerManager.CurrentPlayer.LastAngle - 90;
                initialRot = GameManager.JoystickManager.variableJoystick.LastAngle;
            }
            for (int i = 0; i < count; i++)
            {
                float RotDeg = initialRot + (spaceBetweenProjectiles) * 
                    ((firstCount - 1) / 2) - (spaceBetweenProjectiles * index);
                index++;

                SetProjectile(SlingProjectileList[0], false);

                SlingProjectileList[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, RotDeg));

                SlingProjectileList[0].gameObject.SetActive(true);

                SlingProjectileList.Remove(SlingProjectileList[0]);
            }
        }
        else
        {
            await Task.Yield();
        }
        _target = null;
    }

    private async Task Whip()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;

            var whip = obj.gameObject.GetComponent<WhipProjetile>();
            SetProjectile(whip, false);

            obj.gameObject.SetActive(true);
            whip.WhipAttack();
            await Delay(0.25f);
        }
    }

    private async Task SpiderWeb()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;

            var web = obj.gameObject.GetComponent<SpiderWebProjectile>();
            SetSkill(GameManager.AIManager.EnemyList);

            SetProjectile(web, false);
            obj.gameObject.SetActive(true);
            GameManager.AIManager.EnemyList.Remove(_target);
            await Delay(0.25f);
        }
    }
    private async Task SpiderPoisonWeb()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;

            var web = obj.gameObject.GetComponent<SpiderPoisonProjectile>();
            SetSkill(GameManager.AIManager.EnemyList);

            SetProjectile(web, false);
            obj.gameObject.SetActive(true);
            GameManager.AIManager.EnemyList.Remove(_target);
            await Delay(0.25f);
        }
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
            if (GameManager.InputManager.isMovementStop)
            {
                var x = UnityEngine.Random.Range(-1f, 1f);
                var y = UnityEngine.Random.Range(-1f, 1f);
                mDirection = (new Vector3(x, y)).normalized;
            }
            else
            {
                mDirection = GameManager.JoystickManager.variableJoystick.LastDirection.normalized;
            }
            if (mDirection == Vector3.zero)
            {
                SetSkill(GameManager.AIManager.EnemyList);
            }
            ActiveBeeShots.Add(mBeeShot);
            mBeeShot.index = BeeIndex;
            BeeIndex++;

            SetProjectile(mBeeShot,false);
            mBeeShot.GetDirection(mBeeShot.Direction);
                
            obj.gameObject.SetActive(true);
            //GameManager.AIManager.EnemyList.Remove(_target);
            await Delay(0.25f);
        }
    }
    
    private async Task BirdBomb()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Pooler.GetFromPool();
            obj.transform.position = this.transform.position;

            var mBomb = obj.gameObject.GetComponent<BirdBombProjectile>();
            SetProjectile(mBomb, false);
            obj.gameObject.SetActive(true);
            mBomb.Drop();
            await Delay(0.25f);
        }
    }
    
    private void SkunkGas()
    {
        var obj = Pooler.GetFromPool();
        obj.transform.position = this.transform.position;
        var mSkunk = obj.gameObject.GetComponent<SkunkGasProjectile>();
        mSkunk.Player = mPlayer;
        SetProjectile(mSkunk, true);
        obj.gameObject.SetActive(true);
        mSkunk.Model.gameObject.transform.DOScale(new Vector3(0.3f,0.3f,0.3f), 0.25f);
    }

    private void BananaGuardian() 
    {
        var obj = Pooler.GetFromPool();
        obj.transform.position = this.transform.position;
        var mBanana = obj.gameObject.GetComponent<BananaMainProjectile>();
        mBanana.Player = mPlayer;
        mBanana.BarrelPooler = BarrelPooler;
        SetProjectile(mBanana, true);
        obj.gameObject.SetActive(true);
    }

    async Task<int> Delay(float delay)
    {
        var mDelay = (int)(delay * 1000);
        await Task.Delay(mDelay);
        return mDelay;
    }
    private void SetProjectile(ProjectileBase projectileBase, bool isAoE)
    {
        projectileBase.Initialize(GameManager);
        projectileBase.Pooler = Pooler;
        projectileBase.Speed = BaseSpeed;
        projectileBase.Damage = BaseDamage;
        projectileBase.Direction = mDirection;
        projectileBase.IsAoE = isAoE;
        projectileBase.IsReady = true;
    }

    private Transform GetClosestEnemy(List<Transform> enemies)
    {
        //Transform bestTarget = null;
        
        float closestDistanceSqr = AttackRange;

         var  currentPosition = mPlayer.transform.position;

        
        foreach (Transform potentialTarget in enemies)
        {
            if (!GameManager.IsBossTime)
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
            else
            {
                _target = GameManager.SpawnerManager.BossSpawner.ActiveBossTransform;
            }
            
        }

        return _target;
    }


}
