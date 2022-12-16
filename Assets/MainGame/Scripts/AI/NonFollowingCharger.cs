using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonFollowingCharger : BossBase
{
    protected override void SetMovementPattern()
    {
        base.SetMovementPattern();
        MovementMethod1 += ChasePlayer;
    }

    protected override void SetAttackPattern()
    {
        base.SetAttackPattern();
        StartCoroutine(ChargeAttackWithoutIndicatorFollow(StartDelay, ChargeCount, ChargeBuildUpTime, ChargeTime, BaseAttackCooldown, TimeBtwCharges));
    }
}
