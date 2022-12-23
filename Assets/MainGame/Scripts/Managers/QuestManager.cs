using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : CustomBehaviour
{
    public HUD hud;
    public int QuestSpawnRateSecond;
    public int QuestGoldReward;
    public bool hasActiveQuest = false;
    public bool canSpawnQuest = true;
    private float timer;
    private Vector3 questPos;
    private Vector3 towerPos;
    public GameObject QuestNPC;
    public GameObject QuestPanel;
    public int HuntTarget;
    public int currentKillCount;
    public GameObject Tower;
    public TowerSystem TowerSystem;
    public bool isTowerNear = false;
    public bool canQuestsStart = false;
    public bool isNpcNear = false;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (gameManager != null)
        {
            GameManager.OnStartGame += OnGameStart;
            GameManager.OnLevelCompleted += OnLevelCompleted;
            GameManager.OnLevelFailed += OnLevelFailed;
        }
    }
    private void Awake()
    {
        QuestNPC.SetActive(false);
        Tower.SetActive(false);
        QuestPanel.SetActive(false);
    }

    public void OnGameStart()
    {
        hasActiveQuest = false;
        canSpawnQuest = true;
        canQuestsStart = true;
        timer = 0;
        currentKillCount = 0;
    }

    public void OnLevelCompleted()
    {
        QuestNPC.SetActive(false);
        Tower.SetActive(false);
        QuestPanel.SetActive(false);
        canQuestsStart = false;
        isTowerNear = false;
        hasActiveQuest = false;
        canSpawnQuest = true;
    }
    public void OnLevelFailed()
    {
        QuestNPC.SetActive(false);
        Tower.SetActive(false);
        QuestPanel.SetActive(false);
        canQuestsStart = false;
        isTowerNear = false;
        hasActiveQuest = false;
        canSpawnQuest = true;
    }
    public void Update()
    {
        if (GameManager != null && canQuestsStart)
        {
            timer += Time.deltaTime;
            if ((int)timer == QuestSpawnRateSecond && !hasActiveQuest && canSpawnQuest)
            {
                canSpawnQuest = false;
                SpawnQuest();
            }
            if (hasActiveQuest)
            {
                if(hud.killCount - currentKillCount == 50 || TowerSystem.isTowerDestroyed)
                {
                    QuestSuccess();
                }
            }
            
        }
        if (QuestNPC.activeInHierarchy && Mathf.Abs(Vector3.Distance(QuestNPC.transform.position, GameManager.PlayerManager.CurrentPlayer.transform.position)) < 5f)
        {
            isNpcNear = true;
        }
        else
        {
            isNpcNear = false;
        }
        if (Tower.activeInHierarchy && hasActiveQuest && Mathf.Abs(Vector3.Distance(Tower.transform.position, GameManager.PlayerManager.CurrentPlayer.transform.position)) < 5f)
        {
            isTowerNear = true;
        }
        else
        {
            isTowerNear = false;
        }
    }

    public void SpawnQuest()
    {
        questPos = new Vector3(Random.Range(-80, 80) + GameManager.PlayerManager.CurrentPlayer.transform.position.x, Random.Range(-80, 80) + GameManager.PlayerManager.CurrentPlayer.transform.position.y, 0);
        QuestNPC.transform.position = questPos;
        QuestNPC.SetActive(true);
    }

    public void SearchAndDestroy()
    {
        hasActiveQuest = true;
        QuestPanel.SetActive(false);
        Time.timeScale = 1f;
        QuestNPC.SetActive(false);
        towerPos = new Vector3(Random.Range(-40, 40) + GameManager.PlayerManager.CurrentPlayer.transform.position.x, Random.Range(-40, 40) + GameManager.PlayerManager.CurrentPlayer.transform.position.y, 0);
        Tower.transform.position = towerPos;
        Tower.SetActive(true);
    }

    public void Hunt()
    {
        hasActiveQuest = true;
        QuestPanel.SetActive(false);
        Time.timeScale = 1f;
        currentKillCount = hud.killCount;
        QuestNPC.SetActive(false);

    }

    public void QuestSuccess()
    {
        GameManager.PlayerManager.UpdateCoinCountData(50);
        hasActiveQuest = false;
        canSpawnQuest = true;
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= OnGameStart;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
            GameManager.OnLevelFailed -= OnLevelFailed;
        }
    }
}
