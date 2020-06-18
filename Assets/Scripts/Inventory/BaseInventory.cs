using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Inventory
{
    public enum InventoryCloseType { DISABLE, MINIMIZE }

    public abstract class BaseInventory : MonoBehaviour, IPointerDownHandler
    {
        [Header("Base Inventory UI Configuration")]
        [SerializeField] protected BaseInventoryData inventoryData; // Starting data for the inventory. TODO: Look at base inventory data. Only contains inventory size now.
        [SerializeField] protected ItemDatabase itemDatabase; // TODO: Move this so it's a static class or singleton.

        [Space]
        [SerializeField] protected InventoryCloseType inventoryCloseType;
        [SerializeField] protected ButtonController minimizeButton;

        [Header("Inventory Slot Configuration")]
        [SerializeField] protected BaseItemSlot[] inventorySlots; // The slots that belong to this inventory.
        [SerializeField] protected GameObject slotPrefab;
        [SerializeField] protected Transform slotParent; // This is what the slots are parented to when the inventory is generated.
        [Space]
        [SerializeField] protected BaseItemSlot pointerSlot; // Basic slot that stores what we select and drag in the inventory.
        [SerializeField] protected BaseItemSlot currentlySelectedSlot;

        [Header("Slot Outline Colors")]
        [SerializeField] protected Color slotHoverColor;
        [SerializeField] protected Color slotSelectedColor;
        [SerializeField] protected Color slotSelectedColor1;

        // Private Variables

        // Components
        protected InventoryToolTip toolTip;

        protected virtual void Awake()
        {
            toolTip = FindObjectOfType<InventoryToolTip>();
            
            if(minimizeButton != null)
            {
                minimizeButton.OnClicked.AddListener(OnCloseButtonClicked);
            }
        }

        protected virtual void Start()
        {
            inventorySlots = new BaseItemSlot[inventoryData.inventorySize];
            TogglePointer(false);
            GenerateSlots();
        }

        protected abstract void GenerateSlots();

        protected void RegisterSlotEvent(GameObject slotObject, EventTriggerType eventType, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = slotObject.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger = slotObject.AddComponent<EventTrigger>();
            }

            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = eventType;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        protected void OnCloseButtonClicked()
        {
            switch(inventoryCloseType)
            {
                case InventoryCloseType.DISABLE:
                    InventoryDisable();
                    break;
                case InventoryCloseType.MINIMIZE:
                    InventoryMinimize();
                    break;
            }
        }

        protected abstract void InventoryDisable();
        protected abstract void InventoryMinimize();

        #region Inventory Functions

        public abstract bool AddItem(BaseItem itemToAdd);
        protected virtual void SwapItemsInSlot(BaseItemSlot fromSlot, BaseItemSlot toSlot)
        {

        }

        public virtual void UpdateInventory()
        {
            foreach (BaseItemSlot slot in inventorySlots)
            {
                slot.UpdateSlot(slot.ItemInSlot);
            }
        }

        #endregion

        #region Pointer Event Callbacks

        protected virtual void OnPointerEnter(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (toolTip != null) toolTip.UpdateToolTip(slot.ItemInSlot);

            slot.SetHovered();
        }

        protected virtual void OnPointerExit(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (toolTip != null) toolTip.UpdateToolTip(null);

            if (slot == currentlySelectedSlot)
            {
                slot.Select();
                return;
            }

            slot.Deselect();
        }

        protected virtual void OnPointerDown(BaseItemSlot slot, PointerEventData pointerData)
        {
            if(slot == null)
            {
                if (currentlySelectedSlot != null) currentlySelectedSlot.Deselect();
            }

            SelectSlot(slot);
        }
        protected virtual void OnPointerUp(BaseItemSlot slot, PointerEventData pointerData)
        {
            DeselectSlot(currentlySelectedSlot);
            SelectSlot(slot);
        }

        protected virtual void OnDrag(BaseItemSlot slot, PointerEventData pointerData)
        {
            if(pointerSlot != null)
            {
                pointerSlot.transform.position = Input.mousePosition;
            }
        }

        protected virtual void OnRightClick(BaseItemSlot slot, PointerEventData pointerData)
        {

        }

        #endregion

        protected void SelectSlot(BaseItemSlot slot)
        {
            if (slot != null)
            {
                DeselectSlot(currentlySelectedSlot);
                currentlySelectedSlot = slot;
                slot.Select();
            }
        }

        protected void DeselectSlot(BaseItemSlot slot)
        {
            if(slot != null)
            {
                slot.Deselect();
            }
        }

        #region Pointer Functions

        /// <summary>
        /// Updates the pointer item with the data from the slot passed in as a parameter.
        /// </summary>
        /// <param name="slot"></param>
        protected virtual void UpdatePointerItem(BaseItemSlot slot)
        {
            if (slot == null)
            {
                pointerSlot.ClearSlot();
                TogglePointer(false);
                return;
            }

            if (slot.ContainsItem)
            {
                pointerSlot.UpdateSlot(slot.ItemInSlot);
                TogglePointer(true);
            }
        }

        /// <summary>
        /// Toggles the pointer slot.
        /// </summary>
        /// <param name="toggle">On or off (true or false)</param>
        public void TogglePointer(bool toggle)
        {
            pointerSlot.transform.position = Input.mousePosition;
            pointerSlot.gameObject.SetActive(toggle);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            BaseItemSlot slotUnderMouse = eventData.pointerCurrentRaycast.gameObject.GetComponent<BaseItemSlot>();

            if(slotUnderMouse == null)
            {
                if(currentlySelectedSlot != null)
                {
                    currentlySelectedSlot.Deselect();
                }
            }
        }
        #endregion


    }
}


