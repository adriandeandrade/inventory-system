using Inventory;
using UnityEngine;

public static class InventoryUtils
{
    private static Color32 commonColor = new Color(176f, 176f, 176f, 255f);
    private static Color32 unCommonColor = new Color(256f, 126f, 126f, 255f);
    private static Color32 rareColor = new Color(47f, 106f, 255f, 255f);
    private static Color32 epicColor = new Color(255f, 45f, 233f, 255f);
    private static Color32 legendaryColor = new Color(255f, 128f, 66f, 255f);

    public static Color32 GetColorFromRarityType(RarityType rarityType)
    {
        switch (rarityType)
        {
            case RarityType.Common:
                return commonColor;
            case RarityType.Uncommon:
                return unCommonColor;
            case RarityType.Rare:
                return rareColor;
            case RarityType.Epic:
                return epicColor;
            case RarityType.Legendary:
                return legendaryColor;
            default:
                return Color.white;
        }
    }
}
