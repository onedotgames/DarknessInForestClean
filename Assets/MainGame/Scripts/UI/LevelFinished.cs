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

    [Title("Reward Group")]
    public TMP_Text DiamondRewardText;
    public TMP_Text TotalDiamondText;
    public Image DiamondRewardImage;
    public Image TotalDiamondImage;

    public VideoPlayer WinPlayer;
    public Animator WinAnimator;
    public Animator LoseAnimator;
    public VideoPlayer FailPlayer;

    public GameObject BoxVolumeParent;
    private int mDiamondReward;

    private HUD hud;

    #region Methods
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
            StartCoroutine(AnimateRewards(1.5f, 2f));
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

        var totalDiamonds = GameManager.PlayerManager.GetTotalCoinCount();

        var tmpDiamonds = totalDiamonds - mDiamondReward;


        if (tmpDiamonds <= 0) 
        { 
            tmpDiamonds = 0; 
        }

        DiamondRewardText.SetText(mDiamondReward.ToString());


    }

    private IEnumerator AnimateRewards(float delayOnStart, float duration)
    {
        if(delayOnStart > 0f)
        {
            yield return new WaitForSeconds(delayOnStart);
        }
        AnimateDiamondImage(duration);
        StartCoroutine(AnimateRewardText(duration));
    }

    private void AnimateDiamondImage(float duration)
    {
        var fromImage = DiamondRewardImage;
        var toImage = TotalDiamondImage;
        var imageCount = 5;

        for(int i = 1; i < imageCount + 1; i++)
        {
            var tmpImage = Instantiate(fromImage, fromImage.transform.position, Quaternion.identity, RectTransform);

            var durationScale = (float)i / imageCount;

            if(tmpImage != null && tmpImage.isActiveAndEnabled)
            {
                tmpImage.transform.DOScale(Vector3.one, duration * durationScale);
                tmpImage.transform.DOMove(toImage.transform.position, duration * durationScale);
            }
            

            Destroy(tmpImage.gameObject, duration * durationScale);
        }
    }

    private IEnumerator AnimateRewardText(float duration)
    {
        float increasePerSecond = mDiamondReward / duration;
        float endValue = GameManager.PlayerManager.GetTotalCoinCount();
        float tmpValue = endValue - mDiamondReward;

        if (tmpValue <= 0)
        {
            tmpValue = 0;
        }

        TotalDiamondText.SetText(tmpValue.ToString());

        while(endValue > tmpValue)
        {
            tmpValue += Time.deltaTime * increasePerSecond;
            TotalDiamondText.SetText(((int)tmpValue).ToString());

            yield return null;
        }
    }
    #endregion

    #region Event Methods

    private void LevelCompleted()
    {
        mDiamondReward = ConstantDatas.LEVEL_COMPLETE_REWARD;

        FailCanvas.Close();
        hud.ClosePanel();
        SuccessCanvas.Open();
        OpenPanel();
        //WinPlayer.Play();
        //WinAnimator.Play("VictoryAnim");

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


    #endregion
}
