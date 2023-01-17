using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkillManager : CustomBehaviour
{
    [Space(10), Title("Weapon Bases")]
    public WeaponBaseV2 DefaultWeaponV2;
    public int ActiveWeaponIndex = 0;

    [Space(10), Title("All Skills")]
    public List<WeaponBaseV2> AllWeaponsV2;
    public List<UtilityBase> AllUtils;

    [HideInInspector, Title("Temporary Skill Lists")]
    public List<UtilityBase> TempUtils;
    public List<WeaponBaseV2> TempWeaponsV2;


    [Space(10), Title("Skills In Use")]
    public List<WeaponBaseV2> WeaponsInUseV2;
    public List<UtilityBase> UtilitiesInUse;

    [Space(10), Title("Booleans")]
    public bool IsMiniGameDone = false;
    public bool WeaponLimitReached = false;
    public bool UtilitiesLimitReached = false;

    [Space(10), Title("References")]
    public PausePanel PausePanel;
    public SelectSkill SelectSkillPanel;
    public GameObject[] Minigames;
    public ButtonData selectedWeaponData;
    public ButtonData selectedUtilityData;

    public List<WeaponBaseV2> WeaponBasesV2 = new List<WeaponBaseV2>();
    public List<UtilityBase> UtilityBases = new List<UtilityBase>();

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        AssignReferences();

    }
    private void AssignReferences()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame += OnLevelStart;
        }

        PausePanel = GameManager.UIManager.GetPanel(Panels.Pause).GetComponent<PausePanel>();
        SelectSkillPanel = GameManager.UIManager.GetPanel(Panels.SelectSkill).GetComponent<SelectSkill>();
    }
    public void ActivateDefaultWeapon()
    {
        AllWeaponsV2[1].Initialize(GameManager);
        WeaponsInUseV2.Add(AllWeaponsV2[1]);

        PausePanel.UpdateWeaponIcons(AllWeaponsV2[1].SkillSO.Icon);
        AllWeaponsV2[1].gameObject.SetActive(true);
    }
    public void ActivateWeapon(int index)
    {
        if (AllWeaponsV2[index].IsActivated)
        {
            AllWeaponsV2[index].UpdateWeapon();
        }
        else
        {
            AllWeaponsV2[index].Initialize(GameManager);
            AllWeaponsV2[index].gameObject.SetActive(true);
        }
        
    }

    public void CheckWeaponLimitReached()
    {

        if (WeaponsInUseV2.Count == PausePanel.WeaponIconsOnPause.Count)
        {
            WeaponLimitReached = true;
        }
    }

    public void CheckUtilLimitReached()
    {
        if (UtilitiesInUse.Count == PausePanel.UtilIconsOnPause.Count)
        {
            UtilitiesLimitReached = true;
        }
    }

    public void CreateTempWeaponList()
    {
        if (!WeaponLimitReached)
        {
            TempWeaponsV2.Clear();
            for (int i = 0; i < AllWeaponsV2.Count; i++)
            {
                TempWeaponsV2.Add(AllWeaponsV2[i]);
            }
        }
        else
        {
            TempWeaponsV2.Clear();
            for (int i = 0; i < WeaponsInUseV2.Count; i++)
            {

                TempWeaponsV2.Add(WeaponsInUseV2[i]);
            }
        }

    }

    public void CreateTempUtilList()
    {
        if (!UtilitiesLimitReached)
        {
            TempUtils.Clear();
            for (int i = 0; i < AllUtils.Count; i++)
            {
                TempUtils.Add(AllUtils[i]);
            }
        }
        else
        {
            TempUtils.Clear();
            for (int i = 0; i < UtilitiesInUse.Count; i++)
            {
                TempUtils.Add(UtilitiesInUse[i]);
            }
        }

    }

    public void InvokeWeaponV2()
    {
        if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[0].Button.gameObject)
        {
            selectedWeaponData = SelectSkillPanel.ButtonDataList[0]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            bool contains = false;

            foreach (var item in PausePanel.WeaponIconsOnPause)
            {
                if (item.sprite.name == selectedWeaponData.Image.sprite.name)
                {
                    contains = true;
                }
            }

            if (contains)
            {
                //Increase star level
            }
            else
            {
                PausePanel.UpdateWeaponIcons(selectedWeaponData.Image.sprite);
                WeaponsInUseV2.Add(selectedWeaponData.Weapon);
                CheckWeaponLimitReached();
            }

            //LevelUpWeapon();
        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[1].Button.gameObject)
        {
            selectedWeaponData = SelectSkillPanel.ButtonDataList[1]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            bool contains = false;

            foreach (var item in PausePanel.WeaponIconsOnPause)
            {
                if (item.sprite.name == selectedWeaponData.Image.sprite.name)
                {
                    contains = true;
                }
            }

            if (contains)
            {

            }
            else
            {
                PausePanel.UpdateWeaponIcons(selectedWeaponData.Image.sprite);
                WeaponsInUseV2.Add(selectedWeaponData.Weapon);
                CheckWeaponLimitReached();

            }

            //LevelUpWeapon();
        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[2].Button.gameObject)
        {
            selectedWeaponData = SelectSkillPanel.ButtonDataList[2]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            bool contains = false;

            foreach (var item in PausePanel.WeaponIconsOnPause)
            {
                if (item.sprite.name == selectedWeaponData.Image.sprite.name)
                {
                    contains = true;
                }
            }

            if (contains)
            {

            }
            else
            {
                PausePanel.UpdateWeaponIcons(selectedWeaponData.Image.sprite);
                WeaponsInUseV2.Add(selectedWeaponData.Weapon);
                CheckWeaponLimitReached();

            }

            //LevelUpWeapon();
        }

        ActivateWeapon((int)selectedWeaponData.Weapon.SkillSO.PoolerType);
        //SelectSkillPanel.CloseSkillPanelAndOpenHud();

    }
    public void InvokeUtility()
    {
        if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[0].Button.gameObject)
        {
            selectedUtilityData = SelectSkillPanel.ButtonDataList[0]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            selectedUtilityData.Utility.Initialize(GameManager);
            selectedUtilityData.Utility.MakeUpgrade();

            bool contains = false;

            foreach (var item in PausePanel.UtilIconsOnPause)
            {
                if (item.sprite.name == selectedUtilityData.Image.sprite.name)
                {
                    contains = true;
                }
            }

            if (contains)
            {

            }
            else
            {
                PausePanel.UpdateUtilIcons(selectedUtilityData.Image.sprite);
                UtilitiesInUse.Add(selectedUtilityData.Utility);
                CheckUtilLimitReached();
            }

        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[1].Button.gameObject)
        {
            selectedUtilityData = SelectSkillPanel.ButtonDataList[1]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            selectedUtilityData.Utility.Initialize(GameManager);
            selectedUtilityData.Utility.MakeUpgrade();

            bool contains = false;

            foreach (var item in PausePanel.UtilIconsOnPause)
            {
                if (item.sprite.name == selectedUtilityData.Image.sprite.name)
                {
                    contains = true;
                }
            }

            if (contains)
            {

            }
            else
            {
                PausePanel.UpdateUtilIcons(selectedUtilityData.Image.sprite);
                UtilitiesInUse.Add(selectedUtilityData.Utility);
                CheckUtilLimitReached();
            }
        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[2].Button.gameObject)
        {
            selectedUtilityData = SelectSkillPanel.ButtonDataList[2]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            selectedUtilityData.Utility.Initialize(GameManager);
            selectedUtilityData.Utility.MakeUpgrade();

            bool contains = false;

            foreach (var item in PausePanel.UtilIconsOnPause)
            {
                if (item.sprite.name == selectedUtilityData.Image.sprite.name)
                {
                    contains = true;
                }
            }

            if (contains)
            {

            }
            else
            {
                PausePanel.UpdateUtilIcons(selectedUtilityData.Image.sprite);
                UtilitiesInUse.Add(selectedUtilityData.Utility);
                CheckUtilLimitReached();
            }
        }

        SelectSkillPanel.CloseSkillPanelAndOpenHud();
    }

    private void OnLevelStart()
    {
        WeaponsInUseV2.ForEach(x => x.gameObject.SetActive(false));
        WeaponsInUseV2.Clear();
        ActivateDefaultWeapon();
    }

    private void OnDisable()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= OnLevelStart;
        }
    }
}
