using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerLevelManager : CustomBehaviour
{
    private float CurrentExp;
    public float LevelRequirement;
    [SerializeField] private float InitialLevelRequirement;
    private HUD mHud;
    private SelectSkill mSelectSkillPanel;
    public int PlayerLevel = 0;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        mHud = gameManager.UIManager.GetPanel(Panels.Hud).GetComponent<HUD>();
        mSelectSkillPanel = gameManager.UIManager.GetPanel(Panels.SelectSkill).GetComponent<SelectSkill>();
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
            GameManager.OnStartGame += GameStarted;
        }
        SetLevelRequirement();

        CurrentExp = 0;
        mHud.SetExpBarFillAmount();
        mHud.UpdateLevelText();
        
    }
    
    public void SetExp(float amount)
    {
        CurrentExp += amount;
    }

    public float GetCurrentExp()
    {
        return CurrentExp;
    }

    public float GetLvlReq()
    {
        return LevelRequirement;
    }

    public void CheckExp()
    {
        if (CurrentExp / LevelRequirement >= 1)
        {
            CurrentExp = CurrentExp - LevelRequirement;
            SetLevelRequirement();
            if(PlayerLevel <= 1)
            {
                mSelectSkillPanel.AssignWeaponsToButtons();
            }
            else
            {
                int i = Random.Range(0, 2);

                if (i == 0)
                {
                    mSelectSkillPanel.AssignWeaponsToButtons();
                }
                else if (i == 1)
                {
                    GameManager.WeaponManager.AssignUtilsToButtons();
                }
                
            }
            
            LevelUp();
            SetExp(CurrentExp);

        }
    }

    public void SetLevelRequirement()
    {
        if (PlayerLevel < 10)
        {
            LevelRequirement += 100f;
        }else if(PlayerLevel>=10 && PlayerLevel < 20)
        {
            LevelRequirement += 200;
        }
        else if(PlayerLevel >= 20 && PlayerLevel < 30)
        {
            LevelRequirement += 400;
        }
        else if (PlayerLevel >= 30 && PlayerLevel < 40)
        {
            LevelRequirement += 600;
        }
        else if (PlayerLevel >= 40 && PlayerLevel < 50)
        {
            LevelRequirement += 1000;
        }
        else if (PlayerLevel >= 50 && PlayerLevel < 60)
        {
            LevelRequirement += 1500;
        }
        else
        {
            LevelRequirement += 3000;
        }

    }

    private void LevelUp()
    {
        mSelectSkillPanel.OpenPanel();
        PlayerLevel++;
        mHud.UpdateLevelText();

        Time.timeScale = 0;
    }

    private void GameStarted()
    {
        CurrentExp = 0;
        PlayerLevel = 0;
        LevelRequirement = InitialLevelRequirement;
        mHud.UpdateLevelText();
        mHud.SetExpBarFillAmount();
    }

    private void LevelFailed()
    {
        mSelectSkillPanel.ClosePanel();
    }

    private void LevelCompleted()
    {

    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= GameStarted;
            GameManager.OnLevelFailed -= LevelFailed;
        }
    }
}
