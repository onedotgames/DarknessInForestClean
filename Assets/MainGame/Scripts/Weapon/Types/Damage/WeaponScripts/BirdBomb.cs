using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdBomb : WeaponBase 
{
    Vector3 movePosition;
    public GameObject Model;
    //private void OnEnable()
    //{
    //    //movePosition = new Vector3(GameManager.PlayerManager.CurrentPlayer.transform.position.x + Random.Range(-5, 5), 
    //    //    GameManager.PlayerManager.CurrentPlayer.transform.position.y + Random.Range(-5, 5), 0);
    //    //movePosition.z = 0;
    //}

    public void Drop()
    {
        transform.GetComponent<CircleCollider2D>().enabled = false;

        Model.SetActive(true);
        movePosition = (Vector2)GameManager.PlayerManager.CurrentPlayer.transform.position + Random.insideUnitCircle * 5;
        transform.position = movePosition;

    }

    public override void SetSkill(List<Transform> enemies)
    {
        IsActivated = true;
    }

    //public override void AttackMethod()
    //{
    //    //transform.position = movePosition;

    //}

    private void ReturnToPool()
    {
        PoolerBase.ReturnObjectToPool(gameObject);

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            
            enemy.GetHit(BaseDamage);
        }
        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            BarrelPooler.ReturnObjectToPool(collision.gameObject);
            GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = Random.Range(0, 10);
            if (k < 3)//bombað
            {
                var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if (k >= 3 && k > 6)// magneðt
            {
                var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if (k >= 6) //coðin
            {
                var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
        }
    }

    public void DestroyBomb()
    {
        transform.DOMove(new Vector3(transform.position.x + 2, transform.position.y - 2, 0), 1f).OnComplete(() =>
        {
            Model.SetActive(false);
            transform.GetComponent<CircleCollider2D>().enabled = true;
            if (HitEffect != null && IsActivated)
            {
                IsActivated = false;
                this.HitEffect.gameObject.SetActive(true);
                this.HitEffect.Play();
                Invoke("ReturnToPool", HitEffect.main.duration);
            }
        });
    }
}
