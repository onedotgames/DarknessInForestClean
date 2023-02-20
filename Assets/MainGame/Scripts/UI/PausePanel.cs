using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PausePanel : UIPanel
{
    public List<Image> WeaponIconsOnPause;
    public List<Image> WeaponUpgradeLevelStars;
    public int WeaponIndex = 0;
    public List<Image> UtilIconsOnPause;
    public int UtilIndex = 0;

    public List<GameObject> WeaponIconStars;
    public List<GameObject> UtilIconStars;

    public Target questInd;
    public Target towerInd;

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
            GameManager.OnRestartGame += RestartGame;
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        questInd.targetColor.a = 0;
        towerInd.targetColor.a = 0;
        AddWeaponStar();
        AddUtilStar();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        questInd.targetColor.a = 255;
        towerInd.targetColor.a = 255;
    }
    public void AddWeaponStar()
    {
        for (int i = 0; i < GameManager.SkillManager.WeaponsInUseTemp.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.WeaponsInUseTemp[i].UpgradeLevel + 1; j++)
            {
                if (GameManager.SkillManager.WeaponsInUseTemp[i].IsEvolved)
                {
                    for (int k = 0; k < WeaponIconStars.Count; k++)
                    {
                        WeaponIconStars[i].transform.GetChild(k).gameObject.SetActive(true);
                    }
                }
                else
                {
                    WeaponIconStars[i].transform.GetChild(j).gameObject.SetActive(true);
                }
            }
        }

    }
    public void CloseWeaponStars()
    {
        for (int i = 0; i < WeaponIconStars.Count; i++)
        {
            for (int k = 0; k < WeaponIconStars.Count; k++)
            {
                WeaponIconStars[i].transform.GetChild(k).gameObject.SetActive(false);
            }
        }
    }

    public void CloseUtilStars()
    {
        for (int i = 0; i < WeaponIconStars.Count; i++)
        {
            for (int k = 0; k < WeaponIconStars.Count; k++)
            {
                UtilIconStars[i].transform.GetChild(k).gameObject.SetActive(false);
            }
        }
    }

    public void AddUtilStar()
    {
        for (int i = 0; i < GameManager.SkillManager.UtilitiesInUseTemp.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.UtilitiesInUseTemp[i].UpgradeLevel; j++)
            {
                UtilIconStars[i].transform.GetChild(j).gameObject.SetActive(true);
            }
        }

    }

    public void UpdateWeaponIcons(Sprite sprite)
    {
        for (int i = 0; i < GameManager.SkillManager.WeaponsInUseTemp.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.WeaponsInUseTemp[i].UpgradeLevel + 1; j++)
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
        for (int i = 0; i < GameManager.SkillManager.UtilitiesInUseTemp.Count; i++)
        {
            for (int j = 0; j < GameManager.SkillManager.UtilitiesInUseTemp[i].UpgradeLevel; j++)
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
        CloseUtilStars();
        CloseWeaponStars();
    }
    private void RestartGame()
    {
        ClosePauseIcons();
        ClosePanel();
        CloseUtilStars();
        CloseWeaponStars();
    }
    private void OnLevelSuccess()
    {
        ClosePauseIcons();
        ClosePanel();
        CloseUtilStars();
        CloseWeaponStars();
    }
    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnLevelCompleted -= OnLevelSuccess;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
}
