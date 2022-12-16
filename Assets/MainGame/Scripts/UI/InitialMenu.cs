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
    public CustomButton Coop;
    public CustomButton EquipmentBTN;
    public CustomButton HomeBTN;
    public CustomButton LevelsBTN;
    public CustomButton LeaderBoardBTN;

    public CustomButton[] MenuButtonArray;

    public RectTransform Content;

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        Play.Initialize(uIManager, OnPlayButtonClicked);
        Coop.Initialize(uIManager, OnCoopBTNClicked);
        EquipmentBTN.Initialize(uIManager, OnEquipmentBTNClicked);
        HomeBTN.Initialize(uIManager, OnHomeBTNClicked);
        LevelsBTN.Initialize(uIManager, OnLevelsBTNClicked);
        LeaderBoardBTN.Initialize(uIManager, OnLeaderBoardBTNClicked);
    }

    private void OnCoopBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        AdjustMenuButtonScales();

        Content.DOLocalMoveX(2484f, MenuSwipeSpeed);
        
    }
    private void OnEquipmentBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click); 
        AdjustMenuButtonScales();
        Content.DOLocalMoveX(1242f, MenuSwipeSpeed);


    }
    private void OnHomeBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click); 
        AdjustMenuButtonScales();
        Content.DOLocalMoveX(0, MenuSwipeSpeed);


    }
    private void OnLevelsBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        AdjustMenuButtonScales();
        Content.DOLocalMoveX(-1242, MenuSwipeSpeed);


    }
    private void OnLeaderBoardBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click); 
        AdjustMenuButtonScales();
        Content.DOLocalMoveX(-2484f, MenuSwipeSpeed);
        

    }

    private void AdjustMenuButtonScales()
    {
        foreach (var btn in MenuButtonArray)
        {
            if(EventSystem.current.currentSelectedGameObject.name == btn.name)
            {
                btn.transform.DOScale(1.3f, ButtonAnimationSpeed);
            }
            else
            {
                if(btn.transform.localScale.x != 1)
                {
                    btn.transform.DOScale(1f, ButtonAnimationSpeed);

                }
            }
        }
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
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        GameManager.UIManager.GetPanel(Panels.MainMenu).OpenPanel();
        ClosePanel();
    }



}
