using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : BossBase
{
    protected override void SetMovementPattern()
    {
        base.SetMovementPattern();
        //MovementMethod1 += ChasePlayer;
        MovementMethod1 += RigidBodyChase;
        ShouldRotate = true;
    }

    protected override void SetAttackPattern()
    {
        base.SetAttackPattern();
        //StartCoroutine(RapidFire(StartDelay, ChargeCount, BaseAttackCooldown, TimeBtwCharges));
        AttackMethod1 += JumpAttack;
        AttackMethod2 += MeleeAttack;
    }


}
