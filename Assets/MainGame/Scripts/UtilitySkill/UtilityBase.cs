using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class UtilityBase : CustomBehaviour
{
    public UtilitySO UtilitySO;
    public int UpgradeLevel = 0;
    public UtilityPattern UtilityPattern;
    protected Player mPlayer;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
            GameManager.OnLevelCompleted += LevelSuccess;
            GameManager.OnStartGame += StartGame;
            GameManager.OnRestartGame += RestartGame;
        }
        mPlayer = gameManager.PlayerManager.CurrentPlayer;
    }
    

    public void MakeUpgrade()
    {
        if(UpgradeLevel< UtilitySO.UpgradeUtilityDatas.Count - 1)
        {
            switch (UtilityPattern)
            {
                case UtilityPattern.MovementSpeed:
                    if (UtilitySO.UpgradeUtilityDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Multiplication)
                    {
                        mPlayer.mForwardSpeed *= UtilitySO.UpgradeUtilityDatas[UpgradeLevel].ChangeAmount;
                    }
                    else if (UtilitySO.UpgradeUtilityDatas[UpgradeLevel].PropertyChangeType == PropertyChangeType.Addition)
                    {
                        mPlayer.mForwardSpeed += UtilitySO.UpgradeUtilityDatas[UpgradeLevel].ChangeAmount;
                    }
                    break;

                case UtilityPattern.Health:
                    mPlayer.mMaxHealth += UtilitySO.UpgradeUtilityDatas[UpgradeLevel].ChangeAmount;
                    mPlayer.mCurrentHealth += UtilitySO.UpgradeUtilityDatas[UpgradeLevel].ChangeAmount;
                    GameManager.PlayerHealthManager.SetHealthBar(mPlayer.mMaxHealth);
                    break;

                case UtilityPattern.DamageReduction:
                    mPlayer.DamageReduction *= UtilitySO.UpgradeUtilityDatas[UpgradeLevel].ChangeAmount;
                    break;

                case UtilityPattern.RecharcableShield:
                    if (!mPlayer.IsShieldOn)
                    {
                        ShieldOn();
                    }
                    else
                    {
                        mPlayer.ShieldValue = UtilitySO.UpgradeUtilityDatas[UpgradeLevel].ChangeAmount;
                    }
                    break;

                case UtilityPattern.HealthRegen:
                    GameManager.PlayerHealthManager.IsRegenActive = true;
                    //GameManager.PlayerHealthManager.HpRegen();
                    break;

                case UtilityPattern.WeaponCooldownReduction:
                    break;
            }

            UpgradeLevel++;
            if(UpgradeLevel == UtilitySO.UpgradeUtilityDatas.Count - 2)
            {
                GameManager.SkillManager.AllUtils.Remove(this);
                GameManager.SkillManager.UtilitiesInUse.Remove(this);
            }
        }
    }

    public void ShieldOn()
    {

        mPlayer.CacheShieldRoutine();
    }

    private void LevelFailed()
    {
        UpgradeLevel = 0;
    }
    private void LevelSuccess()
    {
        UpgradeLevel = 0;
    }
    private void StartGame()
    {
        UpgradeLevel = 0;
    }
    private void RestartGame()
    {
        UpgradeLevel = 0;
    }
    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;
            GameManager.OnLevelCompleted -= LevelSuccess;
            GameManager.OnStartGame -= StartGame;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
}
