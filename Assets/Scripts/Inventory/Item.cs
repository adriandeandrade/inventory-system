using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items/Item")]
    public class Item : BaseItem
    {
        private void Awake()
        {
            itemType = ItemType.Default;
        }
    }
}
