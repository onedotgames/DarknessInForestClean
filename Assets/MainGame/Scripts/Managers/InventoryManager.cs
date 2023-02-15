using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryManager : CustomBehaviour
{
    public List<InventorySlot> InventorySlots;
    public List<PopUpSlot> PopUpSlots;
    public Sprite DefaultLockedPopUpSlotSprite;
    public Sprite PlaceHolderSprite;
    public CustomButton PopUpCloseButton;
    public RectTransform PopUpWindow;
    public SpriteRenderer player;

    [Space(10)]
    [Title("Weapons List")]
    public List<InventoryObjectStats> Weapons;

    [Space(10)]
    [Title("Chest Armor List")]
    public List<InventoryObjectStats> Chests;

    [Space(10)]
    [Title("Glove Armor List")]
    public List<InventoryObjectStats> Gloves;

    [Space(10)]
    [Title("Boot Armor List")]
    public List<InventoryObjectStats> Boots;

    [Space(10)]
    [Title("Helmet Armor List")]
    public List<InventoryObjectStats> Helmets;

    [Space(10)]
    [Title("Ammunation List")]
    public List<InventoryObjectStats> Jewelry;


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        InitializeSlots();
        PopUpCloseButton.Initialize(gameManager.UIManager, OnPopUpCloseButtonClicked);
    }

    private void InitializeSlots()
    {
        InventorySlots.ForEach(x => x.Initialize(GameManager));
    }

    private void OnPopUpCloseButtonClicked()
    {
        PopUpWindow.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuint).OnComplete(() => player.enabled = true);
        InventorySlots.ForEach(x => x.SlotButton.interactable = true);
    }

    public void OpenSlots(int value, List<InventoryObjectStats> objectStats)
    {
        Debug.Log("There is " + PopUpSlots.Count + " Pop Up Slot");
        InventorySlots.ForEach(x => x.SlotButton.interactable = false);
        for (int i = 0; i < PopUpSlots.Count; i++)
        {
            if (i < value)
            {
                Debug.Log(i+  ". PopUpSlot must be " + objectStats[i].name);

                PopUpSlots[i].ChangeSlotImage(objectStats[i].Icon);
            }
            else
            {
                Debug.Log(i + ". PopUpSlot must be " + DefaultLockedPopUpSlotSprite.name);

                PopUpSlots[i].ChangeSlotImage(DefaultLockedPopUpSlotSprite);
            }

        }
    }
}
