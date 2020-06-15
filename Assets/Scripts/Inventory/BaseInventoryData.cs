using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    
    public abstract class BaseInventoryData : ScriptableObject
    {
        [Range(5, 25)] public int inventorySize;

    }
}