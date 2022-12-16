using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : CustomBehaviour
{
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        GameManager.OnLevelCompleted += OnLevelCompleted;
        GameManager.OnLevelFailed += OnLevelFailed;
    }

    #region Events

    private void OnLevelCompleted()
    {
        GameManager.PlayerManager.UpdateCoinCountData(ConstantDatas.LEVEL_COMPLETE_REWARD);
    }

    private void OnLevelFailed()
    {
        GameManager.PlayerManager.UpdateCoinCountData(ConstantDatas.LEVEL_FAIL_REWARD);
    }
    #endregion
}
