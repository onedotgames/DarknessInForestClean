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
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        Play.Initialize(uIManager, OnPlayButtonClicked);
        CoopBTN.Initialize(uIManager, OnCoopBTNClicked);
        EquipmentBTN.Initialize(uIManager, OnEquipmentBTNClicked);
        HomeBTN.Initialize(uIManager, OnHomeBTNClicked);
        LevelsBTN.Initialize(uIManager, OnLevelsBTNClicked);
        LeaderBoardBTN.Initialize(uIManager, OnLeaderBoardBTNClicked);

        Content.sizeDelta = new Vector2(Screen.height * 5, 0);
        MidGroup.sizeDelta = new Vector2(Screen.height * 5, Screen.width);
        SubEvents();
    }

    private void SubEvents()
    {
        if(GameManager != null)
        {
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
        }
    }

    private void OnCoopBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(Screen.height * 2, MenuSwipeSpeed);

        Coop.DOScale(Vector3.one * 2, 0.4f);
        Equipment.DOScale(Vector3.one, 0.4f);
        Home.DOScale(Vector3.one, 0.4f);
        Levels.DOScale(Vector3.one, 0.4f);
        LeaderBoard.DOScale(Vector3.one, 0.4f);

    }
    private void OnEquipmentBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click); 
        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(Screen.height, MenuSwipeSpeed);

        Equipment.DOScale(Vector3.one * 2, 0.4f);
        Coop.DOScale(Vector3.one, 0.4f);
        Home.DOScale(Vector3.one, 0.4f);
        Levels.DOScale(Vector3.one, 0.4f);
        LeaderBoard.DOScale(Vector3.one, 0.4f);
    }
    private void OnHomeBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click); 
        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(0, MenuSwipeSpeed);

        Home.DOScale(Vector3.one * 2, 0.4f);
        Equipment.DOScale(Vector3.one, 0.4f);
        Coop.DOScale(Vector3.one, 0.4f);
        Levels.DOScale(Vector3.one, 0.4f);
        LeaderBoard.DOScale(Vector3.one, 0.4f);

    }
    private void OnLevelsBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(-Screen.height, MenuSwipeSpeed);

        Levels.DOScale(Vector3.one * 2, 0.4f);
        Equipment.DOScale(Vector3.one, 0.4f);
        Home.DOScale(Vector3.one, 0.4f);
        Coop.DOScale(Vector3.one, 0.4f);
        LeaderBoard.DOScale(Vector3.one, 0.4f);
    }
    private void OnLeaderBoardBTNClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click); 
        //AdjustMenuButtonScales();
        Content.DOLocalMoveX(-Screen.height * 2, MenuSwipeSpeed);    

        LeaderBoard.DOScale(Vector3.one * 2, 0.4f);
        Equipment.DOScale(Vector3.one, 0.4f);
        Home.DOScale(Vector3.one, 0.4f);
        Levels.DOScale(Vector3.one, 0.4f);
        Coop.DOScale(Vector3.one, 0.4f);
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

    private void OnReturnToMainMenu()
    {
        OpenPanel();
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
        }
    }
}
