using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerHealthManager : CustomBehaviour
{
    public Player Player;
    public HUD mHud;
    public UnityEngine.UI.Image HealthBar;

    public bool IsRegenActive;
    public UtilityBase HpReg;
    private float _timeValue;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        EventSubs();
    }

    
    void EventSubs()
    {
        if(GameManager != null)
        {
            GameManager.OnReturnToMainMenu += ReturnToMainMenu;
            GameManager.OnStartGame += StartGame;
        }
    }

    private void Update()
    {
        if(GameManager.IsGameStarted && !GameManager.IsGamePaused && IsRegenActive)
        {
            _timeValue += Time.deltaTime;
            if (_timeValue >= HpReg.UtilitySO.Cooldown)
            {
                _timeValue = 0;
                if (Player.mCurrentHealth < Player.mMaxHealth)
                {
                    Player.mCurrentHealth += HpReg.UtilitySO.UpgradeUtilityDatas[HpReg.UpgradeLevel].ChangeAmount;
                    if (Player.mCurrentHealth > Player.mMaxHealth)
                    {
                        Player.mCurrentHealth = Player.mMaxHealth;
                    }
                    Player.HealingEffect.gameObject.SetActive(true);
                }
                else
                {
                    Player.HealingEffect.gameObject.SetActive(false);

                }
            }
            
        }
    }

    public void ReturnToMainMenu()
    {
        Player.mCurrentHealth = Player.mMaxHealth;
        SetHealthBar(Player.mMaxHealth);
        IsRegenActive = false;
    }
    public void StartGame()
    {
        Player.mCurrentHealth = Player.mMaxHealth;
        SetHealthBar(Player.mMaxHealth);
        IsRegenActive = false;
    }

    public void HpRegen()
    {
        if (IsRegenActive)
        {
            if (Player.mCurrentHealth < Player.mMaxHealth)
            {
                Player.mCurrentHealth += HpReg.UtilitySO.UpgradeUtilityDatas[HpReg.UpgradeLevel].ChangeAmount;
                if (Player.mCurrentHealth > Player.mMaxHealth)
                {
                    Player.mCurrentHealth = Player.mMaxHealth;
                }
                Player.HealingEffect.gameObject.SetActive(true);
            }
            SetHealthBar(Player.mMaxHealth);
            Invoke("HpRegen", HpReg.UtilitySO.Cooldown);
        }
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
            GameManager.OnStartGame -= StartGame;
        }
    }

    
}
