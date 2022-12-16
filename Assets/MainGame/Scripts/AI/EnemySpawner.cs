using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : CustomBehaviour
{
    public PoolerBase EnemyPoolerToSpawn;
    private TimeManager timeManager;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        timeManager = GameManager.TimeManager;
    }

    public void SpawnEnemy()
    {
        if (!GameManager.IsGamePaused)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if(timeManager.GetTimeValue() < 30f)
        {
            EnemyPoolerToSpawn = GameManager.PoolingManager.EnemyPoolerList[(int)EnemyPoolerType.BasicEnemyPooler];
            var obj = EnemyPoolerToSpawn.GetObjectFromPool();
            obj.transform.position = transform.position;
            obj.GetComponent<EnemyBase>().ActivateEnemy();
        }
        else
        {
            var index = Random.Range(0, GameManager.PoolingManager.EnemyPoolerList.Count);
            EnemyPoolerToSpawn = GameManager.PoolingManager.EnemyPoolerList[index];
            var obj = EnemyPoolerToSpawn.GetObjectFromPool();
            obj.transform.position = transform.position;
            obj.GetComponent<EnemyBase>().ActivateEnemy();
        }

        if(timeManager.GetTimeValue() > 60f)
        {

        }
    }
}
