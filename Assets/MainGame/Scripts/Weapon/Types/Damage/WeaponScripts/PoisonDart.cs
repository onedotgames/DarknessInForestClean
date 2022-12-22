using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDart : WeaponBase
{
    public ParticleSystem PoisonVFX;
    public float PoisonDuration = 2f;
    private float PoisonAreaDamage = 20f;
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
            enemy.GetHit(BaseDamage);
            PoisonVFX.gameObject.SetActive(true);
            PoisonVFX.Play();
            HitEffect.gameObject.SetActive(true);
            HitEffect.Play();
            StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, 0.5f));
            Invoke("StopPoison", 0.2f);
            Invoke("ReturnPoison", PoisonDuration);
            //zehir b?rakma buraya gelecek.

        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            enemy.GetHit(BaseDamage);
            PoisonVFX.gameObject.SetActive(true);
            PoisonVFX.Play();
            HitEffect.gameObject.SetActive(true);
            HitEffect.Play();
            StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, 0.5f));
            Invoke("StopPoison", 0.2f);
            Invoke("ReturnPoison", PoisonDuration);
            //zehir b?rakma buraya gelecek.

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
