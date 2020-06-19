using UnityEngine;

public static class ItemRarityColor
{
    public static Color32 CommonColor = new Color(0.6792453f, 0.6792453f, 0.6792453f, 1f);
    public static Color32 UnCommonColor = new Color(0.1886792f, 0.1886792f, 0.1886792f, 1f);
    public static Color32 RareColor = new Color(0.2042542f, 0.4091979f, 0.8490566f, 1f);
    public static Color32 EpicColor = new Color(0.8509804f, 0.2039215f, 0.7466668f, 1f);
    public static Color32 LegendaryColor = new Color(1f, 0.6341173f, 0f, 1f);
}

namespace Inventory
{
    
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

        public Color GetTitleColor()
        {
            switch (rarityType)
            {
                case RarityType.Common:
                    return ItemRarityColor.CommonColor;
                case RarityType.Uncommon:
                    return ItemRarityColor.UnCommonColor;
                case RarityType.Rare:
                    return ItemRarityColor.RareColor;
                case RarityType.Epic:
                    return ItemRarityColor.EpicColor;
                case RarityType.Legendary:
                    return ItemRarityColor.LegendaryColor;
                default:
                    return Color.white;
            }
        }
    }
}