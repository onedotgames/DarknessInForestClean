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
    public GameObject QuestNPC;
    public GameObject QuestPanel;
    public GameObject ArrowSign;
    public int HuntTarget;
    public int currentKillCount;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }


    public void Update()
    {
        if (GameManager != null)
        {
            timer += Time.deltaTime;
            if((int)timer == QuestSpawnRateSecond && !hasActiveQuest && canSpawnQuest)
            {
                canSpawnQuest = false;
                SpawnQuest();
            }
            if (!canSpawnQuest)
            {
                var dir = questPos - ArrowSign.transform.position;
                if(dir.magnitude < 5)
                {
                    ArrowSign.SetActive(false);
                }
                else
                {
                    ArrowSign.SetActive(true);
                    var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    ArrowSign.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
            if (hasActiveQuest)
            {
                Debug.Log(hud.killCount - currentKillCount);
                if(hud.killCount - currentKillCount == 50)
                {
                    QuestSuccess();
                }

            }
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
        //kale spawnla.
        // indikatörü kaleye çevir.
        //kale kırılırsa ödülü ver.
        // quest sistemi baştan çalışsın.
    }

    public void Hunt()
    {
        hasActiveQuest = true;
        QuestPanel.SetActive(false);
        Time.timeScale = 1f;
        currentKillCount = hud.killCount;
        QuestNPC.SetActive(false);
        //avlayınca ödülü ver.
        // quest sistemi baştan çalışsın.

    }

    public void QuestSuccess()
    {
        GameManager.PlayerManager.UpdateCoinCountData(50);
        hasActiveQuest = false;
        canSpawnQuest = true;
    }
}
