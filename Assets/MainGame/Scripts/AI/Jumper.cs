using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //ShouldAttack = true;

        //StartCoroutine(RapidFire(StartDelay, ChargeCount, BaseAttackCooldown, TimeBtwCharges));
        //AttackMethod1 += JumpAttack;
        //AttackMethod2 += MeleeAttack;
    }

    public override void Update()
    {
        //base.Update();
        if (!GameManager.IsGamePaused && IsActivated && ShouldRotate)
        {
            BossRotation();
        }
        if (IsActivated)
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

            if (ShouldSpecialOne)
            {
                timeValue2 += Time.deltaTime;

                if (timeValue2 >= BaseSpecialOneCooldown)
                {
                    JumpAttack(timeValue2);
                    timeValue2 = 0;

                }
            }
            
        }
    }
}
