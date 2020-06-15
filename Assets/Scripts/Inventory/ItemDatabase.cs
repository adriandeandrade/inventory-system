using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Databases/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<BaseItem> items = new List<BaseItem>();

        public BaseItem GetItem(string itemSlug)
        {
            foreach (BaseItem item in items)
            {
                if (item.itemSlug.Equals(itemSlug))
                {
                    BaseItem itemInstance = Instantiate(item);
                    return itemInstance;
                }
            }

            return null;
        }

        public BaseItem GetItemClone(BaseItem item)
        {
            BaseItem itemInstance = Instantiate(item);
            return itemInstance;
        }
    }
}