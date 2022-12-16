using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SelectSkill : UIPanel, IPointerDownHandler, IPointerUpHandler
{
    public CustomButton SkillButtonOne;
    public Image ButtonOneImage;
    public TMP_Text ButtonOneText;

    public CustomButton SkillButtonTwo;
    public TMP_Text ButtonTwoText;
    public Image ButtonTwoImage;

    public CustomButton SkillButtonThree;
    public TMP_Text ButtonThreeText;
    public Image ButtonThreeImage;

    public List<ButtonData> ButtonDataList;

    private HUD mHud;
    private WeaponManager mWeaponManager;

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        SubscribeEvents();
        mHud = uIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        mWeaponManager = uIManager.GameManager.WeaponManager;
    }

    public void AssignWeaponsToButtons()
    {
        mWeaponManager.CopyInitialWeaponList();

        for (int i = 0; i < ButtonDataList.Count; i++)
        {
            WeaponBase weapon = mWeaponManager.InUseWeaponList[Random.Range(0, mWeaponManager.InUseWeaponList.Count)];
            mWeaponManager.mWeaponList.Add(weapon);
            mWeaponManager.InUseWeaponList.Remove(weapon);

            ButtonDataList[i].Button.Initialize(GameManager.UIManager, mWeaponManager.InvokeWeapon, true);
            ButtonDataList[i].Image.sprite = weapon.SkillSO.Icon;
            ButtonDataList[i].Text.SetText(weapon.SkillSO.name);
            ButtonDataList[i].Weapon = weapon;
        }
    }

    private void SubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
        }
    }
    public override void OpenPanel()
    {
        base.OpenPanel();
        GameManager.UIManager.GetPanel(Panels.Hud).ClosePanel();
        UpdateUIElements();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
    }
    public void CloseSkillPanelAndOpenHud()
    {
        if (gameObject.activeInHierarchy)
        {
            ClosePanel();
        }
        mHud.OpenPanel();
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
    }
    #region Event Methods
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

    private void OnReturnToMainMenu()
    {
        ClosePanel();
    }
    

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
        }
    }

    #endregion
}

[System.Serializable]
public class ButtonData
{
    public CustomButton Button;
    public Image Image;
    public TMP_Text Text;
    public WeaponBase Weapon;
    public UtilityBase Utility;
}
