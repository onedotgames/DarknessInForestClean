using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class WeaponBase :CustomBehaviour
{
    public SkillSO SkillSO;
    public int UpgradeLevel = 0;
    [HideInInspector]
    public bool IsActivated = false;
    [HideInInspector]
    public bool IsInitialized = false;
    [HideInInspector]
    public Transform Target;
    public Vector3 playerCurLook;
    protected Vector3 mDirection;
    public ParticleSystem HitEffect;
    public bool isMiniGameComplete = false;
    public GameObject[] minigames;
    public GameObject[] bananas;
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
    #endregion

    protected Player mPlayer;

    public StatList StatList = new StatList();
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(gameManager != null)
        {
            GameManager.OnMiniGame += OnMiniGame;
            GameManager.OnStartGame += OnGameStart;

            GameManager.OnLevelFailed += OnGameFailed;

        }
        mPlayer = gameManager.PlayerManager.CurrentPlayer;
        PoolerBase = gameManager.PoolingManager.WeaponPooler[(int)SkillSO.PoolerType];
        SetStats();        
    }

    public void SetStats()
    {
        if(UpgradeLevel == 0)
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

    public void UpdateWeapon()
    {

        if(UpgradeLevel < 4)
        {
            switch (SkillSO.UpgradeDatas[UpgradeLevel].UpgradeType)
            {
                case UpgradeType.PropertyChange:
                    MakePropertyReadyForChange(SkillSO.UpgradeDatas[UpgradeLevel].PropertyToChange);
                    break;
            }

        }
    }

    public void ChangeProperty(float propertyToChange, PropertyChangeType propertyChangeType, float changeAmount)
    {
        if(propertyChangeType == PropertyChangeType.Multiplication)
        {
            propertyToChange = propertyToChange * changeAmount;
        }
        else if (propertyChangeType == PropertyChangeType.Addition)
        {
            propertyToChange = propertyToChange + changeAmount;
        }
    }

    public void MakePropertyReadyForChange(PropertyToChange propertyToChange)
    {
        switch (propertyToChange)
        { 
            case PropertyToChange.Damage:
                if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    BaseDamage *=  (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if (SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    BaseDamage += (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }

                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

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

                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

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

                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

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

                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

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

                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

                break;
            case PropertyToChange.Size:
                if(SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                {
                    Size *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                else if(SkillSO.UpgradeDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                {
                    Size *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                }
                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;

                if (poisonArea != null)
                    poisonArea.transform.localScale = Size;

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

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
                StatList.BaseDamage = BaseDamage;
                StatList.BaseSpeed = BaseSpeed;
                StatList.Count = Count;
                StatList.Cooldown = Cooldown;
                StatList.AttackRange = AttackRange;
                StatList.Size = Size;
                StatList.Guardian = Guardian;
                //Guardian da yeni bir muz a?.
                for (int i = 0; i <= StatList.Guardian; i++)
                {
                    bananas[i].SetActive(true);
                }

                if (GameManager.WeaponManager.SelectSkillPanel.gameObject.activeInHierarchy)
                {
                    GameManager.WeaponManager.SelectSkillPanel.ClosePanel();
                }
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }
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
                GameManager.WeaponManager.InitialWeaponList.Remove(this);
            }
            else if (this.SkillSO.DamagePattern == DamagePattern.BananaGuardian)
            {
                this.BaseDamage *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                GameManager.WeaponManager.InitialWeaponList.Remove(this);
            }
            else
            {
                this.AttackRange *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Count *= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                this.Cooldown /= (int)SkillSO.UpgradeDatas[UpgradeLevel].ChangeAmount;
                GameManager.WeaponManager.InitialWeaponList.Remove(this);
            }
        }
        this.StatList.BaseDamage = BaseDamage;
        this.StatList.BaseSpeed = BaseSpeed;
        this.StatList.Count = Count;
        this.StatList.Cooldown = Cooldown;
        this.StatList.AttackRange = AttackRange;
        this.StatList.Size = Size;
        GameManager.WeaponManager.InitialWeaponList.Remove(GameManager.WeaponManager.selectedWeaponData.Weapon);
    }

    public virtual void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            if (IsActivated)
            {
                AttackMethod();
                MovementMethod();
            }
        }
        
    }

    private void OnMiniGame()
    {
        PlayMinigame(Random.Range(0, minigames.Length));
    }
    public void PlayMinigame(int gameIndex)
    {
        Time.timeScale = 0;
        Debug.Log(minigames[gameIndex]);
        minigames[gameIndex].SetActive(true);
    }
    public virtual void SetSkill(List<Transform> enemies)
    {
        Target = GetClosestEnemy(enemies);
        
        if(Target != null)
        {
            mDirection = Vector3.Normalize(Target.position - transform.position);
        }
        else
        {
            var x = Random.Range(-1f, 1f);
            var y = Random.Range(-1f, 1f);
            mDirection =  (new Vector3(x, y)).normalized;
        }
        if (this.SkillSO.DamagePattern == DamagePattern.Yoyo)
        {
            var x = Random.Range(-1f, 1f);
            var y = Random.Range(-1f, 1f);
            mDirection = (new Vector3(x, y)).normalized;
        }
        IsActivated = true;
    }

    public void ActivateWeapon()
    {

    }

    public virtual void AttackMethod()
    {
        
    }
    
    public virtual void MovementMethod()
    {

    }
        
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            switch (SkillSO.DamagePattern)
            {
                case DamagePattern.Projectile:                    
                    enemy.GetHit(BaseDamage);
                    IsActivated = false;
                    if(HitEffect != null)
                    {
                        HitEffect.gameObject.SetActive(true);
                        HitEffect.Play();
                    }                    
                    PoolerBase.ReturnObjectToPool(gameObject);
                    break;
                case DamagePattern.Area:
                    enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(BaseDamage,Cooldown));
                    break;
                case DamagePattern.Yoyo:
                    enemy.GetHit(BaseDamage);
                    //IsActivated = false;
                    break;
                case DamagePattern.Shotgun:
                    enemy.GetHit(BaseDamage);
                    IsActivated = false;
                    PoolerBase.ReturnObjectToPool(gameObject);
                    break;
                case DamagePattern.Whip:
                    enemy.GetHit(BaseDamage);
                    Debug.Log("Infected " + BaseDamage + " Damage");
                    break;
                case DamagePattern.Bomb:
                    enemy.GetHit(BaseDamage);
                    
                    if (HitEffect != null && IsActivated)
                    {
                        IsActivated = false;
                        HitEffect.gameObject.SetActive(true);
                        HitEffect.Play();
                    }
                    break;
                case DamagePattern.SkunkGas:
                    enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(BaseDamage,Cooldown));
                    break;
                case DamagePattern.BananaGuardian:
                    enemy.GetHit(BaseDamage);
                    break;
            }
        }

        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            switch (SkillSO.DamagePattern)
            {
                case DamagePattern.Projectile:
                    enemy.GetHit(BaseDamage);
                    IsActivated = false;
                    if (HitEffect != null)
                    {
                        HitEffect.gameObject.SetActive(true);
                        HitEffect.Play();
                    }
                    PoolerBase.ReturnObjectToPool(gameObject);
                    break;
                case DamagePattern.Area:
                    enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(BaseDamage, Cooldown));
                    break;
                case DamagePattern.Yoyo:
                    enemy.GetHit(BaseDamage);
                    //IsActivated = false;
                    break;
                case DamagePattern.Shotgun:
                    enemy.GetHit(BaseDamage);
                    IsActivated = false;
                    PoolerBase.ReturnObjectToPool(gameObject);
                    break;
                case DamagePattern.Whip:
                    enemy.GetHit(BaseDamage);
                    Debug.Log("Infected " + BaseDamage + " Damage");
                    break;
                case DamagePattern.Bomb:
                    enemy.GetHit(BaseDamage);

                    if (HitEffect != null && IsActivated)
                    {
                        IsActivated = false;
                        HitEffect.gameObject.SetActive(true);
                        HitEffect.Play();
                    }
                    break;
            }
        }



        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            BarrelPooler.ReturnObjectToPool(collision.gameObject);
            GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = Random.Range(0, 10);
            if(k < 3)//bomba?
            {
                var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if(k >= 3 && k > 6 )// magne?t
            {
                var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if(k >= 6) //co?in
            {
                var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            switch (SkillSO.DamagePattern)
            {
                case DamagePattern.Area:
                    if(enemy.AOEDamageRoutine != null)
                    {
                        enemy.StopCoroutine(enemy.AOEDamageRoutine);
                    }
                    
                    break;
            }
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            switch (SkillSO.DamagePattern)
            {
                case DamagePattern.Area:
                    if (enemy.AOEDamageRoutine != null)
                    {
                        enemy.StopCoroutine(enemy.AOEDamageRoutine);
                    }

                    break;
            }
        }
    }

    private Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = AttackRange;
        Vector3 currentPosition = mPlayer.transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    private void OnGameFailed()
    {
        IsActivated = false;
        PoolerBase.ReturnObjectToPool(gameObject);
        StopAllCoroutines();
    }
    private void OnGameStart()
    {
        UpgradeLevel = 0;
        SetStats();

    }
    private void OnBecameInvisible()
    {
        if(SkillSO.DamagePattern == DamagePattern.Projectile || SkillSO.DamagePattern == DamagePattern.Shotgun)
        {
            IsActivated = false;
            PoolerBase.ReturnObjectToPool(gameObject);
        }        
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed -= OnGameFailed;
            GameManager.OnStartGame -= OnGameStart;
            GameManager.OnMiniGame -= OnMiniGame;
        }
    }
}

[System.Serializable]
public class StatList
{
    public float BaseDamage;

    public float BaseSpeed;

    public float AttackRange;

    public float Cooldown;

    public int Count;

    public Vector3 Size;

    public int Guardian;
}
