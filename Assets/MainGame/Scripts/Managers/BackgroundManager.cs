using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : CustomBehaviour
{
    public BGList BGList;

    public GameObject horizontalGroup;
    public GameObject verticalGroup;
    public MapType mapType;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        BGList.Initialize(gameManager);
    }



    private void Update()
    {
        if(mapType == MapType.Normal)
        {
            verticalGroup.SetActive(false);
            horizontalGroup.SetActive(false);
        }
        else if(mapType == MapType.Horizontal)
        {
            verticalGroup.SetActive(false);
            horizontalGroup.SetActive(true);
            horizontalGroup.transform.position = new Vector3(GameManager.PlayerManager.CurrentPlayer.transform.position.x, horizontalGroup.transform.position.y, horizontalGroup.transform.position.z);
        }
        else if(mapType == MapType.Vertical)
        {
            verticalGroup.SetActive(true);
            horizontalGroup.SetActive(false);
            verticalGroup.transform.position = new Vector3(verticalGroup.transform.position.x, GameManager.PlayerManager.CurrentPlayer.transform.position.y, verticalGroup.transform.position.z);
        }
    }
    
}

public enum MapType
{
    Normal = 0,
    Horizontal = 1,
    Vertical = 2
}