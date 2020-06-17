using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopInventory : BaseInventory
{
    [Header("Shop Inventory UI Configuration")]
    [SerializeField] private GameObject shopUI;

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
        if(customer != null)
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
        if (inventoryData == null)
        {
            Debug.LogError($"Inventory object is null. Make sure you specified an inventory data object.");

            return;
        }

        for (int i = 0; i < inventoryData.inventorySize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, slotParent);
            slotInstance.name = $"Slot{i}";

            BaseItemSlot baseSlot = slotInstance.GetComponent<BaseItemSlot>();
            baseSlot.SlotIndex = i;

            // Register slot events
            RegisterSlotEvent(slotInstance, EventTriggerType.PointerDown, (x) => { OnPointerDown(baseSlot, x as PointerEventData); });
            RegisterSlotEvent(slotInstance, EventTriggerType.PointerEnter, (x) => { OnPointerEnter(baseSlot, x as PointerEventData); });
            RegisterSlotEvent(slotInstance, EventTriggerType.PointerExit, (x) => { OnPointerExit(baseSlot, x as PointerEventData); });

            inventorySlots[i] = baseSlot;
        }

        pointerSlot.transform.SetSiblingIndex(slotParent.childCount - 1);
    }

    private void GenerateRandomItems()
    {
        int itemCount = 10;

        for(int i =0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, inventorySlots.Length - 1);
            inventorySlots[randomIndex].UpdateSlot(itemDatabase.GetItemClone(itemDatabase.items[Random.Range(0, itemDatabase.items.Count)]));
        }
    }

    protected override void OnRightClick(BaseItemSlot slot, PointerEventData pointerData)
    {
        BuyItem(slot);
    }

    #region Pointer Event Callbacks

    protected override void OnPointerDown(BaseItemSlot slot, PointerEventData pointerData)
    {
        Debug.Log(slot.ItemInSlot.itemValue);
    }

    #endregion

    #region Shop Functions

    public void BuyItem(BaseItemSlot slot)
    {

    }

    protected override void InventoryDisable()
    {
        CloseShop();
    }

    protected override void InventoryMinimize()
    {
        Debug.Log("Shop minimized");
    }

    #endregion


}
