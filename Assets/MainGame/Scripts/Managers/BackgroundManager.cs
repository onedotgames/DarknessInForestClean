using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : CustomBehaviour
{
    public BGList BGList;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        BGList.Initialize(gameManager);
    }
}
