using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : GameMechanic
{
    public override void Initialize(GameOptions gameOptions)
    {
        base.Initialize(gameOptions);
        InitializeCustomOptions();
    }

    public override void InitializeCustomOptions()
    {
        base.InitializeCustomOptions();
        Deactivate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
