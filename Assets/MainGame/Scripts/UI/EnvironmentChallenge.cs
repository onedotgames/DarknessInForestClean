using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentChallenge : CustomBehaviour
{
    public LevelType levelType;
    public CustomButton PlayButton;
    public Image BGObject;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        PlayButton.Initialize(GameManager.UIManager, OnPlayBTNClick);
        Debug.Log("Enviroment girildi.");
    }

    public void OnPlayBTNClick()
    {
        if(GameManager.PlayerManager.CurrentPlayer.GetComponent<Player>().PlayerVariables.UserCurrentEnergy >= 5)
        {
            GameManager.ChallengeManager.isChallengeLevel = true;
            GameManager.BackgroundManager.mapType = levelType.MapType;
            GameManager.ChallengeManager.CurrentChallengeType = (int)levelType.ChallengeType;
            for (int i = 0; i < 9; i++)
            {
                Debug.Log("Enviroment Challenge ile bg değişti.");
                BGObject.material = levelType.Material;
            }
            GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
            GameManager.UIManager.GetPanel(Panels.Initial).ClosePanel();
            GameManager.StartGame();
            GameManager.PlayerManager.CurrentPlayer.GetComponent<Player>().PlayerVariables.UserCurrentEnergy -= 0;
        }
        else
        {
            Debug.Log("Enerjim yok.");
        }
    }
}

public enum ChallengeType
{
    Ice = 0,
    Thorn = 1,
    Swamp = 2
}

[System.Serializable]
public struct LevelType
{
    public MapType MapType;
    public Material Material;
    public float DifficultMultiplier;
    public ChallengeType ChallengeType;
}