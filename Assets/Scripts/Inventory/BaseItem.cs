using UnityEngine;

namespace Inventory
{
    public enum ItemType { Default, Equipment, Food }
    public enum RarityType { Common, Uncommon, Rare, Epic, Legendary }
    public abstract class BaseItem : ScriptableObject
    {
        [Header("Base Item Configuration")]
        public string itemName;
        public string itemSlug;
        public int itemValue;
        [TextArea(5, 25)] public string itemDescription;
        public ItemType itemType;
        public RarityType rarityType;
        public Sprite itemIcon;
    }
}