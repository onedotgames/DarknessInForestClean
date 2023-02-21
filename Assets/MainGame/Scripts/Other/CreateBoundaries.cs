using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoundaries : CustomBehaviour
{
    public GameObject objTopRight;
    public GameObject objDownLeft;
    public GameObject CamBounds;



    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
        }
    }

    private void StartGame()
    {
        SetV2();
    }
    private void Update()
    {
        //if (GameManager.IsGameStarted)
        //{
        //    CamBounds.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
        //    SetV2();
        //}
        CamBounds.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
        SetV2();
    }

    public void SetV2()
    {
        float camDistance = Vector3.Distance(GameManager.PlayerManager.CurrentPlayer.transform.position, MainCamera.transform.position);

        var dl = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        objDownLeft.transform.position = new Vector3(dl.x - 1, dl.y - 1, dl.z);

        var tr = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, camDistance));
        objTopRight.transform.position = new Vector3(tr.x + 1, tr.y + 1, tr.z);
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
        }
    }
}
