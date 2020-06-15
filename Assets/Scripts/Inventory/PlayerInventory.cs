using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class PlayerInventory : BaseInventory
    {
        private PlayerInventoryData playerInventoryData;
        private bool isShopping = false;
        public bool IsShopping { get => isShopping; set => isShopping = value; }

        protected override void Start()
        {
            base.Start();
            playerInventoryData = inventoryData as PlayerInventoryData;


            if (playerInventoryData.startWithDefault)
            {
                PopulateDefaultInventory();
            }
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
                RegisterSlotEvent(slotInstance, EventTriggerType.PointerUp, (x) => { OnPointerUp(baseSlot, x as PointerEventData); });
                RegisterSlotEvent(slotInstance, EventTriggerType.PointerEnter, (x) => { OnPointerEnter(baseSlot, x as PointerEventData); });
                RegisterSlotEvent(slotInstance, EventTriggerType.PointerExit, (x) => { OnPointerExit(baseSlot, x as PointerEventData); });
                RegisterSlotEvent(slotInstance, EventTriggerType.Drag, (x) => { OnDrag(baseSlot, x as PointerEventData); });

                inventorySlots[i] = baseSlot;
            }

            pointerSlot.transform.SetSiblingIndex(slotParent.childCount - 1); // Sets the pointer slot at the bottom of the hierarchy so it renders infront of everything.
        }

        private void PopulateDefaultInventory()
        {
            if (playerInventoryData.defaultInventoryItems.Count > inventoryData.inventorySize) return;

            foreach (BaseItem item in playerInventoryData.defaultInventoryItems)
            {
                AddItem(item);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddItem(itemDatabase.GetItem("item_wooden_sword"));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddItem(itemDatabase.GetItem("item_gold_ring"));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddItem(itemDatabase.GetItem("item_red_carrot"));
            }
        }

        #region Inventory Functions

        public override bool AddItem(BaseItem itemToAdd)
        {
            if (itemToAdd == null) return false;

            foreach (BaseItemSlot slot in inventorySlots)
            {
                if (!slot.ContainsItem)
                {
                    slot.UpdateSlot(itemToAdd);
                    return true;
                }
            }

            return false;
        }

        protected override void SwapItemsInSlot(BaseItemSlot fromSlot, BaseItemSlot toSlot)
        {
            BaseItem itemInSlot = inventorySlots[toSlot.SlotIndex].ItemInSlot;

            inventorySlots[toSlot.SlotIndex].ItemInSlot = fromSlot.ItemInSlot;
            inventorySlots[fromSlot.SlotIndex].ItemInSlot = itemInSlot;

            DeselectSlot(currentlySelectedSlot);
            SelectSlot(toSlot);

            UpdateSlots();

            Debug.Log("Items swapped");
        }

        #endregion

        #region Pointer Event Callbacks

        protected override void OnPointerEnter(BaseItemSlot slot, PointerEventData pointerData)
        {
            base.OnPointerEnter(slot, pointerData);
        }

        protected override void OnPointerExit(BaseItemSlot slot, PointerEventData pointerData)
        {
            base.OnPointerExit(slot, pointerData);
        }

        protected override void OnPointerDown(BaseItemSlot slot, PointerEventData pointerData)
        {
            base.OnPointerDown(slot, pointerData);

            if (!slot.ContainsItem) return;
            if (isShopping) return;

            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick(slot, pointerData);
                return;
            }

            UpdatePointerItem(slot);
        }

        protected override void OnPointerUp(BaseItemSlot slot, PointerEventData pointerData)
        {
            BaseItemSlot slotHoveredOver = null;
            if (isShopping) return;


            if (pointerData.pointerCurrentRaycast.gameObject != null)
            {
                slotHoveredOver = pointerData.pointerCurrentRaycast.gameObject.GetComponent<BaseItemSlot>();
            }

            if (slotHoveredOver != null) // if what we hovered over is indeed a slot.
            {
                base.OnPointerUp(slotHoveredOver, pointerData);

                if (slotHoveredOver.ContainsItem)
                {
                    SwapItemsInSlot(slot, slotHoveredOver);
                    UpdatePointerItem(null); // Hide pointer slot
                    toolTip.UpdateToolTip(slotHoveredOver.ItemInSlot); // Update tooltip to new hovered

                    Debug.Log($"Pointer Up over slot #{slotHoveredOver.SlotIndex}");
                }
                else
                {
                    slotHoveredOver.UpdateSlot(slot.ItemInSlot);
                    toolTip.UpdateToolTip(slotHoveredOver.ItemInSlot);
                    slot.ClearSlot();
                    UpdatePointerItem(null);
                }
            }
            else
            {
                // If pointer dropped above something other than a slot.
                UpdatePointerItem(null);

                Debug.Log($"Slot not found, resetting.");
            }
        }

        protected override void OnDrag(BaseItemSlot slot, PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right) return;
            if (isShopping) return;
            base.OnDrag(slot, pointerData);
        }

        protected override void OnRightClick(BaseItemSlot slot, PointerEventData pointerData)
        {
            slot.OnSlotRightClicked(pointerData);
        }

        #endregion
    }
}