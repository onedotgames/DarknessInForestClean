using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopChar : CustomBehaviour
{
    [SerializeField] private CircleCollider2D _enemyDetectionCollider;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void LateUpdate()
    {
        if(GameManager.IsGameStarted && !GameManager.IsGamePaused)
        {

        }
    }

    public void DoEmoji()
    {
        Debug.Log("Hello " + GameManager.PlayerManager.CurrentPlayer.name + " :)");
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TAGS.Enemy))
        {

        }
    }
}
