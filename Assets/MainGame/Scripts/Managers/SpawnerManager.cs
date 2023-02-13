using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnerManager : CustomBehaviour
{
    public SwipeLevels SwipeLevels;
    //public List<EnemySpawner> MainSpawners;
    public List<MainEnemySpawner> MainSpawnersV2;
    public List<MainEnemySpawner> VerticalMainSpawnersV2;
    public List<MainEnemySpawner> HorizontalMainSpawnersV2;
    //public List<EnemySpawner> AdditionalSpawners;
    public List<MainEnemySpawner> AdditionalSpawnersV2;
    public List<MainEnemySpawner> VerticalAdditionalSpawnersV2;
    public List<MainEnemySpawner> HorizontalAdditionalSpawnersV2;

    public GameObject MainSpawnerHolder;
    public GameObject HorizontalMainSpawnerHolder;
    public GameObject VerticalMainSpawnerHolder;
    public IEnumerator MainSpawnerRoutine;
    public GameObject AdditionalSpawnerHolder;
    public GameObject HorizontalAdditionalSpawnerHolder;
    public GameObject VerticalAdditionalSpawnerHolder;
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
    public float timeValue;
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
            gameManager.OnLevelCompleted += OnLevelCompleted;
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
        ////MainSpawners.ForEach(x => x.Initialize(GameManager));
        //MainSpawnersV2.ForEach(x => x.Initialize(GameManager));
        ////AdditionalSpawners.ForEach(x => x.Initialize(GameManager));
        //AdditionalSpawnersV2.ForEach(x => x.Initialize(GameManager));
        InitializeSpawners();
    }

    private void InitializeSpawners()
    {
        HorizontalMainSpawnersV2.ForEach(x => x.Initialize(GameManager));
        HorizontalAdditionalSpawnersV2.ForEach(x => x.Initialize(GameManager));
        VerticalMainSpawnersV2.ForEach(x => x.Initialize(GameManager));
        VerticalAdditionalSpawnersV2.ForEach(x => x.Initialize(GameManager));
        MainSpawnersV2.ForEach(x => x.Initialize(GameManager));
        AdditionalSpawnersV2.ForEach(x => x.Initialize(GameManager));
    }

    private void OpenCloseMainSpawners(bool value)
    {
        if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[3])
        {
            //Horizontal
            HorizontalMainSpawnerHolder.SetActive(value);
        }
        else if(SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[4]
            || SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[5])
        {
            //Vertical
            VerticalMainSpawnerHolder.SetActive(value);
        }
        else
        {
            //Normal
            MainSpawnerHolder.SetActive(value);
        }
    }
    private void Update()
    {
        if (GameManager.IsGameStarted && !GameManager.IsGamePaused && !GameManager.IsBossTime)
        {
            timeValue += Time.deltaTime;
            if(timeValue >= activeSpawnTime)
            {
                //Debug.Log("SHOULD SPAWN");
                timeValue = 0;

                //SpawnAllMainV2();
                SpawnFromMains();

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
                        //AdditionalSpawnerHolder.SetActive(true);
                        OpenCloseAdditionalSpawners(true);
                        activeSpawnTime = RushOneSpawnTime;
                    }
                    //SpawnAllAdditionalsV2();
                    SpawnFromAdditionals();
                }
                if (timeManager.GetTimeValue() > RushTimeOneEnd)
                {
                    GameManager.CameraManager.MainCamera.DOOrthoSize(7.5f, 3f);
                    //AdditionalSpawnerHolder.SetActive(false);
                    OpenCloseAdditionalSpawners(false);
                    activeSpawnTime = NormalSpawnTime;
                }
            }
        }
    }
    private void OpenCloseAdditionalSpawners(bool value)
    {
        if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[3])
        {
            //Horizontal
            HorizontalAdditionalSpawnerHolder.SetActive(value);
        }
        else if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[4]
            || SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[5])
        {
            //Vertical
            VerticalAdditionalSpawnerHolder.SetActive(value);
        }
        else
        {
            //Normal
            AdditionalSpawnerHolder.SetActive(value);
        }
    }
    private void SpawnFromMains()
    {
        if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[3])
        {
            //Horizontal
            HorizontalMainSpawnersV2.ForEach(x => x.SpawnEnemyV2());
        }
        else if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[4]
            || SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[5])
        {
            //Vertical
            VerticalMainSpawnersV2.ForEach(x => x.SpawnEnemyV2());

        }
        else
        {
            //Normal
            MainSpawnersV2.ForEach(x => x.SpawnEnemyV2());

        }
    }

    private void SpawnFromAdditionals()
    {
        if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[3])
        {
            //Horizontal
            HorizontalAdditionalSpawnersV2.ForEach(x => x.SpawnEnemyV2());
        }
        else if (SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[4]
            || SwipeLevels.currentLevelMat == SwipeLevels.levelsMaterials[5])
        {
            //Vertical
            VerticalAdditionalSpawnersV2.ForEach(x => x.SpawnEnemyV2());

        }
        else
        {
            //Normal
            AdditionalSpawnersV2.ForEach(x => x.SpawnEnemyV2());

        }
    }
    private void SpawnAllMainV2()
    {
        MainSpawnersV2.ForEach(x => x.SpawnEnemyV2());
    }
    private void SpawnAllAdditionalsV2()
    {
        AdditionalSpawnersV2.ForEach(x => x.SpawnEnemyV2());
    }


    public void MainSpawnerRoutineStop()
    {
        if (MainSpawnerRoutine != null)
        {
            StopCoroutine(MainSpawnerRoutine);
            StopAllCoroutines();
        }
    }
    public void AdditionalSpawnerRoutineStop()
    {
        if (AdditionalSpawnerRoutine != null)
        {
            StopCoroutine(AdditionalSpawnerRoutine);
            StopAllCoroutines();

        }
    }

    #region Events

    private void OnLevelStart()
    {
        activeSpawnTime = NormalSpawnTime;
        //CacheMainSpawnRoutine();
        timeValue = activeSpawnTime;
        OpenCloseMainSpawners(true);

    }

    private void OnLevelFailed()
    {
        MainSpawnerRoutineStop();
        AdditionalSpawnerRoutineStop();
        AdditionalSpawnerHolder.SetActive(false);
        OpenCloseMainSpawners(false);
        OpenCloseAdditionalSpawners(false);
    }
    private void OnLevelCompleted()
    {
        MainSpawnerRoutineStop();
        AdditionalSpawnerRoutineStop();
        AdditionalSpawnerHolder.SetActive(false);
        OpenCloseAdditionalSpawners(false);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
        if (GameManager != null)
        {
            GameManager.OnStartGame -= OnLevelStart;
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
        }
    }
    #endregion
}
