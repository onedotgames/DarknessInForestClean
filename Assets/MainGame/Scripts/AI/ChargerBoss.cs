using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerBoss : BossBase
{
    
    protected override void SetMovementPattern()
    {
        base.SetMovementPattern();
        MovementMethod1 += ChasePlayer;
    }

    protected override void SetAttackPattern()
    {
        base.SetAttackPattern();
        StartCoroutine(ChargeAttackWithoutIndicatorFollow(StartDelay,ChargeCount,ChargeBuildUpTime,ChargeTime,BaseAttackCooldown,TimeBtwCharges));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TAGS.Player))
        {
            collision.GetComponent<Player>().GetHit(BaseDamage);
        }
    }
}
