using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnerManager : CustomBehaviour
{
    public List<EnemySpawner> MainSpawners;
    public List<EnemySpawner> AdditionalSpawners;

    public GameObject MainSpawnerHolder;
    public IEnumerator MainSpawnerRoutine;
    public GameObject AdditionalSpawnerHolder;
    public IEnumerator AdditionalSpawnerRoutine;
    public BossSpawner BossSpawner;

    private Player player;
    private HUD hud;
    private TimeManager timeManager;

    public bool IsFirstEnemyRushActive = false;
    public bool WarningStarted = false;

    [Header("Spawner Settings")]
    private float activeSpawnTime;
    public float NormalSpawnTime = 4f;
    public float RushOneSpawnTime = 3.5f;
    public float RushTwoSpawnTime = 3f;
    public float RushThreeSpawnTime = 2.5f;

    public float RotationSpeed;

    public int RushTimeOneStart;
    public int RushTimeTwoStart;
    public int RushTimeThreeStart;
    public int RushTimeOneEnd;
    public int RushTimeTwoEnd;
    public int RushTimeThreeEnd;
    

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        if (gameManager != null)
        {
            gameManager.OnStartGame += OnLevelStart;
            gameManager.OnLevelFailed += OnLevelFailed;
        }
        InitializeOptions();
    }
    private void InitializeOptions()
    {
        player = GameManager.PlayerManager.CurrentPlayer;
        hud = GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        timeManager = GameManager.TimeManager;

        BossSpawner.Initialize(GameManager);
        MainSpawners.ForEach(x => x.Initialize(GameManager));
        AdditionalSpawners.ForEach(x => x.Initialize(GameManager));
    }

    private void Update()
    {
        if (GameManager.IsGameStarted)
        {
            if (!GameManager.IsGamePaused)
            {
                if (!GameManager.IsBossTime)
                {
                    if (MainSpawnerHolder.activeInHierarchy)
                    {
                        MainSpawnerHolder.transform.position = player.transform.position;
                        MainSpawnerHolder.transform.Rotate(Time.deltaTime * RotationSpeed * Vector3.forward);
                    }

                    if (MainSpawnerHolder.activeInHierarchy)
                    {
                        AdditionalSpawnerHolder.transform.position = player.transform.position;
                        AdditionalSpawnerHolder.transform.Rotate(Time.deltaTime * RotationSpeed * Vector3.forward);
                    }

                    if (timeManager.GetTimeValue() > RushTimeOneStart - 5)
                    {
                        if (!WarningStarted)
                        {
                            WarningStarted = true;
                            StartCoroutine(hud.RushRoutine());
                        }
                    }
                    if (timeManager.GetTimeValue() > RushTimeOneStart && timeManager.GetTimeValue() < RushTimeOneEnd)
                    {
                        if (!IsFirstEnemyRushActive)
                        {
                            IsFirstEnemyRushActive = true;
                            GameManager.CameraManager.MainCamera.DOOrthoSize(10f, 3f);
                            AdditionalSpawnerHolder.SetActive(true);
                            CacheAdditionalSpawnRoutine();
                            activeSpawnTime = RushOneSpawnTime;
                        }
                    }
                    if (timeManager.GetTimeValue() > RushTimeOneEnd)
                    {
                        AdditionalSpawnerRoutineStop();
                        GameManager.CameraManager.MainCamera.DOOrthoSize(7.5f, 3f);
                        AdditionalSpawnerHolder.SetActive(false);
                        activeSpawnTime = NormalSpawnTime;
                    }
                }
            }
        }
    }

    public void CacheMainSpawnRoutine()
    {

        MainSpawnerRoutine = MainSpawnRoutineStart();
        StartCoroutine(MainSpawnerRoutine);

    }
    public void CacheAdditionalSpawnRoutine()
    {

        AdditionalSpawnerRoutine = AdditionalSpawnRoutineStart();
        StartCoroutine(AdditionalSpawnerRoutine);

    }
    public IEnumerator MainSpawnRoutineStart()
    {
        if (GameManager != null)
        {
            if (GameManager.SpawnerManager.isActiveAndEnabled)
            {
                MainSpawners.ForEach(x => x.SpawnEnemy());
                yield return new WaitForSeconds(activeSpawnTime);
                CacheMainSpawnRoutine();
            }
        }

    }
    public IEnumerator AdditionalSpawnRoutineStart()
    {
        if (GameManager != null)
        {
            if (GameManager.SpawnerManager.isActiveAndEnabled)
            {
                AdditionalSpawners.ForEach(x => x.SpawnEnemy());
                yield return new WaitForSeconds(activeSpawnTime);
                CacheAdditionalSpawnRoutine();
            }
        }

    }

    public void MainSpawnerRoutineStop()
    {
        if (MainSpawnerRoutine != null)
        {
            StopCoroutine(MainSpawnerRoutine);
        }
    }
    public void AdditionalSpawnerRoutineStop()
    {
        if (AdditionalSpawnerRoutine != null)
        {
            StopCoroutine(AdditionalSpawnerRoutine);
        }
    }

    #region Events

    private void OnLevelStart()
    {
        activeSpawnTime = NormalSpawnTime;
        CacheMainSpawnRoutine();
    }

    private void OnLevelFailed()
    {
        MainSpawnerRoutineStop();
        AdditionalSpawnerRoutineStop();
        AdditionalSpawnerHolder.SetActive(false);
    }

    

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (GameManager != null)
        {
            GameManager.OnStartGame -= OnLevelStart;
            GameManager.OnLevelFailed -= OnLevelFailed;
        }
    }
    #endregion
}
