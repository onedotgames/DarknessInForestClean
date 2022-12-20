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
    private bool isBossWarningShowed = false;

    public GameObject BossRing;
    public GameObject BossSpawnLocation;

    private TimeManager timeManager;
    private HUD hud;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (GameManager != null)
        {
            GameManager.OnStartGame += GameStart;
        }
        timeManager = GameManager.TimeManager;
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
    }
    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            if (!GameManager.IsBossTime)
            {
                TimePassed += Time.deltaTime;

            }
            if ((timeManager.GetTimeValue() > BossTimeOne - 5) && !isBossWarningShowed)
            {
                SetBossRing();
                isBossWarningShowed = true;
                StartCoroutine(hud.BossRoutine());
                StopEnemySpawners();
                ClearOtherEnemies();
            }
            if ((timeManager.GetTimeValue() > BossTimeOne) && !Boss1Spawned)
            {
                GameManager.IsBossTime = true;

                Boss1Spawned = true;
                Boss1.gameObject.transform.position = transform.position;
                Boss1.gameObject.SetActive(true);
                Boss1.Initialize(GameManager);

            }

        }
    }

    private void ClearOtherEnemies()
    {
        GameManager.AIManager.AIList.ForEach(x =>
        {
            GameManager.PoolingManager.EnemyPoolerList[(int)x.EnemyPoolerType].ReturnObjectToPool(x.gameObject);
        });
        GameManager.AIManager.AIList.Clear();
    }

    private void StopEnemySpawners()
    {
        GameManager.SpawnerManager.MainSpawnerRoutineStop();
        GameManager.SpawnerManager.AdditionalSpawnerRoutineStop();
    }

    private void SetBossRing()
    {
        BossRing.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
        transform.position = BossSpawnLocation.transform.position;
        BossRing.SetActive(true);
    }

    private void GameStart()
    {
        TimePassed = 0;
        isBossWarningShowed = false;
        Boss1Spawned = false;
        Boss2Spawned = false;
        Boss3Spawned = false;
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= GameStart;
        }
    }

}
