using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] private Transform slotsParent;
        [SerializeField] private List<BaseItemSlot> inventorySlots = new List<BaseItemSlot>();
        [SerializeField] private BaseItemSlot pointerSlot;
        [SerializeField] private bool isDraggingSlot = false;
        [SerializeField] private bool debugMode = false;
        [SerializeField] private ItemDatabase itemDatabase;

        // Private Variables

        // Properties
        public ItemDatabase ItemDatabase => itemDatabase;

        private void Awake()
        {
            foreach (Transform slot in slotsParent)
            {
                if (slot.name == "PointerSlot") continue;

                inventorySlots.Add(slot.GetComponent<BaseItemSlot>());
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i] != null)
                {
                    inventorySlots[i].SlotIndex = i;
                    inventorySlots[i].OnSlotPointerEntered += OnSlotPointerEntered;
                    inventorySlots[i].OnSlotPointerExited += OnSlotPointerExited;
                    inventorySlots[i].OnSlotPointerDown += OnSlotPointerDown;
                }
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
                AddItem(itemDatabase.GetItem("item_wooden_axe"));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddItem(itemDatabase.GetItem("item_wooden_shovel"));
            }
        }

        public bool AddItem(BaseItem itemToAdd)
        {
            if (itemToAdd == null) return false;

            foreach (BaseItemSlot slot in inventorySlots)
            {
                if (slot.ItemInSlot == null)
                {
                    slot.UpdateSlot(itemToAdd);
                    return true;
                }
            }

            return false;
        }

        public void AddItemToInventoryAtIndex(BaseItem item, int index)
        {
            BaseItemSlot slotAtIndex = inventorySlots[index];
            if (slotAtIndex != null)
            {
                slotAtIndex.UpdateSlot(item);
                pointerSlot.ClearSlot();
                return;
            }

            Debug.Log("Slot not found in inventory");
        }

        public void RemoveItem()
        {

        }

        private void OnSlotPointerEntered(BaseItemSlot baseSlot)
        {
            if (baseSlot == null) return;
        }

        private void OnSlotPointerExited(BaseItemSlot baseSlot)
        {
            if (baseSlot == null) return;
        }

        private void OnSlotPointerDown(BaseItemSlot baseSlot)
        {
            if (baseSlot == null) return;
        }

        public void SwapItemSlots(ItemSlot from, ItemSlot to)
        {
            ItemSlot fromTemp = from;

        }
    }
}