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
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnRestartGame += RestartGame;
        }
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
    }

    private void LevelFailed()
    {
        isTowerDestroyed = false;
    }

    private void LevelCompleted()
    {
        isTowerDestroyed = false;

    }

    private void RestartGame()
    {
        isTowerDestroyed = false;
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
}
