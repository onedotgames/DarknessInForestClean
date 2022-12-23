using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public GameObject MiniGamePanel;
    public WeaponManager weaponManager;
    public TextMeshProUGUI scoreText;
    public GameManager GameManager;
    public int popCount = 0;
    public HUD HUD;



    private void Update()
    {
        if(popCount == 3)
        {
            MiniGamePanel.SetActive(false);
            weaponManager.isMiniGameDone = true;
        }

        if (weaponManager.isMiniGameDone)
        {
            GameManager.PoolingManager.WeaponPooler[(int)GameManager.WeaponManager.selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
            HUD.OpenPanel();
            Time.timeScale = 1;
            weaponManager.isMiniGameDone = false;
            popCount = 0;
            scoreText.text = (popCount + " / 3");
        }
    }

    private void Awake() => Instance = this;
}
