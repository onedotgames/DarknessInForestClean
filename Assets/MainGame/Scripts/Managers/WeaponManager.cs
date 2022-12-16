using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class WeaponManager : CustomBehaviour
{
    //Kullan�labilecek t�m weaponlar. Sabit liste
    public List<WeaponBase> InitialWeaponList;
    public List<UtilityBase> InitialUtilityList;
    public bool isMiniGameDone = false;
    //Initial liste klonu. De�i�iklik yap�lacak olan liste
    public List<WeaponBase> InUseWeaponList;
    public List<UtilityBase> InUseUtilityList;

    #region WeaponBases
    public List<WeaponBase> ProjectileWeaponsInUse;
    public List<WeaponBase> YoyoWeaponsInUse;
    public List<WeaponBase> AreaWeaponsInUse;
    public List<WeaponBase> ShotgunWeaponsInUse;
    public List<WeaponBase> WhipWeaponsInUse;
    public List<WeaponBase> BombWeaponsInUse;
    public List<WeaponBase> BounceWeaponsInUse;
    public List<WeaponBase> SkunkGasWeaponsInUse;
    public List<WeaponBase> BananaWeaponsInUse;
    #endregion

    public List<ChestnutHammer> ActiveChestnuts;
    public List<BeeShot> ActiveBeeShots;

    public int BeeIndex = 0;


    [SerializeField] WeaponBase mDefaultWeapon;
    [SerializeField] WeaponBase mAreaWeapon;

    private Player mPlayer;
    public HUD HUD;
    public SelectSkill SelectSkillPanel;
    public List<WeaponBase> mWeaponList = new List<WeaponBase>();
    private List<UtilityBase> utilityBases = new List<UtilityBase>();

    public ButtonData selectedWeaponData;
    public ButtonData selectedUtilityData;
    public List<IEnumerator> CoroutineList = new List<IEnumerator>();

    [Title("Shotgun"), GUIColor(0.8f, 0.4f, 0.4f)]
    public List<GameObject> ShotgunAmmo = new List<GameObject>();
    private float AttackDegree = 30;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        mPlayer = gameManager.PlayerManager.CurrentPlayer;
        HUD = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        SelectSkillPanel = gameManager.UIManager.GetPanel(Panels.SelectSkill).GetComponent<SelectSkill>();

        if (GameManager != null)
        {
            GameManager.OnStartGame += OnLevelStart;
            GameManager.OnLevelFailed += OnLevelFailed;
        }
    }

    public void AssignUtilsToButtons()
    {
        CopyInitialUtilList();
        for (int i = 0; i < SelectSkillPanel.ButtonDataList.Count; i++)
        {
            UtilityBase util = InUseUtilityList[Random.Range(0, InUseUtilityList.Count)];
            utilityBases.Add(util);
            InUseUtilityList.Remove(util);

            SelectSkillPanel.ButtonDataList[i].Button.Initialize(GameManager.UIManager, InvokeUtility, true);
            SelectSkillPanel.ButtonDataList[i].Image.sprite = util.UtilitySO.Icon;
            SelectSkillPanel.ButtonDataList[i].Text.SetText(util.UtilitySO.name);
            SelectSkillPanel.ButtonDataList[i].Utility = util;
        }
    }

    public void CopyInitialWeaponList()
    {
        InUseWeaponList.Clear();
        for (int i = 0; i < InitialWeaponList.Count; i++)
        {

            InUseWeaponList.Add(InitialWeaponList[i]);
        }
        InUseWeaponList.Add(mDefaultWeapon);
    }

    private void CopyInitialUtilList()
    {
        InUseUtilityList.Clear();
        for (int i = 0; i < InitialUtilityList.Count; i++)
        {

            InUseUtilityList.Add(InitialUtilityList[i]);
        }
    }

    private void InvokeDefaultWeapon()
    {
        switch (mDefaultWeapon.SkillSO.DamagePattern)
        {
            case DamagePattern.Whip:
                if (mDefaultWeapon.IsInitialized == false)
                {
                    mDefaultWeapon.IsInitialized = true;
                    mDefaultWeapon.Initialize(GameManager);
                    StartCoroutine(WhipWeaponRoutine(mDefaultWeapon));
                }
                break;
        }
            
    }
    
    public void InvokeUtility()
    {
        if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[0].Button.gameObject)
        {
            selectedUtilityData = SelectSkillPanel.ButtonDataList[0]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            selectedUtilityData.Utility.Initialize(GameManager);
            selectedUtilityData.Utility.MakeUpgrade();

            bool contains = false;

            foreach (var item in GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIconsOnPause)
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
                GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIconsOnPause
                [GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIndex].sprite = selectedUtilityData.Image.sprite;
                GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIndex++;
            }

        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[1].Button.gameObject)
        {
            selectedUtilityData = SelectSkillPanel.ButtonDataList[1]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            selectedUtilityData.Utility.Initialize(GameManager);
            selectedUtilityData.Utility.MakeUpgrade();

            bool contains = false;

            foreach (var item in GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIconsOnPause)
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
                GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIconsOnPause
                [GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIndex].sprite = selectedUtilityData.Image.sprite;
                GameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>().UtilIndex++;
            }
        }

        SelectSkillPanel.CloseSkillPanelAndOpenHud();
    }

    public void InvokeWeapon()
    {
        if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[0].Button.gameObject)
        {
            selectedWeaponData = SelectSkillPanel.ButtonDataList[0]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            bool contains = false;

            foreach (var item in HUD.WeaponIconsOnPause)
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
                HUD.WeaponIconsOnPause[HUD.WeaponIndex].sprite = selectedWeaponData.Image.sprite;
                HUD.WeaponIndex++;
            }

            LevelUpWeapon();
        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[1].Button.gameObject)
        {
            selectedWeaponData = SelectSkillPanel.ButtonDataList[1]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            bool contains = false;

            foreach (var item in HUD.WeaponIconsOnPause)
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
                HUD.WeaponIconsOnPause[HUD.WeaponIndex].sprite = selectedWeaponData.Image.sprite;
                HUD.WeaponIndex++;
            }

            LevelUpWeapon();
        }
        else if (EventSystem.current.currentSelectedGameObject == SelectSkillPanel.ButtonDataList[2].Button.gameObject)
        {
            selectedWeaponData = SelectSkillPanel.ButtonDataList[2]; //Selected data daha sonra referens olarak kullan�lmak �zere belirlenir

            bool contains = false;

            foreach (var item in HUD.WeaponIconsOnPause)
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
                HUD.WeaponIconsOnPause[HUD.WeaponIndex].sprite = selectedWeaponData.Image.sprite;
                HUD.WeaponIndex++;
            }

            LevelUpWeapon();
        }

        for (int i = 0; i < ProjectileWeaponsInUse.Count; i++)
        {
            if (ProjectileWeaponsInUse[i].IsInitialized == false)
            {
                ProjectileWeaponsInUse[i].IsInitialized = true;
                ProjectileWeaponsInUse[i].Initialize(GameManager);

                CoroutineList.Add(WeaponSlotRoutine(ProjectileWeaponsInUse[i]));
            }

        }
        for (int i = 0; i < BounceWeaponsInUse.Count; i++)
        {
            if (BounceWeaponsInUse[i].IsInitialized == false)
            {
                BounceWeaponsInUse[i].IsInitialized = true;
                BounceWeaponsInUse[i].Initialize(GameManager);

                StartCoroutine(BeeSlotRoutine(BounceWeaponsInUse[i]));
            }

        }
        for (int i = 0; i < YoyoWeaponsInUse.Count; i++)
        {
            if (YoyoWeaponsInUse[i].IsInitialized == false)
            {
                YoyoWeaponsInUse[i].IsInitialized = true;
                YoyoWeaponsInUse[i].Initialize(GameManager);

                YoyoWeaponSlotRoutine(YoyoWeaponsInUse[i]);
            }
        }
        for (int i = 0; i < ShotgunWeaponsInUse.Count; i++)
        {
            if (ShotgunWeaponsInUse[i].IsInitialized == false)
            {
                ShotgunWeaponsInUse[i].IsInitialized = true;
                ShotgunWeaponsInUse[i].Initialize(GameManager);

                CoroutineList.Add(ShotgunSlotRoutine(ShotgunWeaponsInUse[i]));
                //StartCoroutine(CoroutineList[i]);
            }
        }
        for (int i = 0; i < WhipWeaponsInUse.Count; i++)
        {
            if (WhipWeaponsInUse[i].IsInitialized == false)
            {
                WhipWeaponsInUse[i].IsInitialized = true;
                WhipWeaponsInUse[i].Initialize(GameManager);
                StartCoroutine(WhipWeaponRoutine(WhipWeaponsInUse[i]));
            }
        }
        for (int i = 0; i < BombWeaponsInUse.Count; i++)
        {
            if (BombWeaponsInUse[i].IsInitialized == false)
            {
                BombWeaponsInUse[i].IsInitialized = true;
                BombWeaponsInUse[i].Initialize(GameManager);
                StartCoroutine(BombWeaponRoutine(BombWeaponsInUse[i]));
            }
        }

        for (int i = 0; i < SkunkGasWeaponsInUse.Count; i++)
        {
            if(SkunkGasWeaponsInUse[i].IsInitialized == false)
            {
                SkunkGasWeaponsInUse[i].IsInitialized = true;
                SkunkGasWeaponsInUse[i].Initialize(GameManager);
                StartCoroutine(SkunkGasWeaponRoutine(SkunkGasWeaponsInUse[i]));
            }
        }

        for(int i = 0; i< BananaWeaponsInUse.Count; i++)
        {
            if(BananaWeaponsInUse[i].IsInitialized == false)
            {
                BananaWeaponsInUse[i].IsInitialized = true;
                BananaWeaponsInUse[i].Initialize(GameManager);
                StartCoroutine(BananaWeaponRoutine(BananaWeaponsInUse[i]));
            }
        }
        for (int i = 0; i < CoroutineList.Count; i++)
        {
            StartCoroutine(CoroutineList[i]);
        }
    }
    
    private void LevelUpWeapon()
    {
        switch (selectedWeaponData.Weapon.SkillSO.DamagePattern)
        {
            case DamagePattern.Projectile:

                if (ProjectileWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    ProjectileWeaponsInUse.Remove(selectedWeaponData.Weapon);

                    if (selectedWeaponData.Weapon.UpgradeLevel == 1)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));

                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }

                    ProjectileWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    ProjectileWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();
                }
                break;

            case DamagePattern.Area:
                break;

            case DamagePattern.Shotgun:
                if (ShotgunWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    ShotgunWeaponsInUse.Remove(selectedWeaponData.Weapon);
                    if (selectedWeaponData.Weapon.UpgradeLevel == 1)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));
                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }

                    ShotgunWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    ShotgunWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    
                }
                break;

            case DamagePattern.Yoyo:
                if (YoyoWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    YoyoWeaponsInUse.Remove(selectedWeaponData.Weapon);
                    if (/*selectedWeaponData.Weapon.UpgradeLevel == 1*/
                        selectedWeaponData.Weapon.SkillSO.UpgradeDatas[selectedWeaponData.Weapon.UpgradeLevel].PropertyToChange
                        == PropertyToChange.Evolve)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));
                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }

                    YoyoWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    YoyoWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();

                }
                break;

            case DamagePattern.Whip:
                if (WhipWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    WhipWeaponsInUse.Remove(selectedWeaponData.Weapon);
                    if (selectedWeaponData.Weapon.SkillSO.UpgradeDatas[selectedWeaponData.Weapon.UpgradeLevel].PropertyToChange == PropertyToChange.Evolve)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));

                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }

                    WhipWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    WhipWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();
                }
                break;

            case DamagePattern.Bomb:
                if (BombWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    BombWeaponsInUse.Remove(selectedWeaponData.Weapon);
                    if (selectedWeaponData.Weapon.SkillSO.UpgradeDatas[selectedWeaponData.Weapon.UpgradeLevel].PropertyToChange == PropertyToChange.Evolve)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));

                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }

                    BombWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    BombWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();
                }
                break;

            case DamagePattern.Bounce:
                if (BounceWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    BounceWeaponsInUse.Remove(selectedWeaponData.Weapon);

                    if (selectedWeaponData.Weapon.SkillSO.UpgradeDatas[selectedWeaponData.Weapon.UpgradeLevel].PropertyToChange == PropertyToChange.Evolve)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));

                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }


                    BounceWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    BounceWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();

                }
                break;

            case DamagePattern.SkunkGas:
                if (SkunkGasWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    SkunkGasWeaponsInUse.Remove(selectedWeaponData.Weapon);

                    if (selectedWeaponData.Weapon.SkillSO.UpgradeDatas[selectedWeaponData.Weapon.UpgradeLevel].PropertyToChange == PropertyToChange.Evolve)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));

                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                    }

                    SkunkGasWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    SkunkGasWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();
                }
                break;

            case DamagePattern.BananaGuardian:
                if (BananaWeaponsInUse.Contains(selectedWeaponData.Weapon))
                {
                    BananaWeaponsInUse.Remove(selectedWeaponData.Weapon);
                    if (selectedWeaponData.Weapon.SkillSO.UpgradeDatas[selectedWeaponData.Weapon.UpgradeLevel].PropertyToChange == PropertyToChange.Evolve)
                    {
                        selectedWeaponData.Weapon.PlayMinigame(Random.Range(0, selectedWeaponData.Weapon.minigames.Length));

                    }
                    else
                    {
                        selectedWeaponData.Weapon.UpdateWeapon();
                        SelectSkillPanel.CloseSkillPanelAndOpenHud();
                        //GameManager.PoolingManager.WeaponPooler[(int)selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                    }

                    BananaWeaponsInUse.Add(selectedWeaponData.Weapon);
                }
                else
                {
                    BananaWeaponsInUse.Add(selectedWeaponData.Weapon);
                    SelectSkillPanel.CloseSkillPanelAndOpenHud();
                }
                break;

        }
    }
        private IEnumerator WeaponSlotRoutine(WeaponBase weapon) //Potionda Size artt�rma burada olacak.
        {
            if (GetClosestEnemy(GameManager.AIManager.EnemyList, weapon) != null)
            {
                for (int i = 0; i < weapon.Count; i++)
                {
                    yield return new WaitForSeconds(0.25f);
                    var obj = weapon.PoolerBase.GetObjectFromPool();

                    obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
                    obj.GetComponent<WeaponBase>().SetStats();
                    obj.GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
                    GameManager.AIManager.EnemyList.Remove(obj.GetComponent<WeaponBase>().Target);
                }
            }
            yield return new WaitForSeconds(weapon.Cooldown);
            StartCoroutine(WeaponSlotRoutine(weapon));
        }
        private IEnumerator BeeSlotRoutine(WeaponBase weapon) //Potionda Size artt�rma burada olacak.
        {
            if (ActiveBeeShots != null)
            {
                ActiveBeeShots.Clear();
                BeeIndex = 0;
            }
            for (int i = 0; i < weapon.Count; i++)
            {
                var obj = weapon.PoolerBase.GetObjectFromPool();
                ActiveBeeShots.Add(obj.GetComponent<BeeShot>());
                obj.GetComponent<BeeShot>().index = BeeIndex;
                BeeIndex++;
                obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
                obj.GetComponent<WeaponBase>().SetStats();
                obj.GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
                yield return new WaitForSeconds(0.25f);

            }
            yield return new WaitForSeconds(weapon.transform.GetComponent<BeeShot>().Duration);
            ActiveBeeShots.ForEach(x => x.PoolerBase.ReturnObjectToPool(x.gameObject));
            yield return new WaitForSeconds(weapon.Cooldown);
            StartCoroutine(BeeSlotRoutine(weapon));
        }

        public void YoyoWeaponSlotRoutine(WeaponBase weapon)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                if (ActiveChestnuts != null)
                {
                    ActiveChestnuts.Clear();
                }
                var obj = weapon.PoolerBase.GetObjectFromPool();
                ActiveChestnuts.Add(obj.GetComponent<ChestnutHammer>());//YOYO SADECE CHESTNUT DE��LSE BU SATIRI DE���T�R
                obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
                obj.GetComponent<WeaponBase>().SetStats();
                obj.GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
                GameManager.AIManager.EnemyList.Remove(obj.GetComponent<WeaponBase>().Target);
            }
        }

        public IEnumerator WhipWeaponRoutine(WeaponBase weapon)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                var obj = weapon.PoolerBase.GetObjectFromPool();
                obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
                obj.GetComponent<WeaponBase>().SetStats();
                //obj.GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
                obj.GetComponent<IvyWhip>().WhipAttack();
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(weapon.Cooldown);
            StartCoroutine(WhipWeaponRoutine(weapon));
        }

    public IEnumerator SkunkGasWeaponRoutine(WeaponBase weapon)
    {
        for (int i = 0; i < weapon.Count; i++)
        {
            var obj = weapon.PoolerBase.GetObjectFromPool();
            obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
            obj.GetComponent<WeaponBase>().SetStats();
            obj.GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
        }
        yield return new WaitForSeconds(weapon.Cooldown);
    }
    public IEnumerator BananaWeaponRoutine(WeaponBase weapon)
    {
        weapon.gameObject.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
        weapon.gameObject.SetActive(true);
        weapon.SetStats();
        weapon.SetSkill(GameManager.AIManager.EnemyList);
        yield return new WaitForSeconds(3); //Duration olcak he;
        weapon.gameObject.SetActive(false);
        yield return new WaitForSeconds(weapon.Cooldown);
        StartCoroutine(BananaWeaponRoutine(weapon));
    }

        public IEnumerator BombWeaponRoutine(WeaponBase weapon)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                var obj = weapon.PoolerBase.GetObjectFromPool();
                //obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
                obj.GetComponent<WeaponBase>().SetStats();
                //obj.GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
                yield return new WaitForSeconds(0.25f);
                //obj.GetComponent<WeaponBase>().SetStats();
                obj.GetComponent<BirdBomb>().SetSkill(GameManager.AIManager.EnemyList);
                obj.GetComponent<BirdBomb>().Drop();
                obj.GetComponent<BirdBomb>().DestroyBomb();
            }
            yield return new WaitForSeconds(weapon.Cooldown);
            StartCoroutine(BombWeaponRoutine(weapon));
        }

        private IEnumerator ShotgunSlotRoutine(WeaponBase weapon)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                var obj = weapon.PoolerBase.GetObjectFromPool();
                ShotgunAmmo.Add(obj);
                obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
            }

            float spaceBetweenProjectiles = AttackDegree / (ShotgunAmmo.Count - 1);
            int index = 0;

            var count = ShotgunAmmo.Count;
            for (int i = 0; i < count; i++)
            {
                float horizontal = GameManager.JoystickManager.GetHorizontal();
                float vertical = GameManager.JoystickManager.GetVertical();

                float initialRot = 0;

                if (horizontal != 0 || vertical != 0)
                {
                    initialRot = GameManager.PlayerManager.CurrentPlayer.Angle - 90;
                }
                else if (horizontal == 0 && vertical == 0)
                {
                    initialRot = GameManager.PlayerManager.CurrentPlayer.LastAngle - 90;
                }


                float RotDeg = initialRot + (spaceBetweenProjectiles) * ((ShotgunAmmo.Count - 1) / 2) - (spaceBetweenProjectiles * index);
                index++;

                ShotgunAmmo[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, RotDeg));
                ShotgunAmmo[0].GetComponent<WeaponBase>().SetStats();
                ShotgunAmmo[0].GetComponent<WeaponBase>().SetSkill(GameManager.AIManager.EnemyList);
                ShotgunAmmo.Remove(ShotgunAmmo[0]);
            }

            yield return new WaitForSeconds(weapon.Cooldown);
            StartCoroutine(ShotgunSlotRoutine(weapon));
        }

        private IEnumerator AreaWeaponSlotRoutine(WeaponBase weapon)
        {
            weapon.IsActivated = true;
            weapon.gameObject.SetActive(true);
            yield return new WaitForSeconds(weapon.Cooldown);
            weapon.IsActivated = false;
            weapon.gameObject.SetActive(false);
            StartCoroutine(AreaWeaponSlotRoutine(weapon));
        }

        private void OnLevelStart()
        {
            InvokeDefaultWeapon();
        }
        private void OnLevelFailed()
        {
            StopAllCoroutines();
            CancelInvoke("InitializeDefaultWeapon");

            mDefaultWeapon.IsInitialized = false;
            for (int i = 0; i < ProjectileWeaponsInUse.Count; i++)
            {
                if (ProjectileWeaponsInUse[i].IsInitialized == true)
                {
                    ProjectileWeaponsInUse[i].IsInitialized = false;

                }

            }
            for (int i = 0; i < BounceWeaponsInUse.Count; i++)
            {
                if (BounceWeaponsInUse[i].IsInitialized == true)
                {
                    BounceWeaponsInUse[i].IsInitialized = false;
                }

            }
            for (int i = 0; i < YoyoWeaponsInUse.Count; i++)
            {
                if (YoyoWeaponsInUse[i].IsInitialized == true)
                {
                    YoyoWeaponsInUse[i].IsInitialized = false;
                }
            }
            for (int i = 0; i < ShotgunWeaponsInUse.Count; i++)
            {
                if (ShotgunWeaponsInUse[i].IsInitialized == true)
                {
                    ShotgunWeaponsInUse[i].IsInitialized = false;
                }
            }
            for (int i = 0; i < WhipWeaponsInUse.Count; i++)
            {
                if (WhipWeaponsInUse[i].IsInitialized == true)
                {
                    WhipWeaponsInUse[i].IsInitialized = false;
                }
            }
            for (int i = 0; i < BombWeaponsInUse.Count; i++)
            {
                if (BombWeaponsInUse[i].IsInitialized == true)
                {
                    BombWeaponsInUse[i].IsInitialized = false;
                }
            }

            for (int i = 0; i < SkunkGasWeaponsInUse.Count; i++)
            {
                if (SkunkGasWeaponsInUse[i].IsInitialized == true)
                {
                    SkunkGasWeaponsInUse[i].IsInitialized = false;
                }
            }

            for (int i = 0; i < BananaWeaponsInUse.Count; i++)
            {
                if (BananaWeaponsInUse[i].IsInitialized == true)
                {
                    BananaWeaponsInUse[i].IsInitialized = false;
                }
        }
    }

        private Transform GetClosestEnemy(List<Transform> enemies, WeaponBase weapon)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = weapon.AttackRange;
            Vector3 currentPosition = mPlayer.transform.position;
            foreach (Transform potentialTarget in enemies)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
            return bestTarget;
        }

        private void OnDestroy()
        {

        }
    }


