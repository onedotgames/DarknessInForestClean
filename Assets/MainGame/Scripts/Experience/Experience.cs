using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Experience : CustomBehaviour
{
    public bool isGoing;
    public ExpPoolerType ExpPoolerType;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
        }
    }
    private void Update()
    {
        if (isGoing)
        {
            MoveExp();
        }
    }

    public void MoveExp()
    {
        transform.DOMove(GameManager.PlayerManager.CurrentPlayer.transform.position, 1f);
    }

    private void LevelFailed()
    {
        if (gameObject.activeInHierarchy)
        {
            GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType].ReturnObjectToPool(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;
        }
    }
}
