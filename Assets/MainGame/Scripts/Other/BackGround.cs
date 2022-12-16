using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : CustomBehaviour
{
    public Transform player;
    public Renderer renderTerrain;
    public float speed;

    private void Update()
    {
        renderTerrain.material.mainTextureOffset = new Vector2(player.transform.position.x,
            player.transform.position.y) * speed;
    }


}
