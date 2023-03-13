using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBombProjectile : ProjectileBase
{
    Vector3 movePosition;
    [SerializeField] private ParticleSystem HitMarkVFX;
    [SerializeField] private CircleCollider2D _circleCollider2D;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if (GameManager.IsMiniGame && Model.activeSelf)
        {
            Model.SetActive(false);
            Return();

        }
    }

    public void Drop()
    {
        _circleCollider2D.enabled = false;

        Model.SetActive(true);
        movePosition = new Vector2(Random.Range(-5, 5), Random.Range(15, 20)) + (Vector2)GameManager.PlayerManager.CurrentPlayer.transform.position;
        transform.position = movePosition;
        DestroyBomb();

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            enemy.GetHit(Damage);
        }
        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            BarrelPooler.ReturnObjectToPool(collision.gameObject);
            GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = Random.Range(0, 10);
            if (k < 3)//bomba?
            {
                var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if (k >= 3 && k > 6)// magne?t
            {
                var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if (k >= 6) //co?in
            {
                var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            enemy.GetHit(Damage);
        }
       

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.GetComponent<TowerSystem>().GetHitTower(Damage);
        }
    }

    public void DestroyBomb()
    {
        var mDropPoint = new Vector3(transform.position.x, GameManager.PlayerManager.CurrentPlayer.transform.position.y - Random.Range(-5, 5), 0);
        HitMarkVFX.gameObject.transform.position = mDropPoint;
        HitMarkVFX.gameObject.transform.parent = null;
        OneTimePlayVFX(HitMarkVFX);
        transform.DOMove(mDropPoint,1f).OnComplete(() =>
        {
            HitMarkVFX.Stop();
            HitMarkVFX.gameObject.SetActive(false);
            Model.SetActive(false);
            _circleCollider2D.enabled = true;
            if (HitVFX != null && IsReady)
            {
                IsReady = false;
                OneTimePlayVFX(HitVFX);
                Invoke("Return", 2f);
            }
        });
    }

    protected override void Return()
    {
        HitMarkVFX.transform.parent = this.gameObject.transform;
        HitMarkVFX.gameObject.transform.position = Vector3.zero;
        base.Return();
    }
}
