using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : CustomBehaviour
{
    public EnvironmentChallenge[] EnvironmentChallenges;
    public int CurrentChallengeType;
    public bool isChallengeLevel = true;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        for (int i = 0; i < EnvironmentChallenges.Length; i++)
        {
            EnvironmentChallenges[i].Initialize(gameManager);
        }
    }
}
