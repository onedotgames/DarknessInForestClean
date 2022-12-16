using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : CustomBehaviour
{
    public List<EnemyBase> AIList;
    public List<Transform> EnemyList;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SubEvents();
    }

    private void LevelFailed()
    {
        DisableEnemies();
    }

    private void DisableEnemies()
    {
        GameManager.SpawnerManager.MainSpawnerRoutineStop();
        GameManager.SpawnerManager.AdditionalSpawnerRoutineStop();
        AIList.ForEach(x =>
        {
            GameManager.PoolingManager.EnemyPoolerList[(int)x.EnemyPoolerType].ReturnObjectToPool(x.gameObject);
        });
        ClearEnemyList();
    }

    private void ClearEnemyList()
    {
        AIList.Clear();
    }

    private void SubEvents()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
        }
    }

    private void UnSubEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;

        }
    }

    private void OnDisable()
    {
        UnSubEvents();
    }

    private void OnDestroy()
    {
        UnSubEvents();
    }
}
