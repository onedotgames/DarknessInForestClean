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
    public WeaponBase DefaultWeapon;
    public WeaponBaseV2 DefaultWeaponV2;
    public WeaponBase BeeShot;
    public WeaponBase BirdBomb;
    public WeaponBase CHammer;
    public WeaponBase Whip;
    public int ActiveWeaponIndex = 0;

    [Space(10), Title("Target Weapon Cooldowns")]
    public float TargetWeaponCooldown1;
    public float TargetWeaponCooldown2;
    public float TargetWeaponCooldown3;
    public float TargetWeaponCooldown4;
    public float TargetWeaponCooldown5;
    private float timeValue;

    [Space(10), Title("Target Weapon Cooldowns")]
    public float CurrentWeaponCooldown1;
    public float CurrentWeaponCooldown2;
    public float CurrentWeaponCooldown3;
    public float CurrentWeaponCooldown4;
    public float CurrentWeaponCooldown5;


    [Space(10), Title("All Skills")]
    public List<WeaponBase> AllWeapons;
    public List<WeaponBaseV2> AllWeaponsV2;
    public List<UtilityBase> AllUtils;

    [HideInInspector, Title("Temporary Skill Lists")]
    public List<UtilityBase> TempUtils;
    public List<WeaponBase> TempWeapons;
    public List<WeaponBaseV2> TempWeaponsV2;


    [Space(10), Title("Skills In Use")]
    public List<WeaponBase> WeaponsInUse;
    public List<WeaponBaseV2> WeaponsInUseV2;
    public List<UtilityBase> UtilitiesInUse;

    [Space(10), Title("Booleans")]
    public bool IsMiniGameDone = false;
    public bool WeaponLimitReached = false;
    public bool UtilitiesLimitReached = false;
    public bool Weapon1Active = false;
    public bool Weapon2Active = false;
    public bool Weapon3Active = false;
    public bool Weapon4Active = false;
    public bool Weapon5Active = false;

    [Space(10), Title("References")]
    public PausePanel PausePanel;
    public SelectSkill SelectSkillPanel;
    public GameObject[] Minigames;
    public ButtonData selectedWeaponData;
    public ButtonData selectedUtilityData;

    public List<WeaponBase> WeaponBases = new List<WeaponBase>();
    public List<WeaponBaseV2> WeaponBasesV2 = new List<WeaponBaseV2>();
    public List<UtilityBase> UtilityBases = new List<UtilityBase>();

    [ReadOnly,Title("Attack Methods")]
    public delegate void AttackDelegate();
    protected AttackDelegate FireWeapon1;
    protected AttackDelegate FireWeapon2;
    protected AttackDelegate FireWeapon3;
    protected AttackDelegate FireWeapon4;
    protected AttackDelegate FireWeapon5;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        AssignReferences();
    }
    private void AssignReferences()
    {
        //mPlayer = GameManager.PlayerManager.CurrentPlayer;
        //HUD = GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        PausePanel = GameManager.UIManager.GetPanel(Panels.Pause).GetComponent<PausePanel>();
        SelectSkillPanel = GameManager.UIManager.GetPanel(Panels.SelectSkill).GetComponent<SelectSkill>();
    }
    public void ActivateDefaultWeapon()
    {
        AllWeaponsV2[1].Initialize(GameManager);
        AllWeaponsV2[1].gameObject.SetActive(true);
    }
    public void ActivateWeapon(int index)
    {
        if (AllWeaponsV2[index].IsActivated)
        {
            Debug.Log("UPGRADE");
            AllWeaponsV2[index].UpdateWeapon();
        }
        {
            AllWeaponsV2[index].Initialize(GameManager);
            AllWeaponsV2[index].gameObject.SetActive(true);
        }
        
    }

    //private void Update()
    //{
    //    if(GameManager.IsGameStarted && !GameManager.IsGamePaused && !GameManager.IsBossTime)
    //    {
    //        if (Weapon1Active)
    //        {
    //            CurrentWeaponCooldown1 += Time.deltaTime;
    //            if (CurrentWeaponCooldown1 >= TargetWeaponCooldown1)
    //            {
    //                CurrentWeaponCooldown1 = 0;
    //                if(FireWeapon1 != null)
    //                {
    //                    FireWeapon1();
    //                }
    //            }
    //        }

    //        if (Weapon2Active)
    //        {
    //            CurrentWeaponCooldown2 += Time.deltaTime;
    //            if (CurrentWeaponCooldown2 >= TargetWeaponCooldown1)
    //            {
    //                CurrentWeaponCooldown2 = 0;
    //                if (FireWeapon2 != null)
    //                {
    //                    FireWeapon2();
    //                }
    //            }
    //        }

    //        if (Weapon3Active)
    //        {
    //            CurrentWeaponCooldown3 += Time.deltaTime;
    //            if (CurrentWeaponCooldown3 >= TargetWeaponCooldown1)
    //            {
    //                CurrentWeaponCooldown3 = 0;
    //                if (FireWeapon3 != null)
    //                {
    //                    FireWeapon3();
    //                }
    //            }
    //        }

    //        if (Weapon4Active)
    //        {
    //            CurrentWeaponCooldown4 += Time.deltaTime;
    //            if (CurrentWeaponCooldown4 >= TargetWeaponCooldown1)
    //            {
    //                CurrentWeaponCooldown4 = 0;
    //                if (FireWeapon4 != null)
    //                {
    //                    FireWeapon4();
    //                }
    //            }
    //        }
            
    //        if (Weapon5Active)
    //        {
    //            CurrentWeaponCooldown5 += Time.deltaTime;
    //            if (CurrentWeaponCooldown5 >= TargetWeaponCooldown1)
    //            {
    //                CurrentWeaponCooldown5 = 0;
    //                if (FireWeapon5 != null)
    //                {
    //                    FireWeapon5();
    //                }
    //            }
    //        }
    //    }
        
    //}

    private void WhipAtc()
    {
        var timer = 0f;
        var pooler = GameManager.PoolingManager.WeaponPoolerListV2[(int)DamagePattern.Whip];
        var obj = pooler.GetFromPool();
        obj.objectTransform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
        obj.gameObject.SetActive(true);
        var weapon = obj.GetComponent<WeaponBase>();
        weapon.SetStats();
        obj.GetComponent<IvyWhip>().WhipAttack();
        timer += Time.deltaTime;
        if(timer >= 0.25f)
        {
            timer = 0f;
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
            //if (!DefaultWeapon.IsEvolved)
            //{
            //    TempWeaponsV2.Add(AllWeaponsV2[0]);
            //}
        }
        else
        {
            TempWeaponsV2.Clear();
            for (int i = 0; i < WeaponsInUseV2.Count; i++)
            {

                TempWeaponsV2.Add(WeaponsInUseV2[i]);
            }
            if (!DefaultWeapon.IsEvolved)
            {
                TempWeaponsV2.Add(AllWeaponsV2[0]);
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

    private void AssignSlots(DamagePattern damagePattern)
    {
        switch (damagePattern)
        {
            case DamagePattern.Whip:
                switch (ActiveWeaponIndex)
                {
                    case 0:
                        FireWeapon1 = WhipAtc;
                        break;
                    case 1:
                        FireWeapon2 = WhipAtc;
                        break;
                    case 2:
                        FireWeapon3 = WhipAtc;
                        break;
                    case 3:
                        FireWeapon4 = WhipAtc;
                        break;
                    case 4:
                        FireWeapon5 = WhipAtc;
                        break;
                }
                break;
        }
    }

    //public void InvokeWeapon()
    //{
    //    if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[0].Button.gameObject)
    //    {
    //        selectedWeaponData = SelectSkillPanel.ButtonDataList[0]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

    //        bool contains = false;

    //        foreach (var item in PausePanel.WeaponIconsOnPause)
    //        {
    //            if (item.sprite.name == selectedWeaponData.Image.sprite.name)
    //            {
    //                contains = true;
    //            }
    //        }

    //        if (contains)
    //        {
    //            //Increase star level
    //        }
    //        else
    //        {
    //            PausePanel.UpdateWeaponIcons(selectedWeaponData.Image.sprite);
    //            WeaponsInUse.Add(selectedWeaponData.Weapon);
    //            CheckWeaponLimitReached();
    //        }

    //        //LevelUpWeapon();
    //    }
    //    else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[1].Button.gameObject)
    //    {
    //        selectedWeaponData = SelectSkillPanel.ButtonDataList[1]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

    //        bool contains = false;

    //        foreach (var item in PausePanel.WeaponIconsOnPause)
    //        {
    //            if (item.sprite.name == selectedWeaponData.Image.sprite.name)
    //            {
    //                contains = true;
    //            }
    //        }

    //        if (contains)
    //        {

    //        }
    //        else
    //        {
    //            PausePanel.UpdateWeaponIcons(selectedWeaponData.Image.sprite);
    //            WeaponsInUse.Add(selectedWeaponData.Weapon);
    //            CheckWeaponLimitReached();

    //        }

    //        //LevelUpWeapon();
    //    }
    //    else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[2].Button.gameObject)
    //    {
    //        selectedWeaponData = SelectSkillPanel.ButtonDataList[2]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

    //        bool contains = false;

    //        foreach (var item in PausePanel.WeaponIconsOnPause)
    //        {
    //            if (item.sprite.name == selectedWeaponData.Image.sprite.name)
    //            {
    //                contains = true;
    //            }
    //        }

    //        if (contains)
    //        {

    //        }
    //        else
    //        {
    //            PausePanel.UpdateWeaponIcons(selectedWeaponData.Image.sprite);
    //            WeaponsInUse.Add(selectedWeaponData.Weapon);
    //            CheckWeaponLimitReached();

    //        }

    //        //LevelUpWeapon();
    //    }       
    //}

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
        SelectSkillPanel.CloseSkillPanelAndOpenHud();

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
            }
        }

        SelectSkillPanel.CloseSkillPanelAndOpenHud();
    }
    private void OnLevelStart()
    {
        AssignSlots(DefaultWeapon.SkillSO.DamagePattern);
    }
}
