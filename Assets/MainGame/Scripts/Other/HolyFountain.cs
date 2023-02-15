using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class HolyFountain : CustomBehaviour
{
    public GameObject Fountain;
    public ParticleSystem SmokeVFX;
    public CircleCollider2D HolyFountainCollider;
    public bool isFountainTaken = false;
    public GameObject Water;
    public GameObject TargetObj;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnLevelFailed += LevelFailed;
        }
    }

    private void Awake()
    {
        Fountain.SetActive(false);
        HolyFountainCollider.enabled = true;
    }

    private void StartGame()
    {
        Fountain.transform.position = new Vector3(Random.Range(-80, 80), Random.Range(-80, 80), 0);
        Fountain.SetActive(true);
        Water.transform.localScale = Vector3.one;
        HolyFountainCollider.enabled = true;
    }

    private void Update()
    {
        if (Mathf.Abs(Vector3.Distance(TargetObj.transform.position, GameManager.PlayerManager.CurrentPlayer.transform.position)) < 4.5f)
        {
            TargetObj.SetActive(false);
        }
        else
        {
            TargetObj.SetActive(true);
        }
        var healthRatio = GameManager.PlayerHealthManager.Player.mCurrentHealth / 100;
        TargetObj.GetComponent<Target>().targetColor.a = 1 - healthRatio;
    }

    private void LevelCompleted()
    {
        Fountain.SetActive(false);
        isFountainTaken = false;
    }

    private void LevelFailed()
    {
        Fountain.SetActive(false);
        isFountainTaken = false;
    }


    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnLevelFailed -= LevelFailed;
        }
    }

}
