using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PausePanel : UIPanel
{
    public List<Image> WeaponIconsOnPause;
    public List<Image> WeaponUpgradeLevelStars;
    public int WeaponIndex = 0;
    public List<Image> UtilIconsOnPause;
    public int UtilIndex = 0;

    public List<GameObject> WeaponIconStars;
    public List<GameObject> UtilIconStars; 

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += OnLevelFailed;
            GameManager.OnLevelCompleted += OnLevelSuccess;
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();

    }

    public void AddWeaponStar()
    {
        for (int i = 0; i < GameManager.SkillManager.WeaponsInUseV2.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.WeaponsInUseV2[i].UpgradeLevel + 1; j++)
            {
                WeaponIconStars[i].transform.GetChild(j).gameObject.SetActive(true);
            }
        }

    }

    public void AddUtilStar()
    {
        for (int i = 0; i < GameManager.SkillManager.UtilitiesInUse.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.UtilitiesInUse[i].UpgradeLevel; j++)
            {
                UtilIconStars[i].transform.GetChild(j).gameObject.SetActive(true);
            }
        }

    }

    public void UpdateWeaponIcons(Sprite sprite)
    {
        for (int i = 0; i < GameManager.SkillManager.WeaponsInUseV2.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.WeaponsInUseV2[i].UpgradeLevel + 1; j++)
            {
                WeaponIconStars[i].transform.GetChild(j).gameObject.SetActive(true);
            }
        }

        WeaponIconsOnPause[WeaponIndex].sprite = sprite;
        WeaponIconsOnPause[WeaponIndex].gameObject.SetActive(true);
        WeaponIndex++;
    }
    public void UpdateUtilIcons(Sprite sprite)
    {
        for (int i = 0; i < GameManager.SkillManager.UtilitiesInUse.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.UtilitiesInUse[i].UpgradeLevel; j++)
            {
                UtilIconStars[i].transform.GetChild(j).gameObject.SetActive(true);
            }
        }

        UtilIconsOnPause[UtilIndex].sprite = sprite;
        UtilIconsOnPause[UtilIndex].gameObject.SetActive(true);
        UtilIndex++;
    }

    public void ClosePauseIcons()
    {
        WeaponIconsOnPause.ForEach(x => x.gameObject.SetActive(false));
        UtilIconsOnPause.ForEach(x => x.gameObject.SetActive(false));
        WeaponIndex = 0;
        UtilIndex = 0;
    }
    private void OnLevelFailed()
    {
        ClosePauseIcons();
        ClosePanel();
    }
    private void OnLevelSuccess()
    {
        ClosePauseIcons();
        ClosePanel();
    }
    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnLevelCompleted -= OnLevelSuccess;
        }
    }
}
