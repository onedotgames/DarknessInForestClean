using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSlot : CustomBehaviour
{
    public GameObject Parent;
    public CustomButton PopUpSlotButton;
    public InventoryObjectStats InventoryObjectStat;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        PopUpSlotButton.Initialize(gameManager.UIManager, SetEquipment);
    }
    public void ChangeSlotImage(Sprite sprite)
    {
        this.Image.sprite = sprite;
    }

    public void SetEquipment()
    {
        Debug.Log("ClickWorks");
        if(InventoryObjectStat.EquipmentType == EquipmentType.Weapon)
        {
            Debug.Log("Should set def weapon here");
            GameManager.SkillManager.DefaultWeaponV2 = GameManager.SkillManager.AllWeaponsV2
                [(int)InventoryObjectStat.SkillSO.PoolerType];
        }
        else
        {
            Debug.Log("Add stats of equipment");
        }
    }

    public void ClosePopUpWindow()
    {

    }
}
