using UnityEngine;
using System;
//using GameAnalyticsSDK;
//using Facebook.Unity;

public class GameManager : CustomBehaviour
{
    #region Managers
    public GameOptions GameOptions;
    public JsonConverter JsonConverter;
    public InputManager InputManager;
    public SoundManager SoundManager;
    public JoystickManager JoystickManager;
    public PlayerManager PlayerManager;
    public AIManager AIManager;
    public LevelManagerBaseGame LevelManager;
    public VibrationsManager VibrationsManager;
    public RewardManager RewardManager;
    public CameraManager CameraManager;
    public ParticleManager ParticleManager;
    public UIManager UIManager;
    public PlayerLevelManager PlayerLevelManager;
    public PlayerHealthManager PlayerHealthManager;
    public PoolingManager PoolingManager;
    //public WeaponManager WeaponManager;
    public BarrelSystem BarrelSystem;
    public SpawnerManager SpawnerManager;
    public TimeManager TimeManager;
    public QuestManager QuestManager;
    public HolyFountain HolyFountain;
    public SkillManager SkillManager;
    public EnvironmentGenerator EnvironmentGenerator;
    public CoopManager CoopManager;
    public InventoryManager InventoryManager;
    public BackgroundManager BackgroundManager;
    public ChallengeManager ChallengeManager;
    public TutorialManagerBaseClass TutorialManagerBase;
    public AnimationManager AnimationManager;
    public LevelManager Match3LevelManager;
    #endregion

    public bool IsGameStarted = false;
    public bool IsGamePaused = false;
    public bool IsBossTime = false;
    public bool IsDevelopmentModeOn = false;
    public bool IsMiniGame = false;

    #region Actions
    public event Action OnStartGame;
    public event Action OnReturnToMainMenu;
    public event Action OnRestartGame;
    public event Action OnResumeGame;
    public event Action OnLevelCompleted;
    public event Action OnTutorialCompleted;
    public event Action OnLevelFailed;
    public event Action OnMiniGame;
    public event Action OnBossSpawn;
    #endregion

    #region Methods
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //if (!FB.IsInitialized)
        //{
        //    // Initialize the Facebook SDK
        //    FB.Init(InitCallback, OnHideUnity);
        //}
        //else
        //{
        //    // Already initialized, signal an app activation App Event
        //    FB.ActivateApp();
        //}

        InitializeManagers();
        //GameAnalytics.Initialize();
    }
    //private void InitCallback()
    //{
    //    if (FB.IsInitialized)
    //    {
    //        // Signal an app activation App Event
    //        FB.ActivateApp();
    //        // Continue with Facebook SDK
    //        // ...
    //    }
    //    else
    //    {
    //        Debug.Log("Failed to Initialize the Facebook SDK");
    //    }
    //}

    //private void OnHideUnity(bool isGameShown)
    //{
    //    if (!isGameShown)
    //    {
    //        // Pause the game - we will need to hide
    //        Time.timeScale = 0;
    //    }
    //    else
    //    {
    //        // Resume the game - we're getting focus again
    //        Time.timeScale = 1;
    //    }
    //}
    private void InitializeManagers()
    {
        GameOptions.Initialize(this);
        JsonConverter.Initialize(this);
        InputManager.Initialize(this);
        SoundManager.Initialize(this);
        JoystickManager.Initialize(this);
        PlayerManager.Initialize(this);      
        AIManager.Initialize(this);
        LevelManager.Initialize(this);
        VibrationsManager.Initialize(this);
        RewardManager.Initialize(this);
        CameraManager.Initialize(this);
        ParticleManager.Initialize(this);
        UIManager.Initialize(this);
        PlayerLevelManager.Initialize(this);
        PlayerHealthManager.Initialize(this);
        PoolingManager.Initialize(this);
        //WeaponManager.Initialize(this);
        BarrelSystem.Initialize(this);
        SpawnerManager.Initialize(this);
        TimeManager.Initialize(this);
        QuestManager.Initialize(this);
        HolyFountain.Initialize(this);
        SkillManager.Initialize(this);
        EnvironmentGenerator.Initialize(this);
        CoopManager.Initialize(this);
        InventoryManager.Initialize(this);
        BackgroundManager.Initialize(this);
        ChallengeManager.Initialize(this);
        TutorialManagerBase.Initialize(this);
        AnimationManager.Initialize(this);
        Match3LevelManager.Initialize(this);
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LevelFailed();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LevelCompleted();
        }
    }

    #region Event Methods
    public void StartGame()
    {
        if(OnStartGame != null)
        {
            OnStartGame();
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, PlayerManager.GetLevelNumber().ToString());
            IsGameStarted = true;
        }
    }

    public void ReturnToMainMenu()
    {
        if(OnReturnToMainMenu != null)
        {
            OnReturnToMainMenu();
        }
    }

    public void MiniGame()
    {
        if(OnMiniGame != null)
        {
            OnMiniGame();
        }
    }

    public void RestartGame()
    {
        if (OnRestartGame != null)
        {
            IsGameStarted = true;
            OnRestartGame();
        }
    }

    public void ResumeGame()
    {
        if(OnResumeGame != null)
        {
            IsGameStarted = false;
            OnResumeGame();
        }
    }

    public void LevelCompleted()
    {
        if(OnLevelCompleted != null)
        {
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, PlayerManager.GetLevelNumber().ToString());
            IsGameStarted = false;
            OnLevelCompleted();
        }
    }
    public void LevelFailed()
    {
        if (OnLevelFailed != null)
        {
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, PlayerManager.GetLevelNumber().ToString());
            IsGameStarted = false;
            OnLevelFailed();     
        }
    }

    public void BossSpawn()
    {
        if(OnBossSpawn != null)
        {
            OnBossSpawn();
        }
    }
    public void TutorialCompleted()
    {
        if (OnTutorialCompleted != null)
        {
            OnTutorialCompleted();
        }
    }
    #endregion
}
