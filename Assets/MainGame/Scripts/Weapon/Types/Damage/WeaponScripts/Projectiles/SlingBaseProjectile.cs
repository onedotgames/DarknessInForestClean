using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlingBaseProjectile : ProjectileBase
{
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        TriggerReturn(5f);
    }
    private void OnEnable()
    {
        float horizontal = GameManager.JoystickManager.GetHorizontal();
        float vertical = GameManager.JoystickManager.GetVertical();

        if (horizontal != 0 || vertical != 0)
        {

            if (GameManager.PlayerManager.CurrentPlayer.Angle > 0 && GameManager.PlayerManager.CurrentPlayer.Angle < 180)
            {
                Direction = GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
            else
            {
                Direction = -1 * GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
        }
        else if (horizontal == 0 && vertical == 0)
        {
            if (GameManager.PlayerManager.CurrentPlayer.LastAngle > 0 && GameManager.PlayerManager.CurrentPlayer.LastAngle < 180)
            {
                Direction = GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
            else
            {
                Direction = -1 * GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
        }
    }
    private void Update()
    {
        LinearMovement(Direction);
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
            var k = UnityEngine.Random.Range(0, 13);
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



    public void GetDirection(Vector3 direction)
    {
        if (GameManager.JoystickManager.variableJoystick.LastDirection.normalized == Vector2.zero)
        {
            direction = Vector2.left;
            Debug.Log(Direction);
        }

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45));
        Model.transform.rotation = rot;

    }

    //private void OnBecameInvisible()
    //{
    //    Return();
    //}
}
