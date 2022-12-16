using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IvyWhip : WeaponBase
{
    public ParticleSystem IvySlash;
    public ContactFilter2D contactFilter2;

    public void WhipAttack()
    {
        float horizontal = GameManager.JoystickManager.GetHorizontal();
        float vertical = GameManager.JoystickManager.GetVertical();



        if (horizontal != 0 || vertical != 0)
        {

            Cast(horizontal,vertical,GameManager.PlayerManager.CurrentPlayer.Angle);
        }
        else if (horizontal == 0 && vertical == 0)
        {
            Cast(horizontal,vertical,GameManager.PlayerManager.CurrentPlayer.LastAngle);
            
        }




        IvySlash.Play();
        
        Invoke("ReturnToPooler", IvySlash.main.duration);
    }

  

    private void Cast(float horizontal, float vertical, float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, angle - 90);

        RaycastHit2D[] results = Physics2D.BoxCastAll(transform.position, new Vector2(1, 3), angle -90, GameManager.JoystickManager.variableJoystick.LastDirection, 6f);

        foreach (var item in results)
        {
            if (item.transform.CompareTag("Enemy"))
            {
                item.transform.GetComponent<EnemyBase>().GetHit(BaseDamage);
            }
            if (item.transform.CompareTag("Barrel"))
            {
                var barrelPos = item.transform.position;
                BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
                BarrelPooler.ReturnObjectToPool(item.transform.gameObject);
                GameManager.BarrelSystem.barrelCount--;
                // coin magnet ya da bomb spawn olacak.
                var k = Random.Range(0, 10);
                if (k < 3)//bombağ
                {
                    var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                    var bomb = bombPool.GetObjectFromPool();
                    bomb.transform.position = barrelPos;
                }
                else if (k >= 3 && k > 6)// magneğt
                {
                    var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                    var magnet = magnetPool.GetObjectFromPool();
                    magnet.transform.position = barrelPos;
                }
                else if (k >= 6) //coğin
                {
                    var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                    var coin = coinPool.GetObjectFromPool();
                    coin.transform.position = barrelPos;
                }
            }
        }
    }

    public void ReturnToPooler()
    {
        PoolerBase.ReturnObjectToPool(gameObject);
    }
}
