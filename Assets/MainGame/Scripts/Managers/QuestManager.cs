using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject TargetObj;
    public GameObject QuestPanel;
    public int HuntTarget;
    public int currentKillCount;
    public int enemyType;
    public GameObject Tower;
    public TowerSystem TowerSystem;
    public bool isTowerNear = false;
    public bool canQuestsStart = false;
    public bool isNpcNear = false;
    public TMP_Text questText;
    public GameObject questPanel;
    public TMP_Text towerText;
    public GameObject TowerPanel;
    public EnemyType EnemyType;
    public List<Material> Outlines;
    private string enemy;
    private bool towerQuest = false;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (gameManager != null)
        {
            GameManager.OnStartGame += OnGameStart;
            GameManager.OnLevelCompleted += OnLevelCompleted;
            GameManager.OnLevelFailed += OnLevelFailed;
            GameManager.OnRestartGame += RestartGame;
        }
    }
    private void Awake()
    {
        QuestNPC.SetActive(false);
        Tower.SetActive(false);
        QuestPanel.SetActive(false);
        Outlines.ForEach(x => x.DisableKeyword("OUTBASE_ON"));
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
        questPanel.SetActive(false);
        TowerPanel.SetActive(false);
        currentKillCount = 0;
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
        questPanel.SetActive(false);
        TowerPanel.SetActive(false);
        currentKillCount = 0;
    }

    public void RestartGame()
    {
        hasActiveQuest = false;
        canSpawnQuest = true;
        canQuestsStart = true;
        timer = 0;
        currentKillCount = 0;
        QuestNPC.SetActive(false);
        Tower.SetActive(false);
        QuestPanel.SetActive(false);
        questPanel.SetActive(false);
        TowerPanel.SetActive(false);
        isTowerNear = false;
        hasActiveQuest = false;
    }

    public void Update()
    {
        if (GameManager != null && canQuestsStart)
        {
            timer += Time.deltaTime;
            if (!towerQuest)
            {
                questText.text = "Kill " + enemy + ": " + currentKillCount + " / 50";
                TowerPanel.SetActive(false);
            }
            else
            {
                towerText.text = "Tower Health: " + Tower.GetComponent<TowerSystem>().TowerHealth;
            }
            if ((int)timer == QuestSpawnRateSecond && !hasActiveQuest && canSpawnQuest)
            {
                canSpawnQuest = false;
                SpawnQuest();
            }
            if (hasActiveQuest)
            {
                if(currentKillCount == 50 || TowerSystem.isTowerDestroyed)
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
        if(GameManager.BackgroundManager.mapType == MapType.Normal)
        {
            questPos = new Vector3(Random.Range(-80, 80) + GameManager.PlayerManager.CurrentPlayer.transform.position.x, Random.Range(-80, 80) + GameManager.PlayerManager.CurrentPlayer.transform.position.y, 0);
            QuestNPC.transform.position = questPos;
            QuestNPC.SetActive(true);
            TargetObj.SetActive(true);
        }
        else if(GameManager.BackgroundManager.mapType == MapType.Horizontal)
        {//yatay
            questPos = new Vector3(Random.Range(-80, 80) + GameManager.PlayerManager.CurrentPlayer.transform.position.x, Random.Range(-5, 5), 0);
            QuestNPC.transform.position = questPos;
            QuestNPC.SetActive(true);
            TargetObj.SetActive(true);
        }
        else if(GameManager.BackgroundManager.mapType == MapType.Vertical)
        {//dikey
            questPos = new Vector3(Random.Range(-5, 5), Random.Range(-80, 80) + GameManager.PlayerManager.CurrentPlayer.transform.position.y, 0);
            QuestNPC.transform.position = questPos;
            QuestNPC.SetActive(true);
            TargetObj.SetActive(true);
        }
        
    }

    public void SearchAndDestroy()
    {
        if(GameManager.BackgroundManager.mapType == MapType.Normal)
        {
            towerQuest = true;
            hasActiveQuest = true;
            QuestPanel.SetActive(false);
            Time.timeScale = 1f;
            QuestNPC.SetActive(false);
            towerPos = new Vector3(Random.Range(-40, 40) + GameManager.PlayerManager.CurrentPlayer.transform.position.x, Random.Range(-40, 40) + GameManager.PlayerManager.CurrentPlayer.transform.position.y, 0);
            Tower.transform.position = towerPos;
            Tower.SetActive(true);
            questText.text = "Destroy Tower";
            towerText.text = "Tower Health: " + Tower.GetComponent<TowerSystem>().TowerHealth;
            questPanel.SetActive(true);
            TowerPanel.SetActive(true);
        }
        else if(GameManager.BackgroundManager.mapType == MapType.Horizontal)
        {
            towerQuest = true;
            hasActiveQuest = true;
            QuestPanel.SetActive(false);
            Time.timeScale = 1f;
            QuestNPC.SetActive(false);
            towerPos = new Vector3(Random.Range(-40, 40) + GameManager.PlayerManager.CurrentPlayer.transform.position.x, Random.Range(-5, 5), 0);
            Tower.transform.position = towerPos;
            Tower.SetActive(true);
            questText.text = "Destroy Tower";
            towerText.text = "Tower Health: " + Tower.GetComponent<TowerSystem>().TowerHealth;
            questPanel.SetActive(true);
            TowerPanel.SetActive(true);
        }
        else if(GameManager.BackgroundManager.mapType == MapType.Vertical)
        {
            towerQuest = true;
            hasActiveQuest = true;
            QuestPanel.SetActive(false);
            Time.timeScale = 1f;
            QuestNPC.SetActive(false);
            towerPos = new Vector3(Random.Range(-5, 5), Random.Range(-40, 40) + GameManager.PlayerManager.CurrentPlayer.transform.position.y, 0);
            Tower.transform.position = towerPos;
            Tower.SetActive(true);
            questText.text = "Destroy Tower";
            towerText.text = "Tower Health: " + Tower.GetComponent<TowerSystem>().TowerHealth;
            questPanel.SetActive(true);
            TowerPanel.SetActive(true);
        }
    }

    public void Hunt()
    {
        towerQuest = false;
        hasActiveQuest = true;
        QuestPanel.SetActive(false);
        Time.timeScale = 1f;
        currentKillCount = 0;
        QuestNPC.SetActive(false);
        enemyType = Random.Range(0, 4);
        enemy = EnemyType.GetName(typeof(EnemyType), enemyType);
        Outlines[enemyType].EnableKeyword("OUTBASE_ON");
        questText.text = "Kill " + enemy + ": " + currentKillCount + " / 50";
        questPanel.SetActive(true);
    }

    public void QuestSuccess()
    {
        GameManager.PlayerManager.UpdateCoinCountData(50);
        hasActiveQuest = false;
        canSpawnQuest = true;
        currentKillCount = 0;
        questPanel.SetActive(false);
        Outlines.ForEach(x => x.DisableKeyword("OUTBASE_ON"));
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= OnGameStart;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
            GameManager.OnLevelFailed -= OnLevelFailed;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
}
