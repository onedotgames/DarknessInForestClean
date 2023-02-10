using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMiniProjectiles : MonoBehaviour
{
    [SerializeField] private BananaMainProjectile bananaMainProjectile;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            enemy.GetHit(bananaMainProjectile.Damage);
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            enemy.GetHit(bananaMainProjectile.Damage);
        }
        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            bananaMainProjectile.BarrelPooler = bananaMainProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            bananaMainProjectile.BarrelPooler.ReturnObjectToPool(collision.gameObject);
            bananaMainProjectile.GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = Random.Range(0, 13);
            if (k < 3)//bomba?
            {
                var bombPool = bananaMainProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if (k >= 3 && k < 6)// magne?t
            {
                var magnetPool = bananaMainProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if (k >= 6 && k < 9) //co?in
            {
                var coinPool = bananaMainProjectile.GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
            else if (k >= 9 && k < 13)//healthPot
            {
                var healthPotPool = bananaMainProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.HealthPotPooler];
                var healthPot = healthPotPool.GetObjectFromPool();
                healthPot.transform.position = barrelPos;
            }
        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.GetComponent<TowerSystem>().GetHitTower(bananaMainProjectile.Damage);
        }
    }
}
