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
            GameManager.OnRestartGame += RestartGame;
        }
        timeManager = GameManager.TimeManager;
        hud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
    }
    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted && !GameManager.IsMiniGame)
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
        //GameManager.IsBossTime = true;

        if (bossNumber == 1)
        {
            SetBossRing();
            isBossWarningShowed1 = true;
            StartCoroutine(hud.BossRoutine());
            StopEnemySpawners();
            //ClearOtherEnemies();
        }
        if (bossNumber == 2)
        {
            SetBossRing();
            isBossWarningShowed2 = true;
            StartCoroutine(hud.BossRoutine());
            StopEnemySpawners();
            //ClearOtherEnemies();
        }
        if (bossNumber == 3)
        {
            SetBossRing();
            isBossWarningShowed3 = true;
            StartCoroutine(hud.BossRoutine());
            StopEnemySpawners();
            //ClearOtherEnemies();
        }
        
    }

    private void SpawnBoss(int bossNumber)
    {
        GameManager.IsBossTime = true;
        ClearOtherEnemies();
        Debug.Log(GameManager.IsBossTime);
        if (bossNumber == 1)
        {
            Boss1Spawned = true;

            Boss1._DesiredStartPosition = BossSpawnLocation.transform.position;

            Boss1.BossReset();

            Boss1.gameObject.SetActive(true);

            Boss1.BossReset();

            ActiveBossTransform = Boss1.transform;

            Boss1.Initialize(GameManager);
        }
        if (bossNumber == 2)
        {
            Boss2Spawned = true;

            Boss2._DesiredStartPosition = BossSpawnLocation.transform.position;

            Boss2.BossReset();

            Boss2.gameObject.SetActive(true);

            Boss2.BossReset();

            ActiveBossTransform = Boss2.transform;

            Boss2.Initialize(GameManager);
        }
        if (bossNumber == 3)
        {
            Boss3Spawned = true;

            Boss3._DesiredStartPosition = BossSpawnLocation.transform.position;

            Boss3.BossReset();

            Boss3.gameObject.SetActive(true);

            Boss3.BossReset();

            ActiveBossTransform = Boss3.transform;

            Boss3.Initialize(GameManager);
        }
        Debug.Log(GameManager.IsBossTime);

    }

    public void ClearOtherEnemies()
    {
        GameManager.BossSpawn();
        GameManager.AIManager.AIList.Clear();
        //GameManager.AIManager.EnemyList.Clear();
    }

    private void StopEnemySpawners()
    {
        GameManager.SpawnerManager.MainSpawnerRoutineStop();
        GameManager.SpawnerManager.AdditionalSpawnerRoutineStop();
    }

    private void SetBossRing()
    {
        switch (GameManager.BackgroundManager.mapType)
        {
            case MapType.Normal:
                BossRing.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
                BossRing.transform.eulerAngles = Vector3.zero;

                break;
            case MapType.Vertical:
                BossRing.transform.position = new Vector3(0, GameManager.PlayerManager.CurrentPlayer.transform.position.y,0);
                BossRing.transform.eulerAngles = Vector3.zero;
                break;
            case MapType.Horizontal:
                BossRing.transform.position = new Vector3(GameManager.PlayerManager.CurrentPlayer.transform.position.x, 0, 0);
                BossRing.transform.eulerAngles = new Vector3(0,0,90);

                break;
        }
        //BossRing.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
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

    private void RestartGame()
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
            GameManager.OnRestartGame -= RestartGame;
        }
    }

}
