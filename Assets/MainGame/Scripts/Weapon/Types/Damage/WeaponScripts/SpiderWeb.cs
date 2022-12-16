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
            //slow vercek bir süre
            tempSpeed = collision.gameObject.GetComponent<EnemyBase>().mStats.BaseSpeed;
            collision.gameObject.GetComponent<EnemyBase>().BaseSpeed -= SlowPower;
            BaseSpeed = 0;
            StartCoroutine(WebCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log(tempSpeed);
            collision.gameObject.GetComponent<EnemyBase>().BaseSpeed = tempSpeed;
        }
    }
  
    private IEnumerator WebCoroutine()
    {
        yield return new WaitForSeconds(3f);
        BaseSpeed = 3;
        PoolerBase.ReturnObjectToPool(gameObject);
    }
}
