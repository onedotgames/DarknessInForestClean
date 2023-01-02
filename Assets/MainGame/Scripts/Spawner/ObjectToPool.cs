using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectToPool : CustomBehaviour
{
    public CustomBehaviour BehaviourToInit;
    private ParticleSystem ClearSystem;
    public Transform objectTransform;
    public delegate void OnDisableCallBack(ObjectToPool Instance);
    public OnDisableCallBack Disable;

    private void OnParticleSystemStopped()
    {
        Disable?.Invoke(this);
    }
}
