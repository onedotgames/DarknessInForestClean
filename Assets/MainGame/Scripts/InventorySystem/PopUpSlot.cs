using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSlot : CustomBehaviour
{
    public void ChangeSlotImage(Sprite sprite)
    {
        this.Image.sprite = sprite;
    }
}
