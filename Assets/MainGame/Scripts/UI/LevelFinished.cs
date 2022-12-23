using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Video;

public class LevelFinished : UIPanel
{
    [Title("Fail Group")]
    public CustomButton ReplayButton;
    public CanvasGroup FailCanvas;

    [Title("Success Group")]
    public CustomButton ContinueButton;
    public CanvasGroup SuccessCanvas;

    public GameObject BoxVolumeParent;
    private int mDiamondReward;

    private HUD hud;

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        ReplayButton.Initialize(uIManager, OnReplayButtonClicked);
        ContinueButton.Initialize(uIManager, OnContinueButtonClicked);
        InitializeEvents();
        hud = uIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
    }

    private void OnContinueButtonClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        ClosePanel();
        GameManager.ReturnToMainMenu();
    }

    private void OnReplayButtonClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        ClosePanel();
        GameManager.ReturnToMainMenu();
    }

    private void InitializeEvents()
    {
        if(GameManager != null)
        {
            UIManager.GameManager.OnLevelCompleted += LevelCompleted;
            UIManager.GameManager.OnTutorialCompleted += LevelCompleted;
            UIManager.GameManager.OnLevelFailed += LevelFailed; 
            UIManager.GameManager.OnReturnToMainMenu += ReturnToMain;
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        if (GameManager.PlayerManager.IsTutorialPassed())
        {
            UpdateUIElements();
        }

    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        SuccessCanvas.Close();
        SuccessCanvas.gameObject.SetActive(false);

        FailCanvas.Close();
        FailCanvas.gameObject.SetActive(false);
    }

    public override void UpdateUIElements()
    {
        base.UpdateUIElements();
    }


    private void LevelCompleted()
    {
        BoxVolumeParent.SetActive(true);
        mDiamondReward = ConstantDatas.LEVEL_COMPLETE_REWARD;

        FailCanvas.Close();
        FailCanvas.gameObject.SetActive(false);

        SuccessCanvas.Open();
        SuccessCanvas.gameObject.SetActive(true);

        OpenPanel();
    }
    private void ReturnToMain()
    {
        FailCanvas.Close();;
        SuccessCanvas.Close();

        FailCanvas.gameObject.SetActive(false);
        SuccessCanvas.gameObject.SetActive(false);
        BoxVolumeParent.SetActive(false);

        ClosePanel();
    }
    private void LevelFailed()
    {
        BoxVolumeParent.SetActive(true);
        mDiamondReward = ConstantDatas.LEVEL_FAIL_REWARD;

        SuccessCanvas.Close();
        SuccessCanvas.gameObject.SetActive(false);

        FailCanvas.Open();
        FailCanvas.gameObject.SetActive(true);

        OpenPanel();
    }


    private void OnDestroy()
    {
        if(GameManager != null)
        {
            UIManager.GameManager.OnLevelCompleted -= LevelCompleted;
            UIManager.GameManager.OnTutorialCompleted -= LevelCompleted;
            UIManager.GameManager.OnLevelFailed -= LevelFailed;
            UIManager.GameManager.OnReturnToMainMenu -= ReturnToMain;
        }
    }
}
