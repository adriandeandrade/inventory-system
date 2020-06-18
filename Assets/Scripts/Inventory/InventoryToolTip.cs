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

        SetTitleText(item);
        itemDescriptionText.SetText(item.itemDescription);
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(toolTipBackground);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void SetTitleText(BaseItem item)
    {
        itemNameText.SetText(item.itemName);
        itemNameText.color = item.GetTitleColor();
    }
}
