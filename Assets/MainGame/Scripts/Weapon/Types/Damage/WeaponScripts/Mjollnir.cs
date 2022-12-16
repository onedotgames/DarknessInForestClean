using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mjollnir : WeaponBase
{
    public override void AttackMethod()
    {
        base.AttackMethod();
        transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
    }
}
