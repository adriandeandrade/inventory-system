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
        protected BaseItemSlot pointerSlot;
        protected Vector3 iconStartPos;
        protected ButtonController buttonController;

        #region Unity Functions
        private void Awake()
        {
            iconTransform = slotItemIcon.GetComponent<RectTransform>();
            buttonController = GetComponent<ButtonController>();
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
            slotItemIcon.color = Color.white;
            slotItemIcon.sprite = item.itemIcon;
        }

        public void Select()
        {
            Selected = true;
            buttonController.SetSelectedSprite();
        }

        public void Deselect()
        {
            Selected = false;
            buttonController.SetOriginalSprite();
        }

        public void SetHovered()
        {
            buttonController.SetHoveredSprite();
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