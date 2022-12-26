using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : CustomBehaviour
{
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnLevelFailed += LevelFailed;
        }
    }

    private void LevelCompleted()
    {
        var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];     
        pool.ReturnObjectToPool(gameObject);    
    }

    private void LevelFailed()
    {
        var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
        pool.ReturnObjectToPool(gameObject);
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnLevelFailed -= LevelFailed;
        }
    }
}
