using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanic : CustomBehaviour
{
    public GameOptions GameOptions { get; set; }

    public virtual void Initialize(GameOptions gameOptions)
    {
        GameOptions = gameOptions;
        GameManager = GameOptions.GameManager;
    }

    public virtual void InitializeCustomOptions()
    {

    }

    public virtual void Deactivate()
    {

    }
}
