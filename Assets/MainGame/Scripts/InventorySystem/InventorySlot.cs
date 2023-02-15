using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : CustomBehaviour
{
    public InventoryObjectStats InventoryObjectStats;
    public InventorySlotType InventorySlotType;
    public CustomButton SlotButton;
    public RectTransform PopUpWindow;
    private bool isSlotButtonOpen = false;
    public List<InventoryObjectStats> IdenticalEquipmentList;
    public SpriteRenderer player;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SlotButton.Initialize(gameManager.UIManager, OnSlotButtonClicked);
        GetListAccordingToType();
    }

    private void GetListAccordingToType()
    {
        switch (InventorySlotType)
        {
            case InventorySlotType.Weapon:
                IdenticalEquipmentList = GameManager.InventoryManager.Weapons;
                break;
            case InventorySlotType.Chest:
                IdenticalEquipmentList = GameManager.InventoryManager.Chests;
                break;
            case InventorySlotType.Glove:
                IdenticalEquipmentList = GameManager.InventoryManager.Gloves;
                break;
            case InventorySlotType.Boot:
                IdenticalEquipmentList = GameManager.InventoryManager.Boots;
                break;
            case InventorySlotType.Helmet:
                IdenticalEquipmentList = GameManager.InventoryManager.Helmets;
                break;
            case InventorySlotType.Jewelry:
                IdenticalEquipmentList = GameManager.InventoryManager.Jewelry;
                break;
        }
    }

    private void OnSlotButtonClicked()
    {

        //isSlotButtonOpen = !isSlotButtonOpen;
        ////SlotButton.interactable = false;
        //if (isSlotButtonOpen)
        //{
        //    Debug.Log("Open Inventory");
        //    //PopUpWindow.SetActive(true);
        //    GameManager.InventoryManager.OpenSlots(IdenticalEquipmentList.Count,IdenticalEquipmentList); // this value should replaced by item count
        //    PopUpWindow.DOScale(Vector3.one *1.8f, 0.5f).SetEase(Ease.OutBounce);            
        //    Debug.Log("Slot type: " + InventoryObjectStats.EquipmentType);
        //    Debug.Log("Slot button operatability: " + SlotButton.interactable);
        //}
        //else
        //{
        //    //SlotButton.interactable = true;
        //    Debug.Log("Close Inventory");
        //    //PopUpWindow.SetActive(false);
        //    PopUpWindow.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuint);

        //    Debug.Log("Slot button operatability: " + SlotButton.interactable);
        //}

        if(player.enabled == true)
        {
            player.enabled = false;
        }

        GameManager.InventoryManager.OpenSlots(IdenticalEquipmentList.Count, IdenticalEquipmentList); // this value should replaced by item count
        PopUpWindow.DOScale(Vector3.one * 1.8f, 0.5f).SetEase(Ease.OutBounce);
    }

}
