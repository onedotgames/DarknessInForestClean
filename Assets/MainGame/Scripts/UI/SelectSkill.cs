using Lofelt.NiceVibrations;
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
    public List<GameObject> BtnOneActives;
    public List<GameObject> BtnOneDeactives;


    public CustomButton SkillButtonTwo;
    public TMP_Text ButtonTwoText;
    public Image ButtonTwoImage;
    public List<GameObject> BtnTwoActives;
    public List<GameObject> BtnTwoDeactives;

    public CustomButton SkillButtonThree;
    public TMP_Text ButtonThreeText;
    public Image ButtonThreeImage;
    public List<GameObject> BtnThreeActives;
    public List<GameObject> BtnThreeDeactives;

    public List<ButtonData> ButtonDataList;

    private HUD mHud;
    //private WeaponManager mWeaponManager;
    public SkillManager mSkillManager;
    public List<Image> WeaponIcons;
    public List<Image> UtilityIcons;

    [Header("Info Panel")]
    public TMP_Text ExpText;
    public Slider ExpSlider;
    public TMP_Text CoinText;
    public TMP_Text LevelText;

    public GameObject LevelUpText;
    public GameObject PlayerIcon;

    private int btnOneLevel;
    private int btnTwoLevel;
    private int btnThreeLevel;


    public List<GameObject> Effects;
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        SubscribeEvents();
        mHud = uIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        mSkillManager = uIManager.GameManager.SkillManager;
        //mWeaponManager = uIManager.GameManager.WeaponManager;
    }

    public void AssignSkillButtonsV2()
    {
        //CLear the lists of objects
        mSkillManager.WeaponBasesV2.Clear();
        mSkillManager.UtilityBases.Clear();

        //Check if they reached their limits
        mSkillManager.CheckWeaponLimitReached();
        mSkillManager.CheckUtilLimitReached();

        mSkillManager.CreateTempWeaponList();
        mSkillManager.CreateTempUtilList();

        for (int i = 0; i < ButtonDataList.Count; i++)
        {
            if (GameManager.PlayerLevelManager.PlayerLevel < 5)
            {
                AssignWeaponV3(i);
                GetUpgradeLevels(0, i);
            }
            else
            {
                //Pick weapon or Util
                int j = Random.Range(0, 2);

                if (j == 0)
                {
                    //Weapon
                    if (i < mSkillManager.TempWeaponsV2.Count)
                    {
                        //AssignWeapon(i);
                        AssignWeaponV3(i);
                        GetUpgradeLevels(0, i);
                    }
                    else
                    {
                        if (i < mSkillManager.TempUtils.Count)
                        {
                            //AssignUtil(i);
                            AssignUtilV2(i);
                            GetUpgradeLevels(0, i);
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
                    if (i < mSkillManager.TempUtils.Count)
                    {
                        AssignUtilV2(i);
                        GetUpgradeLevels(1, i);
                    }
                    else
                    {
                        if (i < mSkillManager.TempWeaponsV2.Count)
                        {
                            AssignWeaponV3(i);
                            GetUpgradeLevels(0, i);
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

    private void GetUpgradeLevels(int type, int buttonIndex) // 0 ise weapon 1 ise util
    {
        if(type == 0) //weapon
        {
            if(buttonIndex == 0)
            {
                btnOneLevel = ButtonDataList[0].Weapon.UpgradeLevel + 1;
                if (GameManager.SkillManager.WeaponsInUseTemp.Contains(ButtonDataList[0].Weapon))
                {
                    for (int i = 0; i < btnOneLevel; i++)
                    {
                        BtnOneActives[i].SetActive(true);
                    }
                }
                else
                {
                    BtnOneActives.ForEach(x => x.SetActive(false));
                }
            }
            else if(buttonIndex == 1)
            {
                btnTwoLevel = ButtonDataList[1].Weapon.UpgradeLevel + 1;
                if (GameManager.SkillManager.WeaponsInUseTemp.Contains(ButtonDataList[1].Weapon))
                {
                    for (int i = 0; i < btnTwoLevel; i++)
                    {
                        BtnTwoActives[i].SetActive(true);
                    }
                }
                else
                {
                    BtnTwoActives.ForEach(x => x.SetActive(false));
                }
            }
            else if(buttonIndex == 2)
            {
                btnThreeLevel = ButtonDataList[2].Weapon.UpgradeLevel + 1;
                if (GameManager.SkillManager.WeaponsInUseTemp.Contains(ButtonDataList[2].Weapon))
                {
                    for (int i = 0; i < btnThreeLevel; i++)
                    {
                        BtnThreeActives[i].SetActive(true);
                    }
                }
                else
                {
                    BtnThreeActives.ForEach(x => x.SetActive(false));
                }
            }
        }
        else if(type == 1) //util
        {
            if (buttonIndex == 0)
            {
                btnOneLevel = ButtonDataList[0].Utility.UpgradeLevel;
                if (GameManager.SkillManager.UtilitiesInUseTemp.Contains(ButtonDataList[0].Utility))
                {
                    for (int i = 0; i < btnOneLevel; i++)
                    {
                        BtnOneActives[i].SetActive(true);
                    }
                }
                else
                {
                    BtnOneActives.ForEach(x => x.SetActive(false));
                }
            }
            else if (buttonIndex == 1)
            {
                btnTwoLevel = ButtonDataList[1].Utility.UpgradeLevel;
                if (GameManager.SkillManager.UtilitiesInUseTemp.Contains(ButtonDataList[1].Utility))
                {
                    for (int i = 0; i < btnTwoLevel; i++)
                    {
                        BtnTwoActives[i].SetActive(true);
                    }
                }
                else
                {
                    BtnTwoActives.ForEach(x => x.SetActive(false));
                }
            }
            else if (buttonIndex == 2)
            {
                btnThreeLevel = ButtonDataList[2].Utility.UpgradeLevel;
                if (GameManager.SkillManager.UtilitiesInUseTemp.Contains(ButtonDataList[2].Utility))
                {
                    for (int i = 0; i < btnThreeLevel; i++)
                    {
                        BtnThreeActives[i].SetActive(true);
                    }
                }
                else
                {
                    BtnThreeActives.ForEach(x => x.SetActive(false));
                }
            }
        }
    }
    public void AssignWeaponV3(int index)
    {
        mSkillManager.WeaponBasesV2 = new List<WeaponBaseV2>();
        WeaponBaseV2 weapon = mSkillManager.TempWeaponsV2[Random.Range(0, mSkillManager.TempWeaponsV2.Count)];
        mSkillManager.WeaponBasesV2.Add(weapon);
        mSkillManager.TempWeaponsV2.Remove(weapon);

        ButtonDataList[index].Button.Initialize(GameManager.UIManager, mSkillManager.InvokeWeaponV2, true);
        ButtonDataList[index].Image.sprite = weapon.SkillSO.Icon;
        ButtonDataList[index].Text.SetText(weapon.SkillSO.name);
        ButtonDataList[index].Weapon = weapon;
    }

    public void AssignUtilV2(int index)
    {
        mSkillManager.UtilityBases = new List<UtilityBase>();

        UtilityBase util = mSkillManager.TempUtils[Random.Range(0, mSkillManager.TempUtils.Count)];
        mSkillManager.UtilityBases.Add(util);
        mSkillManager.TempUtils.Remove(util);

        ButtonDataList[index].Button.Initialize(GameManager.UIManager, mSkillManager.InvokeUtility, true);
        ButtonDataList[index].Image.sprite = util.UtilitySO.Icon;
        ButtonDataList[index].Text.SetText(util.UtilitySO.name);
        ButtonDataList[index].Utility = util;
    }

    private void SubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
            GameManager.OnStartGame += OnLevelStart;
            GameManager.OnLevelFailed += OnLevelFailed;
            GameManager.OnLevelCompleted += OnLevelCompleted;
            GameManager.OnRestartGame += RestartGame;
        }
    }
    public override void OpenPanel()
    {
        base.OpenPanel();
        Debug.Log("Level atladım.");
        GameManager.UIManager.GetPanel(Panels.Hud).ClosePanel();
        UpdateUIElements();
        GameManager.VibrationsManager.PlayVibration(HapticPatterns.PresetType.Success);

        ExpText.text = GameManager.PlayerLevelManager.CurrentExp + " / " + GameManager.PlayerLevelManager.LevelRequirement;
        ExpSlider.value = GameManager.PlayerLevelManager.CurrentExp / GameManager.PlayerLevelManager.LevelRequirement;
        CoinText.text = GameManager.PlayerManager.GetTotalCoinCount().ToString();
        LevelText.text = (GameManager.PlayerLevelManager.PlayerLevel + 1).ToString();
        PlayerIcon.SetActive(true);
        LevelUpText.SetActive(true);
        Effects.ForEach(x => x.SetActive(true));
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        for (int i = 0; i < BtnOneActives.Count; i++)
        {
            BtnOneActives[i].SetActive(false);
            BtnTwoActives[i].SetActive(false);
            BtnThreeActives[i].SetActive(false);
        }
        Effects.ForEach(x => x.SetActive(false));
        //CloseSkillPanelAndOpenHud();
        PlayerIcon.SetActive(false);
        LevelUpText.SetActive(false);
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

    private void LevelEnd()
    {
        
    }

    private void LevelStart()
    {

    }

    private void OnLevelStart()
    {
        LevelStart();
    }

    private void RestartGame()
    {
        ClosePanel();
    }

    private void OnLevelFailed()
    {
        LevelEnd();
    }

    private void OnLevelCompleted()
    {
        LevelEnd();
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
            GameManager.OnStartGame -= OnLevelStart;
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
            GameManager.OnRestartGame -= RestartGame;
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
    //public WeaponBase Weapon;
    public WeaponBaseV2 Weapon;
    public UtilityBase Utility;
}