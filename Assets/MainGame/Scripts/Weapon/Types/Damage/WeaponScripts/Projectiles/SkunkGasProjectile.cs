using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkunkGasProjectile : ProjectileBase
{
    private float PoisonAreaDamage = 20f;
    public Player Player;
    private float PoisonDuration = 2f;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            transform.position = Player.gameObject.transform.position;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            enemy.GetHit(Damage);

            if (!enemy.Poisoned)
            {
                enemy.Poisoned = true;
                enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval, PoisonDuration, enemy.Poisoned));
            }         

        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            enemy.GetHit(Damage);

            if (!enemy.Burned)
            {
                enemy.Burned = true;
                enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval, PoisonDuration, enemy.Burned));
            }

        }
        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            BarrelPooler.ReturnObjectToPool(collision.gameObject);
            GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = Random.Range(0, 13);
            if (k < 3)//bomba?
            {
                var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if (k >= 3 && k < 6)// magne?t
            {
                var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if (k >= 6 && k < 9) //co?in
            {
                var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
            else if (k >= 9 && k < 13)//healthPot
            {
                var healthPotPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.HealthPotPooler];
                var healthPot = healthPotPool.GetObjectFromPool();
                healthPot.transform.position = barrelPos;
            }
        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.GetComponent<TowerSystem>().GetHitTower(Damage);
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

            if (enemy.AOEDamageRoutine != null)
            {
                enemy.StopCoroutine(enemy.AOEDamageRoutine);

            }
            
        }
    }
}
