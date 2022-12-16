using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clover : WeaponBase
{
    public ParticleSystem VFX;
    public float RotSpeed;
    public GameObject Model;
    public override void MovementMethod()
    {
        base.MovementMethod();
        Model.transform.Rotate(RotSpeed * Time.deltaTime * Vector3.forward);
    }

    public override void AttackMethod()
    {
        base.AttackMethod();
        if (!VFX.gameObject.activeInHierarchy)
        {
            VFX.gameObject.SetActive(true);
        }
        if (!VFX.isPlaying)
        {
            VFX.Play();
        }
        transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
    }
}
