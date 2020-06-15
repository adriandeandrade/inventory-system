using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public abstract class BaseItemSlot : MonoBehaviour
    {
        [SerializeField] protected BaseItem itemInSlot;
        [SerializeField] protected Image slotItemIcon;
        [SerializeField] protected Outline slotOutLine;
        [SerializeField] protected int slotIndex;

        // Properties
        public BaseItem ItemInSlot
        {
            get => itemInSlot;
            set => itemInSlot = value;
        }

        public Image SlotImage
        {
            get => slotItemIcon;
            set => slotItemIcon = value;
        }

        public int SlotIndex
        {
            get => slotIndex;
            set => slotIndex = value;
        }

        public bool ContainsItem => ItemInSlot != null;
        public bool Selected { get; set; } = false;

        // Private Variables
        protected RectTransform iconTransform;
        protected Vector3 iconStartPos;
        protected BaseItemSlot pointerSlot;

        // Events
        public Action<BaseItemSlot> OnSlotPointerEntered;
        public Action<BaseItemSlot> OnSlotPointerExited;
        public Action<BaseItemSlot> OnSlotPointerDown;

        #region Unity Functions
        private void Awake()
        {
            iconTransform = slotItemIcon.GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (itemInSlot == null)
            {
                slotItemIcon.color = Color.clear;
            }
        }

        #endregion

        public virtual void OnSlotRightClicked(PointerEventData eventData)
        {
            Debug.Log($"Item slot at index {slotIndex} was right clicked");
        }

        public void UpdateSlot(BaseItem item)
        {
            if (item == null)
            {
                ClearSlot();
                return;
            }

            itemInSlot = item;
            //slotItemIcon.color = item.debugSpriteColor; // TODO: Use sprite instead of color here.
            slotItemIcon.color = Color.white;
            slotItemIcon.sprite = item.itemIcon;
            //Debug.Log("Slot was updated");
        }

        public void Select(Color outlineColor)
        {
            Selected = true;
            SetOutlineColor(outlineColor);
        }

        public void Deselect()
        {
            Selected = false;
            slotOutLine.enabled = false;
        }

        public void ClearSlot()
        {
            slotItemIcon.color = Color.clear;
            ItemInSlot = null;
        }

        public void SetOutlineColor(Color newColor)
        {
            slotOutLine.enabled = true;
            slotOutLine.effectColor = newColor;
        }
    }
}