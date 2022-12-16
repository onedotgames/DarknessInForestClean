using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIBase : CustomBehaviour
{
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }


    private void SetTargetDestination(Vector3 target)
    {
        NavMeshAgent.SetDestination(target);
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
