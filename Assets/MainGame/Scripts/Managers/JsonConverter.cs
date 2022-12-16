using System.Collections.Generic;
using UnityEngine;

public class JsonConverter : CustomBehaviour
{
    public PlayerData PlayerData;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        var playerData = PlayerPrefs.GetString(ConstantDatas.PLAYER_DATA);

        if (string.IsNullOrEmpty(playerData))
        {
            var playerMarketData = new List<PurchaseData>();

            var defaultPlayer = new PurchaseData
            {
                Id = 0,
                PurchaseState = PlayerStates.Selected
            };
            playerMarketData.Add(defaultPlayer);

            for(int i = 1; i < ConstantDatas.TOTAL_PLAYER_COUNT; i++)
            {
                var player = new PurchaseData
                {
                    Id = i,
                    PurchaseState = PlayerStates.Purchase
                };

                playerMarketData.Add(player);
            }

            PlayerData = new PlayerData
            {
                Name = "",
                LevelNumber = 1,
                CoinCount = 0,
                SelectedPlayerId = 0,
                SelectedLevelNumber = 0,
                PurchaseData = playerMarketData,
                TutorialLevelFinished = false
            };

            SavePlayerData();
        }
        else
        {
            PlayerData = JsonUtility.FromJson<PlayerData>(playerData);
        }
    }


    public void SavePlayerData()
    {
        var jsonData = JsonUtility.ToJson(PlayerData);
        PlayerPrefs.SetString(ConstantDatas.PLAYER_DATA, jsonData);
        PlayerPrefs.Save();
    }
}
