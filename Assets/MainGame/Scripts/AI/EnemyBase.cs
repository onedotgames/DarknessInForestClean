using Assets.FantasyMonsters.Scripts;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBase : CustomBehaviour
{
    public List<SpriteRenderer> EnemySprites = new List<SpriteRenderer>();
    public List<SpriteRenderer> CleanSprites = new List<SpriteRenderer>();
    public GameObject CleanModel;
    public GameObject SymbiotModel;
    public GameObject Experience;
    public GameObject Coin;
    public ObjectSpawnerV2 Pooler;
    public ObjectToPool ObjectToPool;
    public ParticleSystem DeathVFX;

    public EnemyStats mStats;
    public Collider2D Collider2D;
    public Monster Monster;
    public Monster MonsterClean;

    public float BaseHealth;
    public float BaseSpeed;
    public float BaseDamage;
    public float MeleeRange;
    public float AttackCooldown;

    public bool IsActivated = false;
    public bool CanAttack = true;
    public bool IsPunchable = true;

    public Player Player;

    public EnemyPoolerType EnemyPoolerType;
    public Coroutine AOEDamageRoutine;
    private HUD hud; 
    public Color TempColor;
    private Tweener punchTween;

    private bool isDirtyOver = false;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnLevelCompleted += OnGameCompleted;
            GameManager.OnLevelFailed += OnGameFailed;
            GameManager.OnBossSpawn += OnBossSpawn;

        }
        Player = gameManager.PlayerManager.CurrentPlayer;

        gameManager.AIManager.EnemyList.Add(this.transform);
        gameManager.AIManager.AIList.Add(this);
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        punchTween = transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f).SetRecyclable(true).SetAutoKill(false).OnComplete(() =>
        {
            IsPunchable = true;
            transform.localScale = Vector3.one;
        }
        );
    }

    public void ActivateEnemy()
    {
        Collider2D.enabled = true;
        GameManager.AIManager.EnemyList.Add(this.transform);
        GameManager.AIManager.AIList.Add(this);
        SetStats();
        CleanModel.SetActive(false);
        SymbiotModel.SetActive(true);
        CleanSprites.ForEach(x => x.color = Color.clear);
        EnemySprites.ForEach(x => x.color = Color.white);
        IsActivated = true;
        IsPunchable = true;

    }

    public virtual void Update()
    {
        SymbiotModel.GetComponent<SortingGroup>().sortingOrder = 1;
        if (GameManager != null)
        {
            if (!GameManager.IsGamePaused)
            {
                if (IsActivated)
                {
                    MovementMethod();
                    if (CanAttack)
                    {
                        AttackMethod();
                    }
                    else
                    {
                        AttackCooldown -= Time.deltaTime;
                        if (AttackCooldown <= 0)
                        {
                            CanAttack = true;
                            AttackCooldown = mStats.AttackCooldown;
                        }
                    }
                }
            }
        } 
    }

    private void SetStats()
    {
        BaseHealth = mStats.BaseHealth;
        BaseSpeed = mStats.BaseSpeed;
        BaseDamage = mStats.Damage;
        MeleeRange = mStats.MeleeRange;
        AttackCooldown = mStats.AttackCooldown;
    }

    public void OpenCloseModels(GameObject ActivateModel, GameObject DeactivateModel)
    {
        DeactivateModel.SetActive(false);
        ActivateModel.SetActive(true);
    }

    public void PlayDeathVFX()
    {
        DeathVFX.Play();
        Monster.Die();
        //random range 0,1000 if 1 se gold else exp.
        var i = Random.Range(0, 1000);
        if(i == 1)
        {
            //Invoke("DropCoin", DeathVFX.main.duration);
            Invoke("DropCoin", 0.4f);
        }
        else
        {
            //Invoke("DropExp", DeathVFX.main.duration);
            Invoke("DropExp", 0.4f);
        }
    }
    public void EnemyDeathAnim() //oncomplete e clean modeli a?mak i?in script yaz.
    {
        EnemySprites.ForEach(x => x.DOColor(Color.clear, 0.4f));
        CleanSprites.ForEach(x => x.DOColor(Color.white, 0.4f).OnComplete(() => Invoke("CleanAnim", 0.2f)));
    }
    private void DropExp()
    {
        if (!GameManager.IsDevelopmentModeOn)
        {
            var exp = GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType.SmallExperience].GetObjectFromPool();
            exp.transform.position = transform.position;
        }

        Return();
    }
    private void DropCoin()
    {
        if (!GameManager.IsDevelopmentModeOn)
        {
            var coin = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small].GetObjectFromPool();
            coin.transform.position = transform.position;
        }
        Return();
    }

    public void PunchEffect()
    {
        if (IsPunchable)
        {
            IsPunchable = false;
            punchTween.Restart();
        }
    }

    public void CleanAnim()
    {
        CleanSprites.ForEach(x => x.DOColor(Color.clear, 0.4f));
        var i = Random.Range(0, 1000);
        if (i == 1)
        {
            //Invoke("DropCoin", DeathVFX.main.duration);
            //Invoke("DropCoin", 0.1f);
            DropCoin();
        }
        else
        {
            //Invoke("DropExp", DeathVFX.main.duration);
            //Invoke("DropExp", 0.1f);
            DropExp();
        }
    }
    public virtual void GetHit(float damageToTake)
    {
        if (IsActivated)
        {
            BaseHealth -= damageToTake;
            CheckDeath();
        }
        
    }
    
    public virtual IEnumerator GetAOEHit(float damageToTake, float interval)
    {
        
        if (IsActivated && gameObject.activeInHierarchy)
        {
            BaseHealth -= damageToTake;
            CheckDeath();
            yield return new WaitForSeconds(interval);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(GetAOEHit(damageToTake, interval));
            }        
        }   
    }

    public void CheckDeath()
    {
        if(BaseHealth <= 0)
        {
            IsActivated = false;
            hud.UpdateKillCountBar();
            if (AOEDamageRoutine != null)
            {
                StopCoroutine(AOEDamageRoutine);
            }
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        EnemyDeathAnim();
        Collider2D.enabled = false;
    }

    public virtual void MovementMethod()
    {
        if(transform.position.x - GameManager.PlayerManager.CurrentPlayer.transform.position.x <= 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public virtual void AttackMethod()
    {
        
    }

    private void Return()
    {
        if (gameObject.activeInHierarchy)
        {
            IsActivated = false;
            Collider2D.enabled = false;
            CancelInvoke("DropCoin");
            CancelInvoke("DropExp");
            GameManager.AIManager.EnemyList.Remove(transform);
            GameManager.AIManager.AIList.Remove(this);
            Pooler.ReturnObjectToPool(ObjectToPool);
        }
        
    }

    private void OnGameFailed()
    {
        Return();
    }
    private void OnGameCompleted()
    {
        Return();

    }

    private void OnBossSpawn()
    {
        Return();
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed -= OnGameFailed;
            GameManager.OnLevelCompleted -= OnGameCompleted;
            GameManager.OnBossSpawn -= OnBossSpawn;
        }
    }
}
