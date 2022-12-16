using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CustomBehaviour
{
    public GameObject CleanModel;
    public GameObject SymbiotModel;
    public GameObject Experience;
    public GameObject Coin;

    public ParticleSystem DeathVFX;

    public EnemyStats mStats;
    public Collider2D Collider2D;
    public Monster Monster;

    public float BaseHealth;
    public float BaseSpeed;
    public float BaseDamage;
    public float MeleeRange;
    public float AttackCooldown;

    public bool IsActivated = false;
    public bool CanAttack = true;

    public Player Player;

    public EnemyPoolerType EnemyPoolerType;
    public Coroutine AOEDamageRoutine;
    private HUD hud;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnLevelFailed += OnGameFailed;
        }
        Player = gameManager.PlayerManager.CurrentPlayer;

        gameManager.AIManager.EnemyList.Add(this.transform);
        gameManager.AIManager.AIList.Add(this);
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
    }

    public void ActivateEnemy()
    {
        IsActivated = true;
        Collider2D.enabled = true;
        SetStats();
    }

    public virtual void Update()
    {
        if(GameManager != null)
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
    private void DropExp()
    {
       
        var exp = GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType.SmallExperience].GetObjectFromPool();
        exp.transform.position = transform.position;
        GameManager.AIManager.EnemyList.Remove(transform);
        GameManager.AIManager.AIList.Remove(this);
        GameManager.PoolingManager.EnemyPoolerList[(int)EnemyPoolerType].ReturnObjectToPool(gameObject);
    }
    private void DropCoin()
    {
        var coin = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small].GetObjectFromPool();
        coin.transform.position = transform.position;
        GameManager.AIManager.EnemyList.Remove(transform);
        GameManager.AIManager.AIList.Remove(this);
        GameManager.PoolingManager.EnemyPoolerList[(int)EnemyPoolerType].ReturnObjectToPool(gameObject);
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
        PlayDeathVFX();
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

    private void OnGameFailed()
    {
        if (gameObject.activeInHierarchy)
        {
            IsActivated = false;
            Collider2D.enabled = false;
            CancelInvoke("DropCoin");
            CancelInvoke("DropExp");
            GameManager.PoolingManager.EnemyPoolerList[(int)EnemyPoolerType].ReturnObjectToPool(gameObject);
        }
        
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed -= OnGameFailed;
        }
    }
}
