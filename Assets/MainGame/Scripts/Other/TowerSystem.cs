using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSystem : CustomBehaviour
{
    public float TowerHealth;
    public bool isTowerDestroyed = false;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if(TowerHealth <= 0)
        {
            isTowerDestroyed = true;
            gameObject.SetActive(false);
        }
    }

    public void GetHitTower(float hitPower)
    {
        TowerHealth -= hitPower;
        Debug.Log(TowerHealth);
    }
}
