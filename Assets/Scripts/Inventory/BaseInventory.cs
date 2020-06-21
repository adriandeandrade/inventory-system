using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Inventory
{
    public enum PanelCloseMethod { DISABLE, MINIMIZE }

    public abstract class BaseInventory : MonoBehaviour, IPointerDownHandler
    {
        [Header("Base Inventory Configuration")]
        [SerializeField] protected int slotAmount; // The amount of slots the inventory has.
        [SerializeField] protected PanelCloseMethod panelCloseMethod; // How the panel should close
        [SerializeField] protected ButtonController panelCloseButton;
        [SerializeField] protected GameObject slotPrefab;
        [SerializeField] protected Transform slotContainer; // This is what the slots are parented to when the inventory is generated.
        [SerializeField] protected BaseItemSlot pointerSlot; // Basic slot that stores what we select and drag in the inventory.

        [Header("Other Configuration")]
        [SerializeField] protected ItemDatabase itemDatabase; // TODO: Move this so it's a static class or singleton.
        [Space]
        [Space]
        [SerializeField] private bool enableOnPointerDown = true;
        [SerializeField] private bool enableOnPointerUp = true;
        [SerializeField] private bool enableOnPointerEnter = true;
        [SerializeField] private bool enableOnPointerExit = true;
        [SerializeField] private bool enableOnDrag = true;
        [SerializeField] private bool enableTooltip = true;

        // Private Variables
        protected BaseItemSlot[] inventorySlots; // The actual inventory slots.
        protected BaseItemSlot currentlySelectedSlot;

        // Components
        protected InventoryToolTip toolTip;

        protected virtual void Awake()
        {
            toolTip = FindObjectOfType<InventoryToolTip>();
            
            if(panelCloseButton != null)
            {
                panelCloseButton.OnClicked.AddListener(OnCloseButtonClicked);
            }
        }

        protected virtual void Start()
        {
            inventorySlots = new BaseItemSlot[slotAmount];
            TogglePointer(false);
            GenerateSlots();
        }

        #region Abstract Functions

        protected abstract void GenerateSlots();
        protected abstract void DisablePanel();
        protected abstract void MinimizePanel();

        #endregion

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
            switch(panelCloseMethod)
            {
                case PanelCloseMethod.DISABLE:
                    DisablePanel();
                    break;
                case PanelCloseMethod.MINIMIZE:
                    MinimizePanel();
                    break;
            }
        }


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

        public BaseItemSlot HasEmptySlot()
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if(!inventorySlots[i].ContainsItem)
                {
                    return inventorySlots[i];
                }
            }

            return null;
        }

        #endregion

        #region Pointer Event Callbacks

        protected virtual void OnPointerEnter(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (!enableOnPointerEnter) return;

            if (toolTip != null && enableTooltip) toolTip.UpdateToolTip(slot.ItemInSlot);

            slot.SetHovered();
        }

        protected virtual void OnPointerExit(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (!enableOnPointerExit) return;

            if (toolTip != null && enableTooltip) toolTip.UpdateToolTip(null);

            if (slot == currentlySelectedSlot)
            {
                SelectSlot(slot);
                return;
            }

            DeselectSlot(slot);
        }

        protected virtual void OnPointerDown(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (!enableOnPointerDown) return;
            if (!slot.ContainsItem) return;

            if(slot == null)
            {
                if (currentlySelectedSlot != null) currentlySelectedSlot.Deselect();
            }

            SelectSlot(slot);
        }
        protected virtual void OnPointerUp(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (!enableOnPointerUp) return;

            DeselectSlot(currentlySelectedSlot);
            SelectSlot(slot);
        }

        protected virtual void OnDrag(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (!enableOnDrag) return;

            if(pointerSlot != null)
            {
                pointerSlot.transform.position = Input.mousePosition;
            }
        }

        protected virtual void OnRightClick(BaseItemSlot slot, PointerEventData pointerData)
        {
            slot.OnSlotRightClicked(pointerData);
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


