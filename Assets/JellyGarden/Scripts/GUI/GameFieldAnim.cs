using UnityEngine;
using System.Collections;

public class GameFieldAnim : MonoBehaviour {
    public GameManager GameManager;


    //private void Update()
    //{
    //    var player = GameManager.PlayerManager.CurrentPlayer.transform.position;
    //    transform.position = new Vector3(player.x, player.y, 10);
    //}

    void EndAnimGamField()
    {
        //var player = GameManager.PlayerManager.CurrentPlayer.transform.position;
       LevelManager.THIS.gameStatus = GameState.Playing;
        //transform.position = new Vector3(player.x,player.y, 10);
    }
}
