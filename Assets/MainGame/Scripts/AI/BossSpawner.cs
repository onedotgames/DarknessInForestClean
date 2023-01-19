using System;
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
    public bool isBossWarningShowed1 = false;
    public bool isBossWarningShowed2 = false;
    public bool isBossWarningShowed3 = false;

    public Transform ActiveBossTransform;
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
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnLevelFailed += LevelFailed;
        }
        timeManager = GameManager.TimeManager;
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
    }
    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            if (!GameManager.IsBossTime)
            {
                TimePassed += Time.deltaTime;

                if ((timeManager.GetTimeValue() > BossTimeOne - 5) && !isBossWarningShowed1)
                {
                    StartBossPreperation(1);
                }
                if ((timeManager.GetTimeValue() > BossTimeOne) && !Boss1Spawned)
                {
                    SpawnBoss(1);
                }

                if ((timeManager.GetTimeValue() > BossTimeTwo - 5) && !isBossWarningShowed2)
                {
                    StartBossPreperation(2);
                }
                if ((timeManager.GetTimeValue() > BossTimeTwo) && !Boss2Spawned)
                {
                    SpawnBoss(2);

                }

                if ((timeManager.GetTimeValue() > BossTimeThree - 5) && !isBossWarningShowed3)
                {
                    StartBossPreperation(3);
                }
                if ((timeManager.GetTimeValue() > BossTimeThree) && !Boss3Spawned)
                {
                    SpawnBoss(3);

                }
            }

            
        }
    }

    private void StartBossPreperation(int bossNumber)
    {
        if (bossNumber == 1)
        {
            SetBossRing();
            isBossWarningShowed1 = true;
            StartCoroutine(hud.BossRoutine());
            StopEnemySpawners();
            ClearOtherEnemies();
        }
        if (bossNumber == 2)
        {
            SetBossRing();
            isBossWarningShowed2 = true;
            StartCoroutine(hud.BossRoutine());
            StopEnemySpawners();
            ClearOtherEnemies();
        }
        if (bossNumber == 3)
        {
            SetBossRing();
            isBossWarningShowed3 = true;
            StartCoroutine(hud.BossRoutine());
            StopEnemySpawners();
            ClearOtherEnemies();
        }
        
    }

    private void SpawnBoss(int bossNumber)
    {
        if(bossNumber == 1)
        {
            GameManager.IsBossTime = true;

            Boss1Spawned = true;
            Boss1.gameObject.transform.position = transform.position;
            Boss1.gameObject.SetActive(true);
            ActiveBossTransform = Boss1.transform;
            Boss1.Initialize(GameManager);
        }
        if (bossNumber == 2)
        {
            GameManager.IsBossTime = true;

            Boss2Spawned = true;
            Boss2.gameObject.transform.position = transform.position;
            Boss2.gameObject.SetActive(true);
            ActiveBossTransform = Boss1.transform;
            Boss2.Initialize(GameManager);
        }
        if (bossNumber == 3)
        {
            GameManager.IsBossTime = true;

            Boss3Spawned = true;
            Boss3.gameObject.transform.position = transform.position;
            Boss3.gameObject.SetActive(true);
            ActiveBossTransform = Boss1.transform;
            Boss3.Initialize(GameManager);
        }

    }

    private void ClearOtherEnemies()
    {
        GameManager.BossSpawn();
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
        isBossWarningShowed1 = false;
        isBossWarningShowed2 = false;
        isBossWarningShowed3 = false;
        GameManager.IsBossTime = false;
        BossRing.SetActive(false);
        Boss1Spawned = false;
        Boss2Spawned = false;
        Boss3Spawned = false;
    }

    private void LevelCompleted()
    {
        TimePassed = 0;
        isBossWarningShowed1 = false;
        isBossWarningShowed2 = false;
        isBossWarningShowed3 = false;
        GameManager.IsBossTime = false;
        BossRing.SetActive(false);
        Boss1Spawned = false;
        Boss2Spawned = false;
        Boss3Spawned = false;
    }
    private void LevelFailed()
    {
        TimePassed = 0;
        isBossWarningShowed1 = false;
        isBossWarningShowed2 = false;
        isBossWarningShowed3 = false;
        GameManager.IsBossTime = false;
        BossRing.SetActive(false);
        Boss1Spawned = false;
        Boss2Spawned = false;
        Boss3Spawned = false;
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= GameStart;
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnLevelFailed -= LevelFailed;
        }
    }

}
