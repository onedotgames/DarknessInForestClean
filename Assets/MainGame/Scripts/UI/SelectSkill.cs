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
    public void AssignSkillSelectingButtons()
    {
        mWeaponManager.WeaponBases.Clear();
        mWeaponManager.UtilityBases.Clear();

        mWeaponManager.CheckWeaponLimitReached();
        mWeaponManager.CreateTempWeaponList();
        mWeaponManager.CreateTempUtilList();

        for (int i = 0; i < ButtonDataList.Count; i++)
        {
            if (GameManager.PlayerLevelManager.PlayerLevel <= 5)
            {
                AssignWeapon(i);
            }
            else
            {
                //Pick weapon or Util
                int j = Random.Range(0, 2);


                if (j == 0)
                {
                    //Weapon
                    if (i < mWeaponManager.TempWeaponList.Count)
                    {
                        AssignWeapon(i);
                    }
                    else
                    {
                        if (i < mWeaponManager.TempUtilList.Count)
                        {
                            AssignUtil(i);
                        }
                        else
                        {
                            Debug.Log("Nothing to assign");
                        }
                    }
                }
                else if (j == 1)
                {
                    //Util
                    if(i < mWeaponManager.TempUtilList.Count)
                    {
                        AssignUtil(i);
                    }
                    else
                    {
                        if (i < mWeaponManager.TempWeaponList.Count)
                        {
                            AssignWeapon(i);
                        }
                        else
                        {
                            Debug.Log("Nothing to assign");
                        }
                    }
                }

            }

        }
    }

    public void AssignWeapon(int index)
    {
        WeaponBase weapon = mWeaponManager.TempWeaponList[Random.Range(0, mWeaponManager.TempWeaponList.Count)];
        mWeaponManager.WeaponBases.Add(weapon);
        mWeaponManager.TempWeaponList.Remove(weapon);

        ButtonDataList[index].Button.Initialize(GameManager.UIManager, mWeaponManager.InvokeWeapon, true);
        ButtonDataList[index].Image.sprite = weapon.SkillSO.Icon;
        ButtonDataList[index].Text.SetText(weapon.SkillSO.name);
        ButtonDataList[index].Weapon = weapon;
    }

    public void AssignUtil(int index)
    {
        UtilityBase util = mWeaponManager.TempUtilList[Random.Range(0, mWeaponManager.TempUtilList.Count)];
        mWeaponManager.UtilityBases.Add(util);
        mWeaponManager.TempUtilList.Remove(util);

        ButtonDataList[index].Button.Initialize(GameManager.UIManager, mWeaponManager.InvokeUtility, true);
        ButtonDataList[index].Image.sprite = util.UtilitySO.Icon;
        ButtonDataList[index].Text.SetText(util.UtilitySO.name);
        ButtonDataList[index].Utility = util;
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
        GameManager.PlayerLevelManager.CheckExp();
        mHud.SetExpBarFillAmount();

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
