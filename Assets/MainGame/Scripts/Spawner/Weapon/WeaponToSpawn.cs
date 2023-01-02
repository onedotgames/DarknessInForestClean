using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToSpawn : CustomBehaviour
{
    public WeaponBase BehaviourToInit; 
    private ParticleSystem ClearSystem;
    public Transform objectTransform;
    public delegate void OnDisableWeaponCallBack(WeaponToSpawn Instance);
    public OnDisableWeaponCallBack Disable;

    private void OnParticleSystemStopped()
    {
        Disable?.Invoke(this);
    }
}
