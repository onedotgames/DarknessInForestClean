using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public GameObject MiniGamePanel;
    [SerializeField] private SkillManager skillManager;
    public TextMeshProUGUI scoreText;
    public GameManager GameManager;
    public int popCount = 0;
    public HUD HUD;
    public List<RectTransform> PopOpenList;
    public MiniGameType miniGameType;

    private void Update()
    {
        //if(popCount == 5)
        //{
        //    MiniGamePanel.SetActive(false);
        //    skillManager.IsMiniGameDone = true;
        //}

        //if (skillManager.IsMiniGameDone)
        //{
        //    skillManager.IsMiniGameDone = false;
        //    if(miniGameType == MiniGameType.Evolve)
        //        GameManager.SkillManager.AllWeaponsV2[(int)GameManager.SkillManager.selectedWeaponData.Weapon.SkillSO.PoolerType].EvolveWeapon();

        //    Time.timeScale = 1;
        //    popCount = 0;
        //    scoreText.text = (popCount + " / 5");
        //}
    }

    public void OpenPop()
    {
        for (int i = 0; i < popCount; i++)
        {
            if(i == 0 || i== 1)
            {
                PopOpenList[i].DOScale(Vector3.one, 0.5f);
            }
            else if (i == 2 || i == 3)
            {
                PopOpenList[i].DOScale(Vector3.one * 1.2f, 0.5f);
            }
            else if (i == 4)
            {
                PopOpenList[i].DOScale(Vector3.one * 1.4f, 0.5f);
            }
        }
    }

    public void SuccessMinigame()
    {
        if (popCount >= 5)
        {
            MiniGamePanel.SetActive(false);
            skillManager.IsMiniGameDone = true;
        }

        if (skillManager.IsMiniGameDone)
        {
            skillManager.IsMiniGameDone = false;
            if (miniGameType == MiniGameType.Evolve)
                GameManager.SkillManager.AllWeaponsV2[(int)GameManager.SkillManager.selectedWeaponData.Weapon.SkillSO.PoolerType].EvolveWeapon();

            Time.timeScale = 1;
            popCount = 0;
            scoreText.text = (popCount + " / 5");
        }
    }

    private void Awake() => Instance = this;

    public enum MiniGameType
    {
        Evolve = 0,
        Chest = 1
    }
}
