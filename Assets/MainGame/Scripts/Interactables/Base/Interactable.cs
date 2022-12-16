using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : CustomBehaviour
{
    protected Collider priOther;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        priOther = other;
        if (other.CompareTag(TAGS.Player))
        {
            OnTriggerEnterActionForPlayer();
        }

        if (other.CompareTag(TAGS.AI))
        {
            OnTriggerEnterActionForAI();
        }

        if (other.CompareTag(TAGS.Environment))
        {
            OnTriggerEnterAActionForEnvironment();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TAGS.Player))
        {
            OnTriggerExitActionForPlayer();
        }

        if (other.CompareTag(TAGS.AI))
        {
            OnTriggerExitActionForAI();
        }

        if (other.CompareTag(TAGS.Environment))
        {
            OnTriggerExitActionForEnvironment();
        }
    }

    public virtual void OnTriggerEnterActionForPlayer()
    {

    }

    public virtual void OnTriggerEnterActionForAI()
    {

    }

    public virtual void OnTriggerEnterAActionForEnvironment()
    {

    }

    public virtual void OnTriggerExitActionForPlayer()
    {

    }

    public virtual void OnTriggerExitActionForAI()
    {

    }

    public virtual void OnTriggerExitActionForEnvironment()
    {

    }
}
