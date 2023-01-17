using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class MenuEvent : CustomBehaviour
{
    private float Timer = 0;
    private int LeavesSpawnTime = 0;
    private int FireFliesSpawnTime = 0;
    private int BossSpawnTime = 0;
    private bool isEventStart = false;
    private bool canPlay = false;
    public int minTime;
    public int maxTime;
    public ParticleSystem Leaves;
    public ParticleSystem FireFlies;
    public GameObject Boss;
    public GameObject BossBody;
    public GameObject Shadow;
    public EventType eventType;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        canPlay = true;
        if(GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
            GameManager.OnLevelCompleted += Completed;
            GameManager.OnLevelFailed += Failed;
        }
    }

    private void Awake()
    {
        LeavesSpawnTime = Random.Range(minTime,maxTime);
        FireFliesSpawnTime = Random.Range(minTime,maxTime);
        BossSpawnTime = Random.Range(minTime,maxTime);
    }

    private void StartGame()
    {
        canPlay = false;
        Boss.SetActive(false);
        Leaves.Stop();
        FireFlies.Stop();
    }

    private void Completed()
    {
        canPlay = true;
        Boss.SetActive(true);
    }

    private void Failed()
    {
        canPlay = true;
        Boss.SetActive(true);
    }

    private void Update()
    {
        if (canPlay)
        {
            Timer += Time.deltaTime;
            if (!isEventStart)
            {
                StartEvent();
            }
        }
    }
    public void StartEvent()
    {
        if(eventType == EventType.Leaves)
        {
            if ((int)Timer == LeavesSpawnTime)
            {
                isEventStart = true;
                Leaves.gameObject.SetActive(true);
                Leaves.Play();
                StartCoroutine(LeavesRoutine());
            }
        }
        else if(eventType == EventType.FireFlies)
        {
            if ((int)Timer == FireFliesSpawnTime)
            {
                isEventStart = true;
                FireFlies.gameObject.SetActive(true);
                FireFlies.Play();
                StartCoroutine(FireFliesRoutine());
            }
        }
        else if(eventType == EventType.Boss)
        {
            if ((int)Timer == BossSpawnTime)
            {
                isEventStart = true;
                var animator = Boss.GetComponent<Animator>();
                Boss.GetComponent<SortingGroup>().sortingOrder = 20;
                animator.SetInteger("State", 2);
                StartCoroutine(BossRoutine());
            }
        }
    }

    private IEnumerator LeavesRoutine()
    {
        yield return new WaitForSeconds(5f);
        Leaves.Stop();
        isEventStart = false;
        Timer = 0;
    }

    private IEnumerator FireFliesRoutine()
    {
        FireFlies.gameObject.transform.DOMoveX(2f, 5f).OnComplete(() => FireFlies.gameObject.transform.DOMoveX(-2f, 5f));
        yield return new WaitForSeconds(5f);
        FireFlies.Stop();
        isEventStart = false;
        Timer = 0;
    }

    private IEnumerator BossRoutine()
    {
        BossBody.SetActive(true);
        Shadow.SetActive(true);
        Boss.transform.DOLocalMoveX(-15f, 7f).OnComplete(() => Boss.transform.DOLocalMoveX(15f, 7f));
        Debug.Log(Timer);
        yield return new WaitForSeconds(7f);
        isEventStart = false;
        BossBody.SetActive(false);
        Shadow.SetActive(false);
        Timer = 0;
    }
    public enum EventType
    {
        Leaves = 0,
        FireFlies = 1,
        Boss = 2
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnLevelCompleted -= Completed;
            GameManager.OnLevelFailed -= Failed;
        }
    }
}
