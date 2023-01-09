using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloverProjectile : ProjectileBase
{

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if(!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            ContinueuslyPlayVFX(MovementVFX);
            RotateModel();
            LinearMovement(Direction);
        }
    }

    private void LevelEnd()
    {
        IsReady = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            if (IsAoE)
            {
                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(Damage, AoETickInterval));
            }
            else
            {
                
                enemy.GetHit(Damage);
                Return();
            }           
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            if (IsAoE)
            {
                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(Damage, AoETickInterval));
            }
            else
            {
                enemy.GetHit(Damage);
                Return();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            if (IsAoE)
            {
                if (enemy.AOEDamageRoutine != null)
                {
                    enemy.StopCoroutine(enemy.AOEDamageRoutine);
                }
            }
            
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            if (IsAoE)
            {
                if (enemy.AOEDamageRoutine != null)
                {
                    enemy.StopCoroutine(enemy.AOEDamageRoutine);
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        Return();
    }
    //switch (SkillSO.DamagePattern)
    //{
    //    case DamagePattern.Projectile:
    //        enemy.GetHit(BaseDamage);
    //        IsActivated = false;
    //        if (HitEffect != null)
    //        {
    //            HitEffect.gameObject.SetActive(true);
    //            HitEffect.Play();
    //        }
    //        Pooler.ReturnObjectToPool(gameObject.GetComponent<ProjectileToSpawn>());
    //        break;
    //    case DamagePattern.Area:
    //        enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(BaseDamage, Cooldown));
    //        break;
    //    case DamagePattern.Yoyo:
    //        enemy.GetHit(BaseDamage);
    //        //IsActivated = false;
    //        break;
    //    case DamagePattern.Shotgun:
    //        enemy.GetHit(BaseDamage);
    //        IsActivated = false;
    //        Pooler.ReturnObjectToPool(gameObject.GetComponent<ProjectileToSpawn>());

    //        break;
    //    case DamagePattern.Whip:
    //        enemy.GetHit(BaseDamage);
    //        Debug.Log("Infected " + BaseDamage + " Damage");
    //        break;
    //    case DamagePattern.Bomb:
    //        enemy.GetHit(BaseDamage);

    //        if (HitEffect != null && IsActivated)
    //        {
    //            IsActivated = false;
    //            HitEffect.gameObject.SetActive(true);
    //            HitEffect.Play();
    //        }
    //        break;
    //    case DamagePattern.SkunkGas:
    //        enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(BaseDamage, Cooldown));
    //        break;
    //    case DamagePattern.BananaGuardian:
    //        enemy.GetHit(BaseDamage);
    // 
    //    if (collision.CompareTag("Boss"))
    //    {
    //        var enemy = collision.GetComponent<BossBase>();
    //        switch (SkillSO.DamagePattern)
    //        {
    //            case DamagePattern.Projectile:
    //                enemy.GetHit(BaseDamage);
    //                IsActivated = false;
    //                if (HitEffect != null)
    //                {
    //                    HitEffect.gameObject.SetActive(true);
    //                    HitEffect.Play();
    //                }
    //                PoolerBase.ReturnObjectToPool(gameObject);
    //                break;
    //            case DamagePattern.Area:
    //                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(BaseDamage, Cooldown));
    //                break;
    //            case DamagePattern.Yoyo:
    //                enemy.GetHit(BaseDamage);
    //                //IsActivated = false;
    //                break;
    //            case DamagePattern.Shotgun:
    //                enemy.GetHit(BaseDamage);
    //                IsActivated = false;
    //                Pooler.ReturnObjectToPool(gameObject.GetComponent<ProjectileToSpawn>());

    //                break;
    //            case DamagePattern.Whip:
    //                enemy.GetHit(BaseDamage);
    //                Debug.Log("Infected " + BaseDamage + " Damage");
    //                break;
    //            case DamagePattern.Bomb:
    //                enemy.GetHit(BaseDamage);

    //                if (HitEffect != null && IsActivated)
    //                {
    //                    IsActivated = false;
    //                    HitEffect.gameObject.SetActive(true);
    //                    HitEffect.Play();
    //                }
    //                break;
    //        }
    //    }

    //    if (collision.CompareTag("Barrel"))
    //    {
    //        var barrelPos = collision.transform.position;
    //        BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
    //        BarrelPooler.ReturnObjectToPool(collision.gameObject);
    //        GameManager.BarrelSystem.barrelCount--;
    //        // coin magnet ya da bomb spawn olacak.
    //        var k = Random.Range(0, 13);
    //        if (k < 3)//bomba?
    //        {
    //            var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
    //            var bomb = bombPool.GetObjectFromPool();
    //            bomb.transform.position = barrelPos;
    //        }
    //        else if (k >= 3 && k < 6)// magne?t
    //        {
    //            var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
    //            var magnet = magnetPool.GetObjectFromPool();
    //            magnet.transform.position = barrelPos;
    //        }
    //        else if (k >= 6 && k < 9) //co?in
    //        {
    //            var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
    //            var coin = coinPool.GetObjectFromPool();
    //            coin.transform.position = barrelPos;
    //        }
    //        else if (k >= 9 && k < 13)//healthPot
    //        {
    //            var healthPotPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.HealthPotPooler];
    //            var healthPot = healthPotPool.GetObjectFromPool();
    //            healthPot.transform.position = barrelPos;
    //        }
    //    }

    //    if (collision.gameObject.CompareTag("Tower"))
    //    {
    //        collision.GetComponent<TowerSystem>().GetHitTower(BaseDamage);
    //    }
    //}


}
