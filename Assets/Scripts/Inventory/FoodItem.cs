using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Food Item", menuName = "Inventory/Items/Food Item")]
    public class FoodItem : BaseItem
    {
        private void Awake()
        {
            itemType = ItemType.Food;
        }
    }
}
