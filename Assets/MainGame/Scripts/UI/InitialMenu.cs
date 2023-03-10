using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine.EventSystems;

public class InitialMenu : UIPanel
{

    public float MenuSwipeSpeed;
    public float ButtonAnimationSpeed;

    [Title("Top Group")]
    public CustomButton Play;

    [Title("Canvas Group")]
    public CanvasGroup[] Canvasses;
    public CanvasGroup ShopCanvas;
    public CanvasGroup EquipmentCanvas;
    public CanvasGroup MainCanvas;
    public CanvasGroup ChallangeCanvas;
    public CanvasGroup LeaderBoardCanvas;

    public CanvasGroup LevelCanvas;


    [Title("BottomGroup")]
    public CustomButton CoopBTN;
    public CustomButton EquipmentBTN;
    public CustomButton HomeBTN;
    public CustomButton LevelsBTN;
    public CustomButton LeaderBoardBTN;

    public CustomButton[] MenuButtonArray;

    public RectTransform Content;
    public RectTransform MidGroup;
    public RectTransform Coop;
    public RectTransform Equipment;
    public RectTransform Home;
    public RectTransform Levels;
    public RectTransform LeaderBoard;


    public RectTransform LevelScroller;
    public GameObject PlayerImg;

    public MenuEvent LeavesEvent;
    public MenuEvent FireFliesEvent;
    public MenuEvent BossEvent;

    public GameObject UserInfoPanel;

    public GameObject RedBoxBG;
    public RectTransform rectBG { get; private set; }

    public Slider HealthSlider;
    public Slider AttackSlider;
    public Slider DamageReductionSlider;
    public Slider SpeedSlider;
    public bool isSwipe = true;
    private InventoryManager inventoryManager;

    [Header("Energy System")]
    public TMP_Text EnergyText;
    public int EnergyRechargeTime;
    public Player player;
    private float EnergyTimer;
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        Play.Initialize(uIManager, OnPlayButtonClicked);
        CoopBTN.Initialize(uIManager, OnCoopBTNClicked);
        EquipmentBTN.Initialize(uIManager, OnEquipmentBTNClicked);
        HomeBTN.Initialize(uIManager, OnHomeBTNClicked);
        LevelsBTN.Initialize(uIManager, OnLevelsBTNClicked);
        LeaderBoardBTN.Initialize(uIManager, OnLeaderBoardBTNClicked);
        LeavesEvent.Initialize(GameManager);
        FireFliesEvent.Initialize(GameManager);
        BossEvent.Initialize(GameManager);
        GameManager.UIManager.GetPanel(Panels.Hud).ClosePanel();

        
        LevelScroller.sizeDelta = new Vector2(Screen.width, LevelScroller.sizeDelta.y);
        Content.sizeDelta = new Vector2(Screen.height * 5, 0);
        MidGroup.sizeDelta = new Vector2(Screen.height * 5, Screen.width);

        Home.DOScale(Vector3.one * 1.3f, 0.4f);
        Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
        Coop.DOScale(Vector3.one * 0.9f, 0.4f);
        Levels.DOScale(Vector3.one * 0.9f, 0.4f);
        LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);

        rectBG = RedBoxBG.GetComponent<RectTransform>();
        rectBG.DOLocalMoveX(HomeBTN.transform.position.x, 0.3f).SetEase(Ease.OutQuad);
        rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 3));

        inventoryManager = uIManager.GameManager.InventoryManager;
        SetEquipmentSectionBars();
        SetEnergyText();
        SubEvents();
    }

    private void SubEvents()
    {
        if(GameManager != null)
        {
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
            GameManager.OnStartGame += StartGame;
        }
    }

    private void SetEnergyText()
    {
        EnergyText.text = player.PlayerVariables.UserCurrentEnergy + " / " + player.PlayerVariables.MaxEnergy;
    }

    private void Update()
    {
        if (!GameManager.IsGameStarted)
        {
            EnergyTimer += Time.deltaTime;
            if (EnergyRechargeTime == (int)EnergyTimer)
            {
                Debug.Log("Enerji artt??");
                player.PlayerVariables.UserCurrentEnergy++;
                EnergyText.text = player.PlayerVariables.UserCurrentEnergy + " / " + player.PlayerVariables.MaxEnergy;
                EnergyTimer = 0;
            }
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        GameManager.SoundManager.PlayInitialMenuSound();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        GameManager.SoundManager.StopInitialMenuSound();
    }

    private void OnCoopBTNClicked()
    {
        inventoryManager.ClosePopUpButtonsOnSwipe();
        isSwipe = false;
        GameManager.SoundManager.PlaySwitchScreenSound();

        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(Screen.height * 2, MenuSwipeSpeed).OnComplete(() => isSwipe = true);
        PlayerImg.GetComponent<SpriteRenderer>().enabled = false;

        Coop.DOScale(Vector3.one * 1.3f, 0.4f);
        Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
        Home.DOScale(Vector3.one * 0.9f, 0.4f);
        Levels.DOScale(Vector3.one * 0.9f, 0.4f);
        LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);

        rectBG.DOLocalMoveX(CoopBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
        rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, -1));
    }
    private void OnEquipmentBTNClicked()
    {
        isSwipe = false;
        GameManager.SoundManager.PlaySwitchScreenSound();

        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(Screen.height, MenuSwipeSpeed).OnComplete(() => isSwipe = true);
        PlayerImg.GetComponent<SpriteRenderer>().enabled = true;

        Equipment.DOScale(Vector3.one * 1.3f, 0.4f);
        Coop.DOScale(Vector3.one * 0.9f, 0.4f);
        Home.DOScale(Vector3.one * 0.9f, 0.4f);
        Levels.DOScale(Vector3.one * 0.9f, 0.4f);
        LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);

        rectBG.DOLocalMoveX(EquipmentBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
        rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
    }
    private void OnHomeBTNClicked()
    {
        inventoryManager.ClosePopUpButtonsOnSwipe();
        isSwipe = false;
        GameManager.SoundManager.PlaySwitchScreenSound();
        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(0, MenuSwipeSpeed).OnComplete(() => isSwipe = true);
        PlayerImg.GetComponent<SpriteRenderer>().enabled = true;

        Home.DOScale(Vector3.one * 1.3f, 0.4f);
        Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
        Coop.DOScale(Vector3.one * 0.9f, 0.4f);
        Levels.DOScale(Vector3.one * 0.9f, 0.4f);
        LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);

        rectBG.DOLocalMoveX(HomeBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
        rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 3));

    }
    private void OnLevelsBTNClicked()
    {
        inventoryManager.ClosePopUpButtonsOnSwipe();
        isSwipe = false;
        GameManager.SoundManager.PlaySwitchScreenSound();

        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(-Screen.height, MenuSwipeSpeed).OnComplete(() => isSwipe = true);
        PlayerImg.GetComponent<SpriteRenderer>().enabled = false;


        Levels.DOScale(Vector3.one * 1.3f, 0.4f);
        Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
        Home.DOScale(Vector3.one * 0.9f, 0.4f);
        Coop.DOScale(Vector3.one * 0.9f, 0.4f);
        LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);

        rectBG.DOLocalMoveX(LevelsBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
        rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
    }
    private void OnLeaderBoardBTNClicked()
    {
        inventoryManager.ClosePopUpButtonsOnSwipe();
        isSwipe = false;
        GameManager.SoundManager.PlaySwitchScreenSound();

        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(-Screen.height * 2, MenuSwipeSpeed).OnComplete(() => isSwipe = true);
        PlayerImg.GetComponent<SpriteRenderer>().enabled = false;


        LeaderBoard.DOScale(Vector3.one * 1.3f, 0.4f);
        Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
        Home.DOScale(Vector3.one * 0.9f, 0.4f);
        Levels.DOScale(Vector3.one * 0.9f, 0.4f);
        Coop.DOScale(Vector3.one * 0.9f, 0.4f);

        rectBG.DOLocalMoveX(LeaderBoardBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
        rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 7));
    }

    public void SetEquipmentSectionBars()
    {
        HealthSlider.DOValue((inventoryManager.GlobalHealthIncrease / 1000), 0.5f).SetEase(Ease.OutSine);
        AttackSlider.DOValue((inventoryManager.GlobalDamageIncrease / 1000), 0.5f).SetEase(Ease.OutSine);
        DamageReductionSlider.DOValue((inventoryManager.GlobalDamageReduction / 20), 0.5f).SetEase(Ease.OutSine);
        SpeedSlider.DOValue((inventoryManager.GlobalSpeedIncrease / 20), 0.5f).SetEase(Ease.OutSine);
    }

    private void SetCanvas(CanvasGroup canvas)
    {
        foreach (var cnvs in Canvasses)
        {
            if(cnvs == canvas)
            {
                cnvs.Open();
            }
            else
            {
                cnvs.Close();
            }
        }
    }

    private void OnPlayButtonClicked()
    {
        if(player.PlayerVariables.UserCurrentEnergy >= 5)
        {
            GameManager.ChallengeManager.isChallengeLevel = false;
            GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
            //GameManager.UIManager.GetPanel(Panels.MainMenu).OpenPanel();
            ClosePanel();
            UserInfoPanel.SetActive(false);
            GameManager.StartGame();
            player.PlayerVariables.UserCurrentEnergy -= 0;
            EnergyText.text = player.PlayerVariables.UserCurrentEnergy + " / " + player.PlayerVariables.MaxEnergy;
        }
        else
        {
            Debug.Log("Yeterli Enerjim yok");
        }
    }

    private void OnReturnToMainMenu()
    {
        OpenPanel();
        UserInfoPanel.SetActive(true);
    }

    private void StartGame()
    {
        ClosePanel();
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
        }
    }
}
