using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonFollowingCharger : BossBase
{
    protected override void SetMovementPattern()
    {
        base.SetMovementPattern();
        //MovementMethod1 += ChasePlayer;
        MovementMethod1 += RigidBodyChase;
    }

    protected override void SetAttackPattern()
    {
        base.SetAttackPattern();
        StartCoroutine(ChargeAttackWithoutIndicatorFollow(StartDelay, ChargeCount, ChargeBuildUpTime, ChargeTime, BaseAttackCooldown, TimeBtwCharges));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TAGS.Player))
        {
            collision.gameObject.GetComponent<Player>().GetHit(BaseDamage);
        }
    }
}
