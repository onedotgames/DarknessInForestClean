using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : CustomBehaviour
{
    public float TimePassed = 0f;

    public BossBase Boss1;
    public bool Boss1Spawned = false;

    public BossBase Boss2;
    public bool Boss2Spawned = false;

    public BossBase Boss3;
    public bool Boss3Spawned = false;

    public int BossTimeOne;
    public int BossTimeTwo;
    public int BossTimeThree;

    private TimeManager timeManager;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        timeManager = GameManager.TimeManager;
    }
    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            if (!GameManager.IsBossTime)
            {
                TimePassed += Time.deltaTime;

            }
        }

        if (timeManager.GetTimeValue() > BossTimeOne && !Boss1Spawned)
        {
            GameManager.IsBossTime = true;
            GameManager.SpawnerManager.MainSpawnerRoutineStop();
            GameManager.SpawnerManager.AdditionalSpawnerRoutineStop();
            GameManager.AIManager.AIList.ForEach(x =>
            {
                GameManager.PoolingManager.EnemyPoolerList[(int)x.EnemyPoolerType].ReturnObjectToPool(x.gameObject);
            });
            GameManager.AIManager.AIList.Clear();
            Boss1Spawned = true;
            Boss1.gameObject.transform.position = transform.position;
            Boss1.gameObject.SetActive(true);
            Boss1.Initialize(GameManager);

        }
    }
    
}
