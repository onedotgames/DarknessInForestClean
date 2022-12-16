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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Experience"))
        {
            GameManager.PlayerLevelManager.SetExp(25f);
            hud.SetExpBarFillAmount();
            GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType.SmallExperience].ReturnObjectToPool(collision.gameObject);
            collision.GetComponent<Experience>().isGoing = false;
        }
    }
}
