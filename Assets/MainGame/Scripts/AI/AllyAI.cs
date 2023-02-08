using Panda;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyAI : CustomBehaviour
{
    private Transform playerTransform;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        playerTransform = GameManager.PlayerManager.CurrentPlayer.transform;
    }

    public enum ActionState { IDLE, WORKING};
    private ActionState state = ActionState.IDLE;
}