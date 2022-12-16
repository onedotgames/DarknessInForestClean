using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : CustomBehaviour
{
    public int QuestSpawnRateSecond;
    public int QuestGoldReward;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }


    public void Update()
    {
        if((int)GameManager.TimeManager.TimeValue % QuestSpawnRateSecond == 1)
        {
            Debug.Log("Quest Time");

        }
    }

}
