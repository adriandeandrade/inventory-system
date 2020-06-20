using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Databases/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<BaseItem> items = new List<BaseItem>();

        public bool AddItemToDatabase(BaseItem itemToAdd)
        {
            if(itemToAdd == null)
            {
                Debug.LogError($"Failed to add item to database.");
                return false;
            }

            if (items.Contains(itemToAdd))
            {
                Debug.LogError($"{itemToAdd.itemName} already exists!");
                return false;
            }

            Debug.LogError($"{itemToAdd.name} was <color=green>successfully</color> added to the database.");
            items.Add(itemToAdd);
            return true;
        }

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