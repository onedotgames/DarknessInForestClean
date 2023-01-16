using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemySpawner : CustomBehaviour
{
    private TimeManager timeManager;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        timeManager = gameManager.TimeManager;
    }

    public void SpawnEnemyV2()
    {
        if (timeManager.GetTimeValue() < 30f)
        {
            var pooler = GameManager.PoolingManager.EnemyPoolerListV2[(int)EnemyPoolerType.BasicMeleeEnemyPoolerOne];
            var obj = pooler.GetFromPool();
            obj.objectTransform.position = transform.position;
            obj.gameObject.SetActive(true);
            var enemy = obj.GetComponent<EnemyBase>();
            enemy.ObjectToPool = obj;
            enemy.Pooler = pooler;
            enemy.ActivateEnemy();
        }
        else
        {
            Debug.Log("30+");
            var index = Random.Range(0, GameManager.PoolingManager.EnemyPoolerListV2.Count);
            var pooler = GameManager.PoolingManager.EnemyPoolerListV2[index];
            var obj = pooler.GetFromPool();
            obj.objectTransform.position = transform.position;
            obj.gameObject.SetActive(true);
            var enemy = obj.GetComponent<EnemyBase>();
            enemy.ObjectToPool = obj;
            enemy.Pooler = pooler;
            enemy.ActivateEnemy();
        }
    }
}
