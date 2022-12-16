using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkunkGas : WeaponBase
{
    public override void AttackMethod()
    {
        base.AttackMethod();
    }
    public override void MovementMethod()
    {
        transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
    }
}
