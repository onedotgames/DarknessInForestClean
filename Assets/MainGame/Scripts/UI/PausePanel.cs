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

    public void UpdateWeaponIcons(Sprite sprite)
    {

        WeaponIconsOnPause[WeaponIndex].sprite = sprite;
        WeaponIconsOnPause[WeaponIndex].gameObject.SetActive(true);
        WeaponIndex++;
    }
    public void UpdateUtilIcons(Sprite sprite)
    {

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
