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
        if(popCount == 3)
        {
            MiniGamePanel.SetActive(false);
            skillManager.IsMiniGameDone = true;
        }

        if (skillManager.IsMiniGameDone)
        {
            GameManager.SkillManager.ActivateWeapon((int)GameManager.SkillManager.selectedWeaponData.Weapon.SkillSO.PoolerType);
            HUD.OpenPanel();
            Time.timeScale = 1;
            skillManager.IsMiniGameDone = false;
            popCount = 0;
            scoreText.text = (popCount + " / 3");
        }
    }

    private void Awake() => Instance = this;
}
