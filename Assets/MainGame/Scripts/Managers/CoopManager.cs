using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopManager : CustomBehaviour
{
    public CoopChar CoopChar;
    public AllyAI AllyAi;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        
    }

    private void Update()
    {
        if(GameManager.IsGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //InitializeCoopChar();
                InitializeAllyAi();
            }
        }
    }

    public void InitializeCoopChar()
    {
        CoopChar.Initialize(GameManager);
        ActivateCoopChar();
        CoopChar.DoEmoji();
    }

    public void InitializeAllyAi()
    {
        AllyAi.Initialize(GameManager);
        ActivateAllyAi();
    }

    public void ActivateCoopChar()
    {
        CoopChar.gameObject.SetActive(true);
    }

    public void ActivateAllyAi()
    {
        AllyAi.gameObject.SetActive(true);
    }
}
