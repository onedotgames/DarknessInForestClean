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
        transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyBase>();

        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, 0.5f));
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
