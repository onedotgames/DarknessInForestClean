using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCollider : CustomBehaviour
{
    private HUD hud;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Experience"))
        {
            var exp = collision.GetComponent<Experience>();
            if (exp.IsReturning && exp.isTaken)
            {
                GameManager.PlayerLevelManager.SetExp(25f);
                hud.SetExpBarFillAmount();
                GameManager.PoolingManager.ExpPoolerList[(int)exp.ExpPoolerType]
                    .ReturnObjectToPool(collision.gameObject);
                //Debug.Log("Is Going: " + exp.isGoing + "Is Taken: " + exp.isTaken + "Is Returning: " + exp.IsReturning);
                exp.isGoing = false;
                exp.isTaken = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Experience"))
        {
            //GameManager.PlayerLevelManager.SetExp(25f);
            //hud.SetExpBarFillAmount();
            var exp = collision.GetComponent<Experience>();
            //GameManager.PoolingManager.ExpPoolerList[(int)exp.ExpPoolerType].ReturnObjectToPool(collision.gameObject);
            //exp.isGoing = false;
            if (exp.IsReturning && exp.isTaken)
            {
                GameManager.PlayerLevelManager.SetExp(25f);
                hud.SetExpBarFillAmount();
                GameManager.PoolingManager.ExpPoolerList[(int)exp.ExpPoolerType]
                    .ReturnObjectToPool(collision.gameObject);
                //Debug.Log("Is Going: " + exp.isGoing + "Is Taken: " + exp.isTaken + "Is Returning: " + exp.IsReturning);
                exp.isGoing = false;
                exp.isTaken = false;
            }
            else
            {
                if(exp != null)
                {
                    if (exp.isActiveAndEnabled)
                    {
                        exp.TriggerExperience();
                    }
                }
                
            }
        }
    }
}
