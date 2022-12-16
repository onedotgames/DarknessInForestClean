using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerHealthManager : CustomBehaviour
{
    public Player Player;
    public HUD mHud;
    public Image HealthBar;

    public bool IsRegenActive;
    public UtilityBase HpReg;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        EventSubs();
    }

    
    void EventSubs()
    {
        GameManager.OnReturnToMainMenu += ReturnToMainMenu;
    }

    public void ReturnToMainMenu()
    {
        Player.mCurrentHealth = Player.mMaxHealth;
        SetHealthBar(Player.mMaxHealth);
    }

    public void HpRegen()
    {
        if (Player.mCurrentHealth < Player.mMaxHealth)
        {
            Player.mCurrentHealth += HpReg.UtilitySO.UpgradeUtilityDatas[HpReg.UpgradeLevel].ChangeAmount;
            if(Player.mCurrentHealth > Player.mMaxHealth)
            {
                Player.mCurrentHealth = Player.mMaxHealth;
            }
        }
        SetHealthBar(Player.mMaxHealth);
        Invoke("HpRegen", HpReg.UtilitySO.Cooldown);
    }
    public void SetHealthBar(float maxHealth)
    {
        HealthBar.DOFillAmount(Player.mCurrentHealth / maxHealth, 1f);
    }

    private void OnDisable()
    {
        if (GameManager != null)
        {
            GameManager.OnReturnToMainMenu -= ReturnToMainMenu;
        }
    }

    
}
