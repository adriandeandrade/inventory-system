using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Player Inventory Object", menuName = "Inventory/Player Inventory Object")]
    public class PlayerInventoryData : BaseInventoryData
    {
        public bool startWithDefault;
        public List<BaseItem> defaultInventoryItems;
    }
}

