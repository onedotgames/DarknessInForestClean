using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGList : CustomBehaviour
{
    public List<GameObject> backgrounds;


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
            GameManager.OnLevelCompleted += LevelSucceed;
            GameManager.OnRestartGame += RestartGame;
            GameManager.OnReturnToMainMenu += ReturnMenu;
        }
    }


    private void LevelFailed()
    {
        //for (int i = 0; i < backgrounds.Count; i++)
        //{
        //    Destroy(backgrounds[i]);
        //}
        //backgrounds.Clear();
    }

    private void ReturnMenu()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            Destroy(backgrounds[i]);
        }
        backgrounds.Clear();
    }

    private void LevelSucceed()
    {
        //for (int i = 0; i < backgrounds.Count; i++)
        //{
        //    Destroy(backgrounds[i]);
        //}
        //backgrounds.Clear();
    }

    private void RestartGame()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            Destroy(backgrounds[i]);
        }
        backgrounds.Clear();
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;
            GameManager.OnLevelCompleted -= LevelSucceed;
            GameManager.OnRestartGame -= RestartGame;
            GameManager.OnReturnToMainMenu -= ReturnMenu;
        }
    }
}
