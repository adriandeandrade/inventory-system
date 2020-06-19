using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;

public class ItemEditor : EditorWindow
{
    // Screen 1 Variables
    private EnumField itemTypeEnumScreen1;
    private Button continueButton;

    // Visual Tree Paths
    private string screen1TreePath = "ItemEditor_Main_Screen1";
    private string screen2TreePath = "ItemEditor_Main_Screen2";
    private string defaultItemTreePath = "ItemEditor_Default_Item_Screen";
    private string equipmentItemTreePath = "ItemEditor_Equipment_Item_Screen";
    private string foodItemTreePath = "ItemEditor_Food_Item_Screen";

    private ItemType workingItemType;

    #region Unity Functions

    [MenuItem("Inventory Tools/Create New Item")]
    public static void ShowWindow()
    {
        var window = GetWindow<ItemEditor>();
        window.titleContent = new GUIContent("Item Editor");
        window.minSize = new Vector2(465, 450f);
    }

    private void OnEnable()
    {
        SetStyleSheet();
        ShowScreen1();
    }

    #endregion

    private void SetStyleSheet()
    {
        rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("ItemEditor_Style"));
    }

    private void ShowScreen1()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(screen1TreePath);
        visualTree.CloneTree(rootVisualElement);

        // Setup item type enum field
        itemTypeEnumScreen1 = rootVisualElement.Q(name: "item-type-enum-field") as EnumField;
        itemTypeEnumScreen1.Init(ItemType.Default);
        

        // Setup confirm button
        Button confirmButton = rootVisualElement.Q(name: "confirm-button-screen1") as Button;
        confirmButton.clickable.clicked += () => ShowScreen2();
    }

    private void ShowScreen2()
    {
        workingItemType = (ItemType)itemTypeEnumScreen1.value; // Store the value chosen in screen 1

        rootVisualElement.Clear(); // Clear the panel.

        switch(workingItemType)
        {
            case ItemType.Default:
                ShowDefaultItemEditor();
                break;

            case ItemType.Equipment:
                ShowEquipmentItemEditor();
                break;

            case ItemType.Food:
                ShowFoodItemEditor();
                break;
        }

        //var visualTree = Resources.Load<VisualTreeAsset>(screen2TreePath); // Load screen 2.
        //visualTree.CloneTree(rootVisualElement);
    }

    #region Item Type Screen Functions
    private void ShowDefaultItemEditor()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(defaultItemTreePath);
        visualTree.CloneTree(rootVisualElement);
    }

    private void ShowEquipmentItemEditor()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(equipmentItemTreePath);
        visualTree.CloneTree(rootVisualElement);
    }

    private void ShowFoodItemEditor()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(foodItemTreePath);
        visualTree.CloneTree(rootVisualElement);
    }

    #endregion
}
