using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : WeaponBase
{
    public float SlowPower;
    private float tempSpeed;

    public override void AttackMethod()
    {
        base.AttackMethod();
        transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyBase>();
            //slow vercek bir süre
            tempSpeed = enemy.mStats.BaseSpeed;
            enemy.BaseSpeed -= SlowPower;
            BaseSpeed = 0;
            StartCoroutine(WebCoroutine());
        }
        if (collision.CompareTag("Boss"))
        {
            var boss = collision.GetComponent<BossBase>();
            //slow vercek bir süre
            tempSpeed = boss.BaseMoveSpeed;
            boss.BaseMoveSpeed -= SlowPower;
            BaseSpeed = 0;
            StartCoroutine(WebCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBase>().BaseSpeed = tempSpeed;
        }
        if (collision.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossBase>().BaseMoveSpeed = tempSpeed;
        }
    }
  
    private IEnumerator WebCoroutine()
    {
        yield return new WaitForSeconds(3f);
        BaseSpeed = 3;
        PoolerBase.ReturnObjectToPool(gameObject);
    }
}
