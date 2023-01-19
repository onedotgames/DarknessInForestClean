using Assets.FantasyMonsters.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBase : CustomBehaviour
{
    public delegate void AttackDelegate();
    protected AttackDelegate AttackMethod1;
    protected AttackDelegate AttackMethod2;
    protected AttackDelegate AttackMethod3;

    public delegate void MovementDelegate();
    protected MovementDelegate MovementMethod1;
    protected MovementDelegate MovementMethod2;
    protected MovementDelegate MovementMethod3;

    public GameObject Model;
    public GameObject ExperienceToDrop;
    public GameObject CoinToDrop;

    public ParticleSystem Attack1;
    public ParticleSystem Attack2;
    public ParticleSystem Attack3;
    public ParticleSystem DeathEffect;

    public GameObject ChargeIndicator;
    public GameObject ChargeIndicatorParent;
    public GameObject Destination;

    public BossStats Stats;
    public float BaseHealth;
    public float BaseMoveSpeed;
    public float BaseDamage;
    public float BaseRange;
    public float BaseAttackCooldown;

    private float currentHP;

    public int ChargeCount;
    public float StartDelay;
    public float ChargeBuildUpTime;
    public float ChargeTime;
    public float TimeBtwCharges;
    public Transform MomentaryPlayerTransform;

    public Coroutine AOEDamageRoutine;

    public GameObject Hand;

    public bool IsActivated = false;
    public bool ShouldMove = false;
    public bool CanAttack = true;
    public bool ShouldRotate = false;
    public bool ShouldIndicatorRotate = false;
    public bool IsPunchable = true;

    public Player Player;
    public Animator Anim;
    public Collider2D Collider2D;
    public Monster Monster;


    public SpriteRenderer BossHeadRenderer;
    public Sprite AttackHead;
    public Sprite RunHead;

    private HUD hud;
    [SerializeField] private Vector3 _originalScale;
    private Tweener punchTween;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (gameManager != null)
        {            
            
            gameManager.OnLevelCompleted += OnGameSuccess;
            gameManager.OnLevelFailed += OnGameFailed;
        }
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        Player = gameManager.PlayerManager.CurrentPlayer;
        gameManager.AIManager.EnemyList.Add(this.transform);
        transform.localScale = _originalScale;
        SetStats();
        IsActivated = true;
        Collider2D.enabled = true;
        BossHeadRenderer.sprite = RunHead;
        Anim.SetInteger("State", 0);
        Anim.SetBool("Action", false);
        SetMovementPattern();
        SetAttackPattern();
        punchTween = transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f).SetRecyclable(true).SetAutoKill(false).OnComplete(() =>
        {
            IsPunchable = true;
            transform.localScale = _originalScale;
        }
        ); 
        hud.SetBossHPBarActivation(true);
        hud.SetBossFillValue(currentHP,BaseHealth);
        hud.SetBossFillText(currentHP, BaseHealth);
        //gameManager.AIManager.EnemyList.Add(this.transform);

        Monster.ResetAttack();
        Monster.ChangeAction(false);
        Monster.SetState(MonsterState.Run);

    }

    public void PunchEffect()
    {
        if (IsPunchable)
        {
            IsPunchable = false;
            punchTween.Restart();
        }
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            if (IsActivated)
            {
                if (ShouldRotate)
                {
                    BossRotation();
                }

                if (ShouldIndicatorRotate)
                {
                    IndicatorRotation();
                }

                if (ShouldMove)
                {
                    if (MovementMethod1 != null)
                    {
                        MovementMethod1();
                    }
                    if (MovementMethod2 != null)
                    {
                        MovementMethod2();
                    }
                    if (MovementMethod3 != null)
                    {
                        MovementMethod3();
                    }
                }
            }
        } 
    }
    private void SetStats()
    {
        BaseHealth = Stats.BaseHealth;
        BaseMoveSpeed = Stats.BaseMoveSpeed;
        BaseDamage = Stats.Damage;
        BaseRange = Stats.AttackRange;
        BaseAttackCooldown = Stats.AttackCooldown;

        currentHP = BaseHealth;
    }

    protected virtual void SetAttackPattern()
    {
        
    }

    protected virtual void SetMovementPattern()
    {

    }

    public void Stationary()
    {
        Debug.Log("Enemy is not moving");
    }

    public void ChasePlayer()
    {
        if (!GameManager.IsGamePaused)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) < BaseRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, BaseMoveSpeed * Time.deltaTime);

            }
        }
        
    }

    public IEnumerator ChargeAttack(float startDelay,int chargeCount, float chargeBuilUpTime, float chargeTime, float chargeCooldown, float timeBetweenCharges)
    {
        while (GameManager.IsGamePaused)
        {
            yield return null;
        }
        if (CanAttack)
        {
            yield return new WaitForSeconds(StartDelay);
            CanAttack = false;
        }

        for (int i = 0; i < chargeCount; i++)
        {
            if (ShouldMove)
            {
                ShouldMove = false;
                Monster.ResetAttack();
                Monster.SetState(MonsterState.Idle);
            }

            ShouldRotate = true;
            ShouldIndicatorRotate = true;

            ChargeIndicator.SetActive(true);
            ChargeIndicator.transform.DOScaleY(7.5f, chargeBuilUpTime).OnComplete(() =>
            {
                ShouldIndicatorRotate = false;

                var temp = Destination.transform.position;
                ChargeIndicator.SetActive(false);
                ChargeIndicator.transform.localScale = new Vector3(2, 1, 1);
                Monster.ChangeAction(true);
                Monster.Attack();

                gameObject.transform.DOMove(temp, chargeTime).OnComplete(() =>
                {
                    Monster.ChangeAction(false);
                    Monster.ResetAttack();
                    Monster.SetState(MonsterState.Ready);

                });
            });
            yield return new WaitForSeconds(chargeTime);
            yield return new WaitForSeconds(timeBetweenCharges);
        }

        yield return new WaitForSeconds(chargeCooldown);
        ShouldMove = true;
        Monster.ResetAttack();
        Monster.ChangeAction(false);
        Monster.SetState(MonsterState.Run);


        StartCoroutine(ChargeAttack(StartDelay, ChargeCount, ChargeBuildUpTime, ChargeTime, BaseAttackCooldown, TimeBtwCharges));
    }

    public IEnumerator ChargeAttackWithoutIndicatorFollow(float startDelay, int chargeCount, float chargeBuilUpTime, float chargeTime, float chargeCooldown, float timeBetweenCharges)
    {
        while (GameManager.IsGamePaused)
        {
            yield return null;
        }
        ShouldIndicatorRotate = false;
        if (CanAttack)
        {
            yield return new WaitForSeconds(StartDelay);
            CanAttack = false;
        }

        for (int i = 0; i < chargeCount; i++)
        {
            if (ShouldMove)
            {
                ShouldMove = false;
                Monster.ResetAttack();
                Monster.SetState(MonsterState.Idle);
            }

            ShouldRotate = true;
            IndicatorRotation();

            ChargeIndicator.SetActive(true);
            ChargeIndicator.transform.DOScaleY(7.5f, chargeBuilUpTime).OnComplete(() =>
            {
                var temp = Destination.transform.position;
                ChargeIndicator.SetActive(false);
                ChargeIndicator.transform.localScale = new Vector3(2, 1, 1);
                Monster.ChangeAction(true);
                Monster.Attack();

                gameObject.transform.DOMove(temp, chargeTime).OnComplete(() =>
                {
                    Monster.ChangeAction(false);
                    Monster.ResetAttack();
                    Monster.SetState(MonsterState.Ready);

                });
            });
            yield return new WaitForSeconds(chargeTime);
            yield return new WaitForSeconds(timeBetweenCharges);
        }

        yield return new WaitForSeconds(chargeCooldown);
        ShouldMove = true;
        Monster.ResetAttack();
        Monster.ChangeAction(false);
        Monster.SetState(MonsterState.Run);

        StartCoroutine(ChargeAttackWithoutIndicatorFollow(StartDelay, ChargeCount, ChargeBuildUpTime, ChargeTime, BaseAttackCooldown, TimeBtwCharges));
    }

    protected IEnumerator RapidFire(float startDelay, int shotCount, float shotCooldown, float timeBetweenShots)
    {
        while (GameManager.IsGamePaused)
        {
            yield return null;
        }
        if (CanAttack)
        {
            yield return new WaitForSeconds(StartDelay);
            CanAttack = false;
        }
        for (int i = 0; i < shotCount; i++)
        {
            if (ShouldMove)
            {
                ShouldMove = false;
                Monster.ChangeAction(true);
                Monster.Attack();
            }

            var bullet = GameManager.PoolingManager.EnemyBulletPoolerList[(int)EnemyBulletPoolerType.BossClub].GetObjectFromPool();
            if(Hand != null)
            {
                if(bullet != null)
                {
                    bullet.transform.position = Hand.transform.position;

                }
                else
                {
                    Debug.Log("NO Bullet");
                }

            }
            else
            {
                bullet.transform.position = transform.position;

            }

            var bulletShot = bullet.GetComponent<BossRotatingProjectile>();

            bulletShot.gm = GameManager;
            GameManager.OnLevelFailed += bulletShot.OnGameFailed;

            bulletShot.mDirection = Player.transform.position - transform.position;
            bulletShot.DirectionNorm = bulletShot.mDirection.normalized;
            if(bulletShot.DirectionNorm == Vector3.zero)
            {
                bulletShot.DirectionNorm = Vector3.down;
            }
            bulletShot.PoolerBase = GameManager.PoolingManager.EnemyBulletPoolerList[(int)EnemyBulletPoolerType.BossClub];
            bulletShot.isShotted = true;
            bulletShot.damage = BaseDamage;
            i++;
            yield return new WaitForSeconds(timeBetweenShots);
        }
        ShouldMove = true;
        Monster.ResetAttack();
        Monster.ChangeAction(false);
        Monster.SetState(MonsterState.Run);
        yield return new WaitForSeconds(shotCooldown);
        StartCoroutine(RapidFire(StartDelay, ChargeCount, BaseAttackCooldown, TimeBtwCharges));
    }

    private void BossRotation()
    {
        if (transform.position.x - GameManager.PlayerManager.CurrentPlayer.transform.position.x <= 0)
        {
            Model.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Model.transform.eulerAngles = Vector3.zero;
        }
    }

    private void IndicatorRotation()
    {
        Vector2 direction = Player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ChargeIndicatorParent.transform.eulerAngles = new Vector3(0, 0, angle + 90);
    }

    public virtual void GetHit(float damageToTake)
    { 
        currentHP -= damageToTake;
        hud.SetBossFillValue(currentHP, BaseHealth);
        hud.SetBossFillText(currentHP, BaseHealth);
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (currentHP <= 0 && IsActivated)
        {
            IsActivated = false;

            hud.killCount++;
            hud.KillCount.text = hud.killCount.ToString();
            hud.BossHpGroup.SetActive(false);
            if (AOEDamageRoutine != null)
            {
                StopCoroutine(AOEDamageRoutine);
            }
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        PlayDeathVFX();
        //GameManager.PlayerManager.PlayerSkillHandler.TargetList.Remove(this.gameObject.transform);
        Collider2D.enabled = false;
    }

    public virtual IEnumerator GetAOEHit(float damageToTake, float interval)
    {
        while (GameManager.IsGamePaused)
        {
            yield return null;
        }
        currentHP -= damageToTake;
        hud.SetBossFillValue(currentHP, BaseHealth);
        hud.SetBossFillText(currentHP, BaseHealth);
        CheckDeath();
        if (IsActivated && gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(interval);
            StartCoroutine(GetAOEHit(damageToTake, interval));
        }

    }

    public void PlayDeathVFX()
    {
        GameManager.SpawnerManager.BossSpawner.BossRing.SetActive(false);
        Monster.Die();
        //random range 0,1000 if 1 se gold else exp.
        var i = Random.Range(0, 1000);
        if (i == 1)
        {
            Debug.Log(gameObject.name + " Coin bırakıyor");
            Invoke("DropCoin", Anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.Log(gameObject.name + " Exp bırakıyor");
            Invoke("DropExp", Anim.GetCurrentAnimatorStateInfo(0).length);
        }
        GameManager.IsBossTime = false;
    }
    private void DropExp()
    {
        if (!GameManager.IsDevelopmentModeOn)
        {
            var exp = GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType.SmallExperience].GetObjectFromPool();
            exp.transform.position = transform.position;
        }
        //GameManager.AIManager.EnemyList.Remove(transform);

        CheckIfLastBoss();
        gameObject.SetActive(false);
    }

    private void DropCoin()
    {
        if (!GameManager.IsDevelopmentModeOn)
        {
            var coin = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small].GetObjectFromPool();
            coin.transform.position = transform.position;
        }
        //GameManager.AIManager.EnemyList.Remove(transform);

        CheckIfLastBoss();
        gameObject.SetActive(false);
    }

    private void CheckIfLastBoss()
    {
        if(GameManager.SpawnerManager.BossSpawner.Boss3 == this)
        {
            Debug.Log("Son Boss yenildi, bölümü sonlandırıyorum");
            GameManager.LevelCompleted();
        }
    }

    private void OnGameFailed()
    {
        gameObject.SetActive(false);
        IsActivated = false;
        ShouldMove = false;
        CanAttack = true;
        ShouldRotate = false;
        ShouldIndicatorRotate = false;
        StopAllCoroutines();

    }
    private void OnGameSuccess()
    {
        Debug.Log("Success");
        gameObject.SetActive(false);
        IsActivated = false;
        ShouldMove = false;
        CanAttack = true;
        ShouldRotate = false;
        ShouldIndicatorRotate = false;
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= OnGameFailed;
            GameManager.OnLevelCompleted -= OnGameSuccess;
        }
    }
}
