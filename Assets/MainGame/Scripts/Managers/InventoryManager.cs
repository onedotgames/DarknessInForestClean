using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : CustomBehaviour
{
    public List<InventorySlot> InventorySlots;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        InventorySlots.ForEach(x => x.Initialize(GameManager));
    }
}
