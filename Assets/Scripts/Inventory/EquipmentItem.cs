using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Items/Equipment Item")]
    public class EquipmentItem : BaseItem
    {
        private void Awake()
        {
            itemType = ItemType.Equipment;
        }
    }
}
