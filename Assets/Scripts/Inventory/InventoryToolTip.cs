using Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemValueText;
    [SerializeField] private RectTransform toolTipBackground;

    [SerializeField] private Color commonColor;
    [SerializeField] private Color unCommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdateToolTip(BaseItem item)
    {
        gameObject.SetActive(false);

        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }

        SetTitleText(item.itemName, item.rarityType);
        itemDescriptionText.SetText(item.itemDescription);
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(toolTipBackground);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void SetTitleText(string newText, RarityType itemRarityType)
    {
        itemNameText.SetText(newText);

        switch(itemRarityType)
        {
            case RarityType.Common:
                itemNameText.color = commonColor;
                break;
            case RarityType.Uncommon:
                itemNameText.color = unCommonColor;
                break;
            case RarityType.Rare:
                itemNameText.color = rareColor;
                break;
            case RarityType.Epic:
                itemNameText.color = epicColor;
                break;
            case RarityType.Legendary:
                itemNameText.color = legendaryColor;
                break;

            default:
                itemNameText.color = Color.white;
                break;
        }
    }
}
