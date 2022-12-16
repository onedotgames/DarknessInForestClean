using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : CustomBehaviour
{
    public float TimeValue;
    public float TargetGameTimeInSeconds;


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame += GameStarted;
            GameManager.OnReturnToMainMenu += OnREturnToMainMenu;
        }
    }
    private void UnSubscribeToEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= GameStarted;
            GameManager.OnReturnToMainMenu -= OnREturnToMainMenu;
        }
    }

    private void Update()
    {
        if (GameManager.IsGameStarted)
        {
            if (!GameManager.IsGamePaused)
            {
                if (!GameManager.IsBossTime)
                {
                    TimeValue += Time.deltaTime;
                }
            }
        }
    }

    public float GetTimeValue()
    {
        return TimeValue;
    }
    public float GetTargetTimeValueInSeconds()
    {
        return TargetGameTimeInSeconds;
    }

    private void GameStarted()
    {
        TimeValue = 0;
    }
    private void OnREturnToMainMenu()
    {
        TimeValue = 0;
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}
