using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class CloverProjectile : ProjectileBase
{
    [SerializeField] private GameObject PinkTrail;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        TriggerReturn(5f);
    }

    private void Update()
    {
        if(!GameManager.IsGamePaused && GameManager.IsGameStarted && IsReady && !GameManager.IsMiniGame)
        {
            //ContinueuslyPlayVFX(MovementVFX);
            RotateModel();
            LinearMovement(Direction);
        }

        if (GameManager.IsMiniGame && Model.activeSelf)
        {
            Model.SetActive(false);
            MovementVFX.Stop();
            PinkTrail.SetActive(false);
        }
        else if (!GameManager.IsMiniGame && !Model.activeSelf)
        {
            Model.SetActive(true);
            MovementVFX.Play();
            PinkTrail.SetActive(true);
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

            if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
            {
                var obj = ParticlePooler.Pool.Get();
                obj.gameObject.transform.position = enemy.transform.position;
            }
            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();
            enemy.GetHit(Damage);
            CancelReturnTrigger();
            Return();

        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();


            if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
            {
                var obj = ParticlePooler.Pool.Get();
                obj.gameObject.transform.position = enemy.transform.position;
            }

            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            enemy.GetHit(Damage);
            CancelReturnTrigger();
            Return();
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

    private void OnBecameInvisible()
    {
        //Return();
    }
}
