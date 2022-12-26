using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyFountain : CustomBehaviour
{
    public GameObject Fountain;
    public bool isFountainTaken = false;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnLevelFailed += LevelFailed;
        }
    }

    private void Awake()
    {
        Fountain.SetActive(false);
    }

    private void StartGame()
    {
        Fountain.transform.position = new Vector3(Random.Range(-80, 80), Random.Range(-80, 80), 0);
        Fountain.SetActive(true);
    }

    private void LevelCompleted()
    {
        Fountain.SetActive(false);
        isFountainTaken = false;
    }

    private void LevelFailed()
    {
        Fountain.SetActive(false);
        isFountainTaken = false;
    }


    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnLevelFailed -= LevelFailed;
        }
    }

}
