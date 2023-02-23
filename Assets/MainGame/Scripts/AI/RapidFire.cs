using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BossBase;
using UnityEngine.UIElements;

public class RapidFire : BossBase
{
    protected override void SetMovementPattern()
    {
        base.SetMovementPattern();
        //MovementMethod1 += ChasePlayer;
        MovementMethod1 += RigidBodyChase;
        ShouldRotate = true;
        ShouldMove = true;
    }

    protected override void SetAttackPattern()
    {
        base.SetAttackPattern();
        ShouldAttack = true;
        //StartCoroutine(RapidFire(StartDelay, ChargeCount, BaseAttackCooldown, TimeBtwCharges));
        AttackMethod1 += MeleeAttack;
        //AttackMethod2 += RapidFireAttack;
    }

    public override void Update()
    {
        //base.Update();
        if (!GameManager.IsGamePaused && IsActivated && ShouldRotate)
        {
            BossRotation();
        }
        if (!GameManager.IsGamePaused && IsActivated && ShouldMove)
        {
            RigidBodyChase();
        }
        if (!GameManager.IsGamePaused && IsActivated && ShouldAttack)
        {
            timeValue1 += Time.deltaTime;
            //Debug.Log(attackDelegate + " Remaing Time: " + (cooldown - timeValue));
            if (timeValue1 >= BaseAttackCooldown)
            {
                MeleeAttack(timeValue1);
                timeValue1 = 0;

            }
            timeValue2 += Time.deltaTime;
            //Debug.Log(attackDelegate + " Remaing Time: " + (cooldown - timeValue));
            if (timeValue2 >= BaseSpecialOneCooldown)
            {
                RapidFireAttack(timeValue2);
                timeValue2 = 0;

            }
        }
    }
}
