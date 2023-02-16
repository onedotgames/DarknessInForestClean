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
        var mng = GameManager.InventoryManager;
        Debug.Log("ClickWorks");
        if(InventoryObjectStat.EquipmentType == EquipmentType.Weapon)
        {
            Debug.Log("Should set def weapon here");
            GameManager.SkillManager.DefaultWeaponV2 = GameManager.SkillManager.AllWeaponsV2
                [(int)InventoryObjectStat.SkillSO.PoolerType];
            //GameManager.InventoryManager.GlobalDamageIncrease += InventoryObjectStat.Damage;
            mng.CurrentWeapon = this.InventoryObjectStat;
            mng.InventorySlots[0].Icon.sprite = this.InventoryObjectStat.Icon;
        }
        else
        {
            switch (InventoryObjectStat.ArmorType)
            {
                case ArmorType.Chest:
                    mng.CurrentChest = this.InventoryObjectStat;
                    mng.InventorySlots[(int)InventoryObjectStat.ArmorType +1].Icon.sprite = this.InventoryObjectStat.Icon;

                    break;
                case ArmorType.Boot:
                    mng.CurrentBoot = this.InventoryObjectStat;
                    mng.InventorySlots[(int)InventoryObjectStat.ArmorType +1].Icon.sprite = this.InventoryObjectStat.Icon;

                    break;
                case ArmorType.Glove:
                    mng.CurrentGlove = this.InventoryObjectStat;
                    mng.InventorySlots[(int)InventoryObjectStat.ArmorType +1].Icon.sprite = this.InventoryObjectStat.Icon;

                    break;
                case ArmorType.Helmet:
                    mng.CurrentHelmet = this.InventoryObjectStat;
                    mng.InventorySlots[(int)InventoryObjectStat.ArmorType +1].Icon.sprite = this.InventoryObjectStat.Icon;

                    break;
                case ArmorType.Jewel:
                    mng.CurrentJewel = this.InventoryObjectStat;
                    mng.InventorySlots[(int)InventoryObjectStat.ArmorType +1].Icon.sprite = this.InventoryObjectStat.Icon;

                    break;
            }
            Debug.Log("Add stats of equipment");
            //GameManager.InventoryManager.GlobalHealthIncrease += InventoryObjectStat.Health;
            //GameManager.InventoryManager.GlobalDamageReduction += InventoryObjectStat.DamageReduction;
            //GameManager.InventoryManager.GlobalSpeedIncrease += InventoryObjectStat.Speed;
        }
        mng.SetGlobalStats();
        GameManager.UIManager.GetPanel(Panels.Initial).GetComponent<InitialMenu>().SetEquipmentSectionBars();
        mng.OnPopUpCloseButtonClicked();
    }

    public void ClosePopUpWindow()
    {

    }
}
