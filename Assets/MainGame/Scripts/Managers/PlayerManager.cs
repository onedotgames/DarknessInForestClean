using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CustomBehaviour
{
    #region Fields
    public Player CurrentPlayer;
    #endregion
    
    #region Initialization
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        if(gameManager != null)
        {
            gameManager.OnStartGame += OnLevelStart;
        }

        InitializeOptions();
        TutorialPassed(true);
    }
    
    private void InitializeOptions()
    {
        CurrentPlayer.Initialize(GameManager);
    }
    #endregion


    #region Update Methods
    public void UpdateCoinCountData(int collectedCoin)
    {
        GameManager.JsonConverter.PlayerData.CoinCount += collectedCoin;
        GameManager.JsonConverter.SavePlayerData();
    }

    public void UpdateNameData(string name)
    {
        GameManager.JsonConverter.PlayerData.Name = name;
        GameManager.JsonConverter.SavePlayerData();
    }

    public void UpdateSelectedLevelNumber(int number)
    {
        GameManager.JsonConverter.PlayerData.SelectedLevelNumber = number;
        GameManager.JsonConverter.SavePlayerData();
    }
    public void UpdateLevelData()
    {
        GameManager.JsonConverter.PlayerData.LevelNumber++;
        GameManager.JsonConverter.SavePlayerData();
    }
    public void UpdateSelectedPlayerId(int id)
    {
        GameManager.JsonConverter.PlayerData.SelectedPlayerId = id;
        GameManager.JsonConverter.SavePlayerData();
    }
    #endregion

    #region Get Methods
    public string GetPlayerName()
    {
        var name = GameManager.JsonConverter.PlayerData.Name;

        if (string.IsNullOrEmpty(name))
        {
            return "Player";
        }

        return name;
    }

    public int GetLevelNumber()
    {
        return GameManager.JsonConverter.PlayerData.LevelNumber;
    }

    public int GetTotalCoinCount()
    {
        
        return GameManager.JsonConverter.PlayerData.CoinCount;
    }

    public int GetSelectedPlayerId()
    {
        return GameManager.JsonConverter.PlayerData.SelectedPlayerId;
    }
    public int GetSelectedLevelNumber()
    {
        return GameManager.JsonConverter.PlayerData.SelectedLevelNumber;
    }
    #endregion

    #region Bool Methods
    public bool IsTutorialPassed()
    {
        return GameManager.JsonConverter.PlayerData.TutorialLevelFinished;
    }

    public bool TutorialPassed(bool boolean)
    {
        return GameManager.JsonConverter.PlayerData.TutorialLevelFinished = boolean;
    }
    #endregion

    private void OnLevelStart()
    {
        
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= OnLevelStart;
        }
    }
}
