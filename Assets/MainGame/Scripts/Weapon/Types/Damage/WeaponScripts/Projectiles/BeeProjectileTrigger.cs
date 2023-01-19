using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeProjectileTrigger : MonoBehaviour
{
    [SerializeField] BeeShotProjectile _beeShotProjectile;
    private void PunchEffect(Transform transform, bool isPunchable)
    {
        if (isPunchable)
        {
            isPunchable = false;
            transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f).OnComplete(() => isPunchable = true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (_beeShotProjectile.ParticlePooler != null && _beeShotProjectile.ParticlePooler.isActiveAndEnabled)
            {
                var obj = _beeShotProjectile.ParticlePooler.Pool.Get();
                obj.gameObject.transform.position = enemy.transform.position;
            }
            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            enemy.GetHit(_beeShotProjectile.Damage);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            var enemy = collision.gameObject.GetComponent<BossBase>();
            if (_beeShotProjectile.ParticlePooler != null && _beeShotProjectile.ParticlePooler.isActiveAndEnabled)
            {
                var obj = _beeShotProjectile.ParticlePooler.Pool.Get();
                obj.gameObject.transform.position = enemy.transform.position;
            }
            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            enemy.GetHit(_beeShotProjectile.Damage);
        }
        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            _beeShotProjectile.BarrelPooler = _beeShotProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            _beeShotProjectile.BarrelPooler.ReturnObjectToPool(collision.gameObject);
            _beeShotProjectile.GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = Random.Range(0, 13);
            if (k < 3)//bomba?
            {
                var bombPool = _beeShotProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if (k >= 3 && k < 6)// magne?t
            {
                var magnetPool = _beeShotProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if (k >= 6 && k < 9) //co?in
            {
                var coinPool = _beeShotProjectile.GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
            else if (k >= 9 && k < 13)//healthPot
            {
                var healthPotPool = _beeShotProjectile.GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.HealthPotPooler];
                var healthPot = healthPotPool.GetObjectFromPool();
                healthPot.transform.position = barrelPos;
            }
        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerSystem>().GetHitTower(_beeShotProjectile.Damage);
        }
    }
}
