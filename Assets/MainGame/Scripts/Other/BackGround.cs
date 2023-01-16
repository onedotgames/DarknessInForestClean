using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : CustomBehaviour
{
    public Transform player;
    public Renderer renderTerrain;
    public float speed;
    public Material bgMat;

    public void Update()
    {
        //renderTerrain.materials[0].mainTextureOffset= new Vector2(player.transform.position.x, player.transform.position.y) * speed;
        bgMat.mainTextureOffset = new Vector2(player.transform.position.x, player.transform.position.y) * speed;
    }
}
