using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using Panda.Examples.PlayTag;
using System.Runtime.CompilerServices;

public class Experience : CustomBehaviour
{
    public bool isGoing;
    public bool IsMovingAway;
    public bool IsReturning;
    public ExpPoolerType ExpPoolerType;
    [SerializeField] private float moveAwayFactor;
    [SerializeField] private float moveBackFactor;
    public GameObject Shadow;
    public float moveAwaySpeed = 1.0f;
    public float moveBackSpeed = 1.0f;
    public float waitTime = 0.2f;
    public Vector3 startPos;
    private Vector3 targetPosition;
    public Player player;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SubEvents();
        player = gameManager.PlayerManager.CurrentPlayer;
    }
    private void OnEnable()
    {
        isGoing = false;
        IsReturning = false;
    }


    private void OnDisable()
    {
        isGoing = false;
        IsReturning = false;
    }

    public void SubEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnLevelFailed += LevelFailed;
            GameManager.OnRestartGame += RestartGame;
        }
    }

    //private void Update()
    //{
    //    if (isGoing)
    //    {
    //        MoveExp();
    //    }
    //}

    private void Update()
    {
        if (IsReturning)
        {
            targetPosition = GameManager.PlayerManager.CurrentPlayer.transform.position;
            var myTween2 = transform.DOMove(targetPosition, 0.1f).SetEase(Ease.InQuad);

        }
    }

    public void TriggerExperience()
    {
        StartCoroutine(ExperienceMovementRoutine());
    }

    private IEnumerator ExperienceMovementRoutine()
    {
        Shadow.SetActive(false);

        Vector3 targetDirection = (transform.position - 
            GameManager.PlayerManager.CurrentPlayer.transform.position).normalized;
        
        var myTween = transform.DOMove(transform.position + 
            (targetDirection * moveAwayFactor), 0.5f).SetEase(Ease.OutQuad);
        yield return myTween.WaitForCompletion();
        //targetPosition = GameManager.PlayerManager.CurrentPlayer.transform.position;
        //var myTween2 = transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InQuad);
        IsReturning = true;


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

    private void RestartGame()
    {
        if (gameObject.activeInHierarchy)
        {
            GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType].ReturnObjectToPool(this.gameObject);
        }
    }

    private void LevelCompleted()
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
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
}
