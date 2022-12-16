using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public float targetTime = 60f;
    public int _score;
    public GameObject MiniGamePanel;
    public WeaponManager weaponManager;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameManager GameManager;
    public int popCount = 0;
    

    private void Update()
    {
        targetTime -= Time.unscaledDeltaTime;
        timerText.SetText("Time: " + targetTime);

        if(popCount == 3)
        {
            MiniGamePanel.SetActive(false);
            weaponManager.isMiniGameDone = true;
        }
        if (weaponManager.isMiniGameDone)
        {
            GameManager.PoolingManager.WeaponPooler[(int)GameManager.WeaponManager.selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());

            Time.timeScale = 1;
            weaponManager.isMiniGameDone = false;
            popCount = 0;
        }
    }
    public int Score
    {
        get  => _score;
        set
        {
            if (_score == value) return;
            _score = value;

        }
    }

    private void Awake() => Instance = this;
}
