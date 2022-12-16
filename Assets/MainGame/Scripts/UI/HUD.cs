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
using Assets.FantasyMonsters.Scripts;
using System.Xml.Linq;

public class HUD : UIPanel, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text CoinCount;
    public TMP_Text KillCount;
    public Slider KillSlider;
    public TMP_Text GameTime;
    
    public float killCount = 0;
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

    public List<Image> WeaponIconsOnPause;
    public int WeaponIndex = 0;
    public List<Image> UtilIconsOnPause;
    public int UtilIndex = 0;

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

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        playerLevelManager = GameManager.PlayerLevelManager;
        PauseButton.Initialize(uIManager, PauseGame, true);
        ResumeButton.Initialize(uIManager, ResumeGame, true);
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
        WarningBool = true;
        BossWarningObject.SetActive(true);
        BossWarningText.transform.DOScale(1.05f, 0.2f).SetLoops(-1, LoopType.Yoyo).From(1f);
        yield return new WaitForSeconds(WarningDuration);
        WarningBool = false;
        BossWarningObject.SetActive(false);

    }
    public void SetExpBarFillAmount()
    {
        ExpText.SetText(playerLevelManager.GetCurrentExp().ToString() + "/" + playerLevelManager.GetLvlReq().ToString());
        ExperienceBar.DOValue((playerLevelManager.GetCurrentExp() / playerLevelManager.GetLvlReq()), 1f).OnComplete(() =>
        {
            playerLevelManager.CheckExp();
        });

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
    private void SubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame += OnStartGame;
            GameManager.OnGameFinished += OnGameFinished;
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
            GameManager.OnLevelFailed += OnLevelFailed;
        }
    }

    public override void OpenPanel()
    {
        UpdateUIElements();
        base.OpenPanel();
        Debug.Log("HUD Open");

    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        Debug.Log("HUD CLOSE");
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
        ClosePanel();
    }

    public void UpdateKillCountBar()
    {
        killCount++;
        KillSlider.DOValue((killCount / (killCount + 100)), 0.25f);
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= OnStartGame;
            GameManager.OnGameFinished -= OnGameFinished;
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
            GameManager.OnLevelFailed -= OnLevelFailed;
        }
    }

}




