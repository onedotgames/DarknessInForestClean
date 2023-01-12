using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class LevelFinished : UIPanel
{
    [Title("Fail Group")]
    public CustomButton ReplayButton;
    public CanvasGroup FailCanvas;
    public Animator FailAnimator;
    public GameObject FailImg;
    [Title("Success Group")]
    public CustomButton ContinueButton;
    public CanvasGroup SuccessCanvas;
    public Animator SuccessAnimator;
    public GameObject SuccessImg;

    public GameObject BoxVolumeParent;
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private Coroutine mLoseRout;
    private Coroutine mWinRout;

    private int mDiamondReward;
    private HUD hud;

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        ReplayButton.Initialize(uIManager, OnReplayButtonClicked);
        ContinueButton.Initialize(uIManager, OnContinueButtonClicked);
        InitializeEvents();
        hud = uIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        volume = BoxVolumeParent.GetComponent<Volume>();

        if(volume.profile.TryGet<ColorAdjustments>(out var colorAd))
        {
            colorAdjustments = colorAd;
        }
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
        colorAdjustments.saturation.value = 0;
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
        SuccessImg.GetComponent<PlayAnimation>().Play();
        colorAdjustments.saturation.value = 0;
        WinSaturation();
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
        FailImg.GetComponent<PlayAnimation>().Play();
        colorAdjustments.saturation.value = 0;
        LoseSaturation();
    }

    public void WinSaturation()
    {
        mWinRout = StartCoroutine(SaturationRoutine(5f));
    }

    public void LoseSaturation()
    {
        mLoseRout = StartCoroutine(SaturationRoutine(-5f));
    }

    private IEnumerator SaturationRoutine(float add)
    {
        while (colorAdjustments.saturation.value != 100 || colorAdjustments.saturation.value != -100)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            colorAdjustments.saturation.value += add;
        }
        //if(colorAdjustments.saturation.value != 100 || colorAdjustments.saturation.value != -100)
        //{
            //yield return new WaitForSecondsRealtime(0.1f);
            //colorAdjustments.saturation.value += add;
            //if (colorAdjustments.saturation.value != 100f)
            //{
              //  StartCoroutine(SaturationRoutine(add));
            //}
        //}
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
