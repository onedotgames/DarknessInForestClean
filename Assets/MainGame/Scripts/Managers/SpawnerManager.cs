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
    private bool isRoutineActive = true;

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
            gameManager.OnRestartGame += RestartGame;
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
        if (GameManager.BackgroundManager.mapType == MapType.Horizontal)
        {
            //Horizontal
            HorizontalMainSpawnerHolder.SetActive(value);
            VerticalMainSpawnerHolder.SetActive(false);
            MainSpawnerHolder.SetActive(false);
        }
        else if(GameManager.BackgroundManager.mapType == MapType.Vertical)
        {
            //Vertical
            VerticalMainSpawnerHolder.SetActive(value);
            MainSpawnerHolder.SetActive(false);
            HorizontalMainSpawnerHolder.SetActive(false);
        }
        else
        {
            //Normal
            MainSpawnerHolder.SetActive(value);
            VerticalMainSpawnerHolder.SetActive(false);
            HorizontalMainSpawnerHolder.SetActive(false);
        }
    }
    private void Update()
    {
        if (GameManager.IsGameStarted && !GameManager.IsGamePaused && !GameManager.IsBossTime && !GameManager.IsMiniGame)
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
                        //GameManager.CameraManager.MainCamera.DOOrthoSize(10f, 3f);
                        //GameManager.CameraManager.OrtoSize = Mathf.Lerp(GameManager.CameraManager.OrtoSize, 11f, 10f * Time.deltaTime);
                        if(isRoutineActive)
                            StartCoroutine(CamSizeRoutine(11f, 7.5f, 3f));
                        //AdditionalSpawnerHolder.SetActive(true);
                        OpenCloseAdditionalSpawners(true);
                        activeSpawnTime = RushOneSpawnTime;
                    }
                    //SpawnAllAdditionalsV2();
                    SpawnFromAdditionals();
                }
                if (timeManager.GetTimeValue() > RushTimeOneEnd)
                {
                    //GameManager.CameraManager.MainCamera.DOOrthoSize(7.5f, 3f);
                    //GameManager.CameraManager.OrtoSize = Mathf.Lerp(GameManager.CameraManager.OrtoSize, 7.5f, 10f * Time.deltaTime);
                    if(!isRoutineActive)
                        StartCoroutine(CamSizeRoutine(7.5f, 11f, 3f));
                    //AdditionalSpawnerHolder.SetActive(false);
                    OpenCloseAdditionalSpawners(false);
                    activeSpawnTime = NormalSpawnTime;
                }
            }
        }
    }
    private void OpenCloseAdditionalSpawners(bool value)
    {
        if (GameManager.BackgroundManager.mapType == MapType.Horizontal)
        {
            //Horizontal
            HorizontalAdditionalSpawnerHolder.SetActive(value);
        }
        else if (GameManager.BackgroundManager.mapType == MapType.Vertical)
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
        if (GameManager.BackgroundManager.mapType == MapType.Horizontal)
        {
            //Horizontal
            HorizontalMainSpawnersV2.ForEach(x => x.SpawnEnemyV2());
        }
        else if (GameManager.BackgroundManager.mapType == MapType.Vertical)
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
        if (GameManager.BackgroundManager.mapType == MapType.Horizontal)
        {
            //Horizontal
            HorizontalAdditionalSpawnersV2.ForEach(x => x.SpawnEnemyV2());
        }
        else if (GameManager.BackgroundManager.mapType == MapType.Vertical)
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

    private IEnumerator CamSizeRoutine(float endValue, float startValue, float time)
    {
        if(endValue > startValue)
            isRoutineActive = false;
        else
            isRoutineActive = true;
        Debug.Log("Cam değişiyor.");
        float elapsed = 0;
        while(elapsed <= time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            GameManager.CameraManager.OrtoSize = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        
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

    private void RestartGame()
    {
        MainSpawnerRoutineStop();
        AdditionalSpawnerRoutineStop();
        AdditionalSpawnerHolder.SetActive(false);
        OpenCloseMainSpawners(false);
        OpenCloseAdditionalSpawners(false);
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
        OpenCloseMainSpawners(false);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
        if (GameManager != null)
        {
            GameManager.OnStartGame -= OnLevelStart;
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
    #endregion
}
