using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : CustomBehaviour
{
    public InventoryObjectStats InventoryObjectStats;
    public CustomButton SlotButton;
    public RectTransform PopUpWindow;
    private bool isSlotButtonOpen = false;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SlotButton.Initialize(gameManager.UIManager, OnSlotButtonClicked);
    }

    private void OnSlotButtonClicked()
    {
        isSlotButtonOpen = !isSlotButtonOpen;
        //SlotButton.interactable = false;
        if (isSlotButtonOpen)
        {
            Debug.Log("Open Inventory");
            //PopUpWindow.SetActive(true);
            PopUpWindow.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            Debug.Log("Slot type: " + InventoryObjectStats.EquipmentType);
            Debug.Log("Slot button operatability: " + SlotButton.interactable);
        }
        else
        {
            //SlotButton.interactable = true;
            Debug.Log("Close Inventory");
            //PopUpWindow.SetActive(false);
            PopUpWindow.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuint);

            Debug.Log("Slot button operatability: " + SlotButton.interactable);
        }
    }

}
