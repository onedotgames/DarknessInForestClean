using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileToSpawn : CustomBehaviour
{
    public CustomBehaviour BehaviourToInit;
    private ParticleSystem ClearSystem;
    public Transform objectTransform;
    public delegate void OnDisableWeaponCallBack(ProjectileToSpawn Instance);
    public OnDisableWeaponCallBack Disable;

    private void OnParticleSystemStopped()
    {
        Disable?.Invoke(this);
    }
}
