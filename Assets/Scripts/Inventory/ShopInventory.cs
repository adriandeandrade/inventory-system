using Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopInventory : BaseInventory
{
    [Header("Shop Inventory UI Configuration")]
    [SerializeField] private GameObject shopUI;
    [Space]
    [SerializeField] private Image displayItemImage;
    [SerializeField] private TextMeshProUGUI displayItemDescText;
    [SerializeField] private TextMeshProUGUI displayItemNameText;
    [SerializeField] private ButtonController buyButton;

    // TODO: Seperate this stuff into a seperate ShopInteractable class.
    private PlayerInventory customer;
    private Interactable shopInteractable;

    protected override void Awake()
    {
        base.Awake();
        shopInteractable = GetComponent<Interactable>();
        shopInteractable.OnInteracted += OpenShop;
        shopInteractable.OnInteractionOutOfRange += CloseShop;
    }

    protected override void Start()
    {
        base.Start();
        GenerateRandomItems();
        DisableBuyButton();
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
    }

    public void InitializeShop(PlayerInventory _customer)
    {
        customer = _customer;
        customer.IsShopping = true;

        shopUI.SetActive(true);
    }

    public void CloseShop()
    {
        if (customer != null)
        {
            customer.IsShopping = false;
            customer = null;
        }

        shopUI.SetActive(false);
    }

    public override bool AddItem(BaseItem itemToAdd)
    {
        Debug.Log("Added new item to shop.");
        return true;
    }

    protected override void GenerateSlots()
    {
        //if (inventoryData == null)
        //{
        //    Debug.LogError($"Inventory object is null. Make sure you specified an inventory data object.");

        //    return;
        //}

        for (int i = 0; i < slotAmount; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, slotContainer);
            slotInstance.name = $"Slot{i}";

            BaseItemSlot baseSlot = slotInstance.GetComponent<BaseItemSlot>();
            baseSlot.SlotIndex = i;

            // Register slot events
            RegisterSlotEvent(slotInstance, EventTriggerType.PointerDown, (x) => { OnPointerDown(baseSlot, x as PointerEventData); });
            RegisterSlotEvent(slotInstance, EventTriggerType.PointerEnter, (x) => { OnPointerEnter(baseSlot, x as PointerEventData); });
            RegisterSlotEvent(slotInstance, EventTriggerType.PointerExit, (x) => { OnPointerExit(baseSlot, x as PointerEventData); });

            inventorySlots[i] = baseSlot;
        }

        pointerSlot.transform.SetSiblingIndex(slotContainer.childCount - 1);
    }

    private void GenerateRandomItems()
    {
        int itemCount = 10;

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, inventorySlots.Length - 1);
            inventorySlots[randomIndex].UpdateSlot(itemDatabase.GetItemClone(itemDatabase.items[Random.Range(0, itemDatabase.items.Count)]));
        }
    }

    protected override void OnRightClick(BaseItemSlot slot, PointerEventData pointerData)
    {
        Debug.Log("Slot right clicked.");
    }

    #region Pointer Event Callbacks

    protected override void OnPointerDown(BaseItemSlot slot, PointerEventData pointerData)
    {
        base.OnPointerDown(slot, pointerData);

        if(!slot.ContainsItem)
        {
            currentlySelectedSlot = null;
            UpdateInventory();
            return;
        }

        UpdateInventory();
    }

    #endregion

    #region Shop Functions

    public void BuyItem(BaseItemSlot slot)
    {

    }

    protected override void DisablePanel()
    {
        CloseShop();
    }

    protected override void MinimizePanel()
    {
        Debug.Log("Shop minimized");
    }

    #endregion

    #region UI Functions

    public override void UpdateInventory()
    {
        base.UpdateInventory();
        UpdateDisplayPanel();

        if (currentlySelectedSlot != null)
        {
            EnableBuyButton();
        }

    }
    private void UpdateDisplayPanel()
    {
        if (currentlySelectedSlot == null)
        {
            ResetDisplayPanel();
            return;
        }

        SetItemDisplayImage(currentlySelectedSlot.ItemInSlot.itemIcon);
        SetItemDescriptionText(currentlySelectedSlot.ItemInSlot.itemDescription);
        SetItemNameText(currentlySelectedSlot.ItemInSlot.itemName);
    }

    private void SetItemNameText(string itemName)
    {
        if (displayItemNameText != null)
        {
            displayItemNameText.SetText(itemName);

            if (currentlySelectedSlot != null)
                displayItemDescText.color = currentlySelectedSlot.ItemInSlot.GetTitleColor();
        }
        else
            Debug.LogError("Display item name text not found. Make sure it's referenced in the inspector!");
    }

    private void SetItemDescriptionText(string newDescription)
    {
        if (displayItemDescText != null)
        {
            displayItemDescText.SetText(newDescription);
        }
        else
            Debug.LogError("Display item description text not found. Make sure it's referenced in the inspector!");
    }

    private void SetItemDisplayImage(Sprite newItem = null)
    {
        if(newItem == null)
        {
            displayItemImage.sprite = null;
            displayItemImage.color = Color.clear;
            return;
        }

        if (displayItemImage != null)
        {
            displayItemImage.sprite = newItem;
            displayItemImage.color = Color.white;
        }
        else
        {
            Debug.LogError("Display item image not found. Make sure it's referenced in the inspector!");
        }
    }

    private void EnableBuyButton()
    {
        buyButton.Enable();
    }

    private void DisableBuyButton()
    {
        buyButton.Disable();
    }

    private void ResetDisplayPanel()
    {
        SetItemNameText("");
        SetItemDescriptionText("");
        SetItemDisplayImage();
        DisableBuyButton();
    }

    #endregion
}
