using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public GameObject MiniGamePanel;
    [SerializeField] private SkillManager skillManager;
    public TextMeshProUGUI scoreText;
    public GameManager GameManager;
    public int popCount = 0;
    public HUD HUD;



    private void Update()
    {
        if(popCount == 5)
        {
            MiniGamePanel.SetActive(false);
            skillManager.IsMiniGameDone = true;
        }

        if (skillManager.IsMiniGameDone)
        {
            skillManager.IsMiniGameDone = false;

            GameManager.SkillManager.AllWeaponsV2[(int)GameManager.SkillManager.selectedWeaponData.Weapon.SkillSO.PoolerType].EvolveWeapon();

            Time.timeScale = 1;
            popCount = 0;
            scoreText.text = (popCount + " / 5");
        }
    }

    private void Awake() => Instance = this;
}
