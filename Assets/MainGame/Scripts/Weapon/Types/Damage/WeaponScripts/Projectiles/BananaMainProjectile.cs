using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMainProjectile : ProjectileBase
{
    public Player Player;
    [SerializeField] private Transform Rotator;
    public List<BananaMiniProjectiles> MiniBananas;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        ScaleUpRotator();
    }
    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            transform.position = Player.gameObject.transform.position;
        }
        if(GameManager.IsMiniGame && MiniBananas[0].gameObject.activeSelf)
        {
            MiniBananas.ForEach(x => x.gameObject.SetActive(false));
        }
        else if(!GameManager.IsMiniGame && !MiniBananas[0].gameObject.activeSelf)
        {
            MiniBananas.ForEach(x => x.gameObject.SetActive(true));
        }
    }

    private void ScaleUpRotator()
    {
        Rotator.DOScale(Vector3.one, 0.5f);
    }
}
