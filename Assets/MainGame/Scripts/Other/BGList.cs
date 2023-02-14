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
        }
    }


    private void LevelFailed()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            Destroy(backgrounds[i]);
        }
        backgrounds.Clear();
    }

    private void LevelSucceed()
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
        }
    }
}
