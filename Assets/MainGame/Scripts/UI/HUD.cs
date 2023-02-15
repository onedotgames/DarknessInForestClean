using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


public class HUD : UIPanel, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text CoinCount;
    public TMP_Text KillCount;
    public TMP_Text GameTime;
    
    public int killCount = 0;
    [Title("INPUT")]
    public GuideTypes GuideType;// = GuideTypes.None;
    public List<GameObject> SwipeGuides;
    public GameObject VariableJoystick;
    public Slider ExperienceBar;
    public Image HealthBar;

    public TMP_Text LevelText;
    public TMP_Text ExpText;

    public CustomButton PauseButton;
    public CustomButton ResumeButton;
    public CustomButton ReplayButton;
    public CustomButton QuitButton;

    public CustomToggle VibrationToggle;
    public GameObject VibrationToggleOn;
    public GameObject VibrationToggleOff;

    public CustomToggle DevToggle;
    public GameObject DevToggleOn;
    public GameObject DevToggleOff;
    public GameObject DevToolBTNS;

    public CustomButton DevLevelUpBTN;

    public GameObject EnemyWarningObject;
    public GameObject BossWarningObject;
    public TMP_Text EnemyRushWarningText;
    public TMP_Text BossWarningText;
    public float WarningDuration;
    public bool WarningBool = false;

    [SerializeField] private Volume volume;
    private Vignette vignette;

    public GameObject BossHpGroup;
    public Slider BossHPBar;
    public TMP_Text BossHPTXT;

    public Slider GameEventSlider;

    private PlayerLevelManager playerLevelManager;

    public TMP_Text FPSCounter;

    public GameObject PlayerIcon;

    public RectTransform TopText;
    public RectTransform BottomText;
    public GameObject BossImage;

    public Animator ExpSliderAnim;
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        playerLevelManager = GameManager.PlayerLevelManager;
        PauseButton.Initialize(uIManager, PauseGame, true);
        ResumeButton.Initialize(uIManager, ResumeGame, true);
        ReplayButton.Initialize(uIManager, OnReplayButtonClicked, true);
        QuitButton.Initialize(uIManager, OnQuitButtonClicked, true);
        DevToggle.Initialize(uIManager, OnDevToggleClicked);
        VibrationToggle.Initialize(uIManager, OnVibrationToggleClicked);
        DevLevelUpBTN.Initialize(uIManager, DevLevelUp, true);
        SubscribeEvents();
        OpenCloseJoystick();
        ClosePanel();

        if (volume.profile.TryGet<Vignette>(out var vig))
        {
            vignette = vig;
        }
    }

    public void SetBossHPBarActivation(bool setValue)
    {
        BossHpGroup.SetActive(setValue);
    }

    public void SetBossFillValue(float currentHP, float maxHP)
    {
        BossHPBar.DOValue((currentHP/maxHP),0.5f);
    }
    public void SetBossFillText(float currentHP, float maxHP)
    {
        BossHPTXT.SetText(currentHP.ToString() + "/" + maxHP.ToString());
    }

    public IEnumerator RushRoutine()
    {
        WarningBool = true;
        EnemyWarningObject.SetActive(true);
        EnemyRushWarningText.transform.DOScale(1.05f, 0.2f).SetLoops(-1, LoopType.Yoyo).From(1f);
        yield return new WaitForSeconds(WarningDuration);
        WarningBool = false;
        EnemyWarningObject.SetActive(false);
    }

    public IEnumerator BossRoutine()
    {
        //WarningBool = true;
        BossWarningObject.SetActive(true);
        BossImage.transform.DOScale(.75f, 0.4f).SetLoops(-1, LoopType.Yoyo).From(.95f);
        TopText.DOAnchorPosX(800f, 3f).SetLoops(-1, LoopType.Restart).From(new Vector2(-800,0));
        BottomText.DOAnchorPosX(800f, 3f).SetLoops(-1, LoopType.Restart).From(new Vector2(-800, 0));

        yield return new WaitForSeconds(WarningDuration);
        //WarningBool = false;
        BossWarningObject.SetActive(false);

    }
    

    private void ResetBossUI()
    {
        BossWarningObject.SetActive(false);
        SetBossHPBarActivation(false);
    }
    public void SetExpBarFillAmount()
    {
        SetExpText();
        ExpSliderAnim.enabled = true;
        if(ExperienceBar.value <= 1)
        {
            ExperienceBar.DOValue((playerLevelManager.GetCurrentExp() / playerLevelManager.GetLvlReq()), 0.5f).OnComplete(() =>
            {
                playerLevelManager.CheckExp();
                ExpSliderAnim.enabled = false;
            });
        }
        
        
    }

    public void SetExpText()
    {
        ExpText.SetText(playerLevelManager.GetCurrentExp().ToString() + "/" + playerLevelManager.GetLvlReq().ToString());

    }

    public void UpdateLevelText()
    {
        LevelText.SetText(playerLevelManager.PlayerLevel.ToString());
    }

    

    private void Update()
    {
        
        if (!GameManager.IsGamePaused)
        {
            if (!GameManager.IsBossTime)
            {
                int minutes = Mathf.FloorToInt(GameManager.TimeManager.GetTimeValue() / 60);
                int seconds = Mathf.FloorToInt(GameManager.TimeManager.GetTimeValue() % 60);
                GameTime.SetText( string.Format("{0:00}:{1:00}", minutes,seconds));
                GameEventSlider.DOValue(GameManager.TimeManager.GetTimeValue() / GameManager.TimeManager.GetTargetTimeValueInSeconds(), 0.25f);
            }
            
        }
        if(WarningBool)
        {
            vignette.intensity.value = Mathf.PingPong(Time.time * 0.75f, .5f);
            if (vignette.intensity.value <= 0.02f)
            {
                vignette.intensity.value = 0;
            }
        }
        else
        {
            if (vignette.intensity.value != 0.02f)
            {
                vignette.intensity.value = 0;
            }
        }
    }

    private void OpenCloseJoystick()
    {
        if(GameManager.GameOptions.ControlType == ControlType.Joystick)
        {
            VariableJoystick.SetActive(true);
        }
        else
        {
            VariableJoystick.SetActive(false);
        }
    }
    public void ToggleDevMode()
    {
        GameManager.IsDevelopmentModeOn = !GameManager.IsDevelopmentModeOn;
    }

    private void SubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame += OnStartGame;
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
            GameManager.OnLevelFailed += OnLevelFailed;
            GameManager.OnLevelCompleted += OnLevelCompleted;
        }
    }

    public override void OpenPanel()
    {
        UpdateUIElements();
        GameManager.UIManager.GetPanel(Panels.Initial).ClosePanel();
        PlayerIcon.SetActive(true);
        base.OpenPanel();
    }

    public override void ClosePanel()
    {
        PlayerIcon.SetActive(false);
        base.ClosePanel();
    }

    public override void UpdateUIElements()
    {
        base.UpdateUIElements();
        CoinCount.SetText(GameManager.PlayerManager.GetTotalCoinCount().ToString());
    }

    private void PauseGame()
    {
        ClosePanel();
        GameManager.UIManager.GetPanel(Panels.Pause).OpenPanel();
        GameManager.IsGamePaused = true;
        
    }
    private void ResumeGame()
    {
        GameManager.UIManager.GetPanel(Panels.Pause).ClosePanel();
        OpenPanel();

        GameManager.IsGamePaused = false;
    }

    private void OnQuitButtonClicked()
    {
        //Application.Quit();
        //Debug.Log("Quit");
        UnityEngine.SceneManagement.SceneManager.LoadScene
                (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnReplayButtonClicked()
    {
        //GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        //GameManager.UIManager.GetPanel(Panels.Pause).ClosePanel();

        //GameManager.ReturnToMainMenu();
        UnityEngine.SceneManagement.SceneManager.LoadScene
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        GameManager.UIManager.GetPanel(Panels.MainMenu).OpenPanel();
        GameManager.UIManager.GetPanel(Panels.Initial).ClosePanel();
        Debug.Log("Level baştan başlamalı");
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIManager.GameManager.InputManager.Interactable)
        {
            UIManager.GameManager.InputManager.Touched(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIManager.GameManager.InputManager.TouchEnd(eventData);
    }
    private void OnDevToggleClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        ToggleDevMode();
        SwithDevToggleBG();
        Debug.Log("DevMode: " + GameManager.IsDevelopmentModeOn);
    }
    private void OnVibrationToggleClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        GameManager.VibrationsManager.ToggleVibration();
        SwithVibrationToggle();
    }

    private void DevLevelUp()
    {
        GameManager.PlayerLevelManager.SetExp(GameManager.PlayerLevelManager.LevelRequirement);
        SetExpBarFillAmount();
    }

    private void SwithDevToggleBG()
    {
        if (GameManager.IsDevelopmentModeOn)
        {
            DevToggleOn.SetActive(true);
            DevToggleOff.SetActive(false);
            DevToolBTNS.SetActive(true);
        }
        else if (!GameManager.IsDevelopmentModeOn)
        {
            DevToggleOn.SetActive(false);
            DevToggleOff.SetActive(true);
            DevToolBTNS.SetActive(false);

        }
    }
    private void SwithVibrationToggle()
    {
        if (!GameManager.VibrationsManager.IsVibrationOn)
        {
            VibrationToggleOn.SetActive(false);
            VibrationToggleOff.SetActive(true);
            Debug.Log(GameManager.VibrationsManager.IsVibrationOn);
        }
        else 
        {
            VibrationToggleOn.SetActive(true);
            VibrationToggleOff.SetActive(false);
            Debug.Log(GameManager.VibrationsManager.IsVibrationOn);

        }
    }
    private void OnStartGame()
    {
        OpenPanel();
        killCount = 0;
        KillCount.text = killCount.ToString();
    }

    private void OnGameFinished()
    {
        ClosePanel();
    }

    private void OnReturnToMainMenu()
    {

        ClosePanel();
    }
    private void OnLevelFailed()
    {
        ResetBossUI();
        ClosePanel();
    }
    private void OnLevelCompleted()
    {
        ResetBossUI();
        ClosePanel();
    }

    public void UpdateKillCountBar()
    {
        killCount++;
        KillCount.text = killCount.ToString();
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= OnStartGame;
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
        }
    }

}




