using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPoison : WeaponBase
{
    private float PoisonAreaDamage = 20f;
    public float PoisonDuration = 2f;
    public override void AttackMethod()
    {
        base.AttackMethod();
    }

    public override void MovementMethod()
    {
        transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();

            StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, 0.5f));
            Invoke("StopPoison", 0.5f);
            Invoke("ReturnPoison", PoisonDuration);
        }
        if (collision.CompareTag("Boss"))
        {
            var boss = collision.GetComponent<BossBase>();

            StartCoroutine(boss.GetAOEHit(PoisonAreaDamage, 0.5f));
            Invoke("StopPoison", 0.5f);
            Invoke("ReturnPoison", PoisonDuration);
        }
    }

    private void ReturnPoison()
    {
        IsActivated = false;
        PoolerBase.ReturnObjectToPool(gameObject);
    }

    private void StopPoison()
    {
        BaseSpeed = 0f;
    }
}
