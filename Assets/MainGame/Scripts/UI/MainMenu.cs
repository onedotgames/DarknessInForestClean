using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIPanel, IPointerDownHandler, IPointerUpHandler
{
    #region Buttons
    public CustomButton PlayButton;
    public CustomButton SkinsButton;
    public CustomButton SettingsButton;
    public CustomButton BuyCoins;
    #endregion

    #region Actions
    public event Action OnButtonClickedAlertTutor;
    #endregion

    #region Fields
    public TMP_Text TextPlayButton;
    public TMP_Text LevelNumber;
    public TMP_Text CoinCount;
    private TMP_InputField mNameInputField;
    #endregion

    #region Methods
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        PlayButton.Initialize(uIManager, OnButtonClickedPlay);
        SkinsButton.Initialize(uIManager, OnButtonClickedSkins, false); //Remove false to make it visible
        SettingsButton.Initialize(uIManager, OnButtonClickedSettings);
        BuyCoins.Initialize(uIManager, OnButtonClickedSettings, false);
        InitializeCustomOptions();

        GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
    }

    private void InitializeCustomOptions()
    {
        switch (GameManager.GameOptions.StartTextType)
        {
            case StartTextTypes.SwipeToPlay:
                SetPlayButtonToSwipe();
                break;
            case StartTextTypes.TapToPlay:
                SetPlayButtonToTap();
                break;
        }
    }

    private void UpdatePlayerName()
    {
        if(mNameInputField != null)
        {
            var temporaryName = mNameInputField.text;

            if (!string.IsNullOrEmpty(temporaryName))
            {
                GameManager.PlayerManager.UpdateNameData(temporaryName);
            }
        }
    }

    public override void OpenPanel()
    {
        
        GameManager.InputManager.OnSwiped += OnSwiped;
        UpdateUIElements();
       
        base.OpenPanel();
        
        
    }

    public override void ClosePanel()
    {
        GameManager.InputManager.OnSwiped -= OnSwiped;
        
        base.ClosePanel();
        //gameObject.SetActive(false);
    }

    public override void UpdateUIElements()
    {
        base.UpdateUIElements();
        LevelNumber.SetText("LEVEL" + GameManager.LevelManager.CurrentLevelNumber);
        CoinCount.SetText(GameManager.PlayerManager.GetTotalCoinCount().ToString());

        GameManager.GameOptions.GetName();
    }

    private void SetPlayButtonToSwipe()
    {
        TextPlayButton.SetText(ConstantDatas.SWIPE_TO_PLAY);
        TextPlayButton.raycastTarget = false;
        PlayButton.interactable = false;
        PlayButton.image.raycastTarget = false;
        TextPlayButton.transform.DOScale(1.05f, 0.2f).SetLoops(-1, LoopType.Yoyo).From(1f);
    }

    private void SetPlayButtonToTap()
    {
        TextPlayButton.SetText(ConstantDatas.TAP_TO_PLAY);
        TextPlayButton.raycastTarget = false;
        PlayButton.interactable = true;
        PlayButton.image.raycastTarget = true;
        TextPlayButton.transform.DOScale(1.05f, 0.2f).SetLoops(-1, LoopType.Yoyo).From(1f);
    }

    public void SetNameInputFieldTo(TMP_InputField field)
    {
        mNameInputField = field;
    }
    #endregion

    #region Events
    private void OnButtonClickedPlay()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        UpdatePlayerName();
        ClosePanel();
        if(OnButtonClickedAlertTutor != null)
        {
            OnButtonClickedAlertTutor();
        }
        GameManager.StartGame();
    }

    private void OnButtonClickedSettings()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        UIManager.GetPanel(Panels.Settings).OpenPanel();
    }

    private void OnButtonClickedSkins()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        //UIManager.GetPanel(Panels.Skins).ShowPanel();
        ClosePanel();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.GameManager.InputManager.Touched(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIManager.GameManager.InputManager.TouchEnd(eventData);
    }
    private void OnReturnToMainMenu()
    {
        OpenPanel();
    }

    private void OnSwiped()
    {
        OnButtonClickedPlay();
    }
    #endregion
}
