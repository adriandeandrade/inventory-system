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

    private string assetStoragePath = "Assets/Scripts/Inventory/Objects/Items";
    private string itemDatabaseStoragePath = "Assets/Scripts/Inventory/Objects";

    // Visual Tree Paths
    private string screen1TreePath = "ItemEditor_Main_Screen1";
    private string defaultItemTreePath = "ItemEditor_Default_Item_Screen";
    private string equipmentItemTreePath = "ItemEditor_Equipment_Item_Screen";
    private string foodItemTreePath = "ItemEditor_Food_Item_Screen";

    private ItemType workingItemType;
    private ItemDatabase itemDatabase;

    #region Item Data Variables

    class DefaultItemData
    {
        public string itemName;
        public string itemDescription;
        public int itemValue;
        public ItemType itemType;
        public RarityType rarityType;
        public Sprite itemIcon;

        public DefaultItemData(string itemName, string itemDescription, int itemValue, ItemType itemType, RarityType rarityType, Sprite itemIcon)
        {
            this.itemName = itemName;
            this.itemDescription = itemDescription;
            this.itemValue = itemValue;
            this.itemType = itemType;
            this.rarityType = rarityType;
            this.itemIcon = itemIcon;
        }
    }

    class EquipmentItemData : DefaultItemData
    {
        public EquipmentItemData(string itemName, string itemDescription, int itemValue, ItemType itemType, RarityType rarityType, Sprite itemIcon) : base(itemName, itemDescription, itemValue, itemType, rarityType, itemIcon)
        {
        }
    }

    class FoodItemData : DefaultItemData
    {
        public FoodItemData(string itemName, string itemDescription, int itemValue, ItemType itemType, RarityType rarityType, Sprite itemIcon) : base(itemName, itemDescription, itemValue, itemType, rarityType, itemIcon)
        {
        }
    }

    #endregion

    #region Unity Functions

    [MenuItem("Inventory Tools/Create New Item")]
    public static void ShowWindow()
    {
        var window = GetWindow<ItemEditor>();
        window.titleContent = new GUIContent("Item Editor");
        window.minSize = new Vector2(465f, 450f);
    }

    private void OnEnable()
    {
        SetStyleSheet();
        ShowStartingScreen();
        GetItemDatabase();
    }

    private void GetItemDatabase()
    {
        itemDatabase = AssetDatabase.LoadAssetAtPath($"{itemDatabaseStoragePath}/ItemDatabase.asset", typeof(ItemDatabase)) as ItemDatabase;
        if(itemDatabase == null)
        {
            Debug.Log("Failed to get item database");
        }
    }

    #endregion

    private void SetStyleSheet()
    {
        rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("ItemEditor_Style"));
    }

    private void ShowStartingScreen()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(screen1TreePath);
        visualTree.CloneTree(rootVisualElement);

        // Setup item type enum field
        itemTypeEnumScreen1 = rootVisualElement.Q(name: "item-type-enum-field") as EnumField;
        itemTypeEnumScreen1.Init(ItemType.Default);


        // Setup confirm button
        Button confirmButton = rootVisualElement.Q(name: "confirm-button-screen1") as Button;
        confirmButton.clickable.clicked += () => ShowItemCreationScreen();
    }

    private void ShowItemCreationScreen()
    {
        workingItemType = (ItemType)itemTypeEnumScreen1.value; // Store the value chosen in screen 1

        rootVisualElement.Clear(); // Clear the panel.

        switch (workingItemType)
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
    }

    #region Item Type Screen Functions
    private void ShowDefaultItemEditor()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(defaultItemTreePath);
        visualTree.CloneTree(rootVisualElement);

        // Setup default item editor screen
        Button createItemButton = rootVisualElement.Q(name: "base-item-data-create-button") as Button;
        TextField itemNameField = rootVisualElement.Q(name: "item-data-name") as TextField;
        IntegerField itemValueField = rootVisualElement.Q(name: "item-data-value") as IntegerField;
        TextField itemDescField = rootVisualElement.Q(name: "item-data-desc") as TextField;

        EnumField itemTypeField = rootVisualElement.Q(name: "item-data-type") as EnumField;
        itemTypeField.Init(workingItemType);
        Debug.Log(itemTypeField.childCount);

        EnumField itemRarityField = rootVisualElement.Q(name: "item-data-rarity") as EnumField;
        itemRarityField.Init(RarityType.Common);

        ObjectField itemIconField = rootVisualElement.Q(name: "item-data-icon") as ObjectField;
        itemIconField.objectType = typeof(Sprite); // Set the object fields type.

        if (createItemButton != null)
        {
            createItemButton.clickable.clicked += () =>
            {
                DefaultItemData itemData = new DefaultItemData(itemNameField.value, itemDescField.value, itemValueField.value, workingItemType, (RarityType)itemRarityField.value, (Sprite)itemIconField.value);
                CreateDefaultItem(itemData);
            };
        }
    }

    private void ShowEquipmentItemEditor()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(equipmentItemTreePath);
        visualTree.CloneTree(rootVisualElement);

        // Setup equipment item editor screen
        Button createItemButton = rootVisualElement.Q(name: "base-item-data-create-button") as Button;
        TextField itemNameField = rootVisualElement.Q(name: "item-data-name") as TextField;
        IntegerField itemValueField = rootVisualElement.Q(name: "item-data-value") as IntegerField;
        TextField itemDescField = rootVisualElement.Q(name: "item-data-desc") as TextField;

        EnumField itemTypeField = rootVisualElement.Q(name: "item-data-type") as EnumField;
        itemTypeField.Init(workingItemType);

        EnumField itemRarityField = rootVisualElement.Q(name: "item-data-rarity") as EnumField;
        itemRarityField.Init(RarityType.Common);

        ObjectField itemIconField = rootVisualElement.Q(name: "item-data-icon") as ObjectField;
        itemIconField.objectType = typeof(Sprite); // Set the object fields type.

        if (createItemButton != null)
        {
            createItemButton.clickable.clicked += () =>
            {
                EquipmentItemData itemData = new EquipmentItemData(itemNameField.value, itemDescField.value, itemValueField.value, workingItemType, (RarityType)itemRarityField.value, (Sprite)itemIconField.value);
                CreateEquipmentItem(itemData);
            };
        }
    }

    private void ShowFoodItemEditor()
    {
        rootVisualElement.Clear();

        var visualTree = Resources.Load<VisualTreeAsset>(foodItemTreePath);
        visualTree.CloneTree(rootVisualElement);

        // Setup food item editor screen
        Button createItemButton = rootVisualElement.Q(name: "base-item-data-create-button") as Button;
        TextField itemNameField = rootVisualElement.Q(name: "item-data-name") as TextField;
        IntegerField itemValueField = rootVisualElement.Q(name: "item-data-value") as IntegerField;
        TextField itemDescField = rootVisualElement.Q(name: "item-data-desc") as TextField;

        EnumField itemTypeField = rootVisualElement.Q(name: "item-data-type") as EnumField;
        itemTypeField.Init(workingItemType);

        EnumField itemRarityField = rootVisualElement.Q(name: "item-data-rarity") as EnumField;
        itemRarityField.Init(RarityType.Common);

        ObjectField itemIconField = rootVisualElement.Q(name: "item-data-icon") as ObjectField;
        itemIconField.objectType = typeof(Sprite); // Set the object fields type.

        if (createItemButton != null)
        {
            createItemButton.clickable.clicked += () =>
            {
                FoodItemData itemData = new FoodItemData(itemNameField.value, itemDescField.value, itemValueField.value, workingItemType, (RarityType)itemRarityField.value, (Sprite)itemIconField.value);
                CreateFoodItem(itemData);
            };
        }
    }

    #endregion

    #region Item Creation Functions

    private void CreateDefaultItem(DefaultItemData defaultItemData)
    {
        Item itemAsset = ScriptableObject.CreateInstance<Item>();
        string itemSlug = ConvertToSlug(defaultItemData.itemName);

        // TODO: If the item already exists in the item database, just cancel the whole process.

        //if(itemDatabase.CheckDatabaseContain(itemSlug))
        //{
        //    // Show pop up dialog Choices[Create new item, Close editor window]
        //    DestroyImmediate(itemAsset);
        //    Debug.Log("Item already exists in database");
        //    return;
        //}

        // Configure new scriptable object
        itemAsset.itemName = defaultItemData.itemName;
        itemAsset.itemSlug = itemSlug;
        itemAsset.itemDescription = defaultItemData.itemDescription;
        itemAsset.itemValue = defaultItemData.itemValue;
        itemAsset.itemType = defaultItemData.itemType;
        itemAsset.rarityType = defaultItemData.rarityType;
        itemAsset.itemIcon = defaultItemData.itemIcon;

        // Save new asset.
        string formattedName = defaultItemData.itemName;
        if (defaultItemData.itemName.Contains(" "))
            formattedName = defaultItemData.itemName.Replace(' ', '_');

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{assetStoragePath}/Item{formattedName}.asset");

        AssetDatabase.CreateAsset(itemAsset, assetPathAndName);
        AddNewItemToDatabase<Item>(itemAsset);
        AssetDatabase.Refresh();
        Selection.activeObject = itemAsset;
    }

    private void CreateEquipmentItem(EquipmentItemData equipmentData)
    {
        EquipmentItem itemAsset = ScriptableObject.CreateInstance<EquipmentItem>();
        string itemSlug = ConvertToSlug(equipmentData.itemName);

        // Configure new scriptable object
        itemAsset.itemName = equipmentData.itemName;
        itemAsset.itemSlug = itemSlug;
        itemAsset.itemDescription = equipmentData.itemDescription;
        itemAsset.itemValue = equipmentData.itemValue;
        itemAsset.itemType = equipmentData.itemType;
        itemAsset.rarityType = equipmentData.rarityType;
        itemAsset.itemIcon = equipmentData.itemIcon;

        // Save new asset.
        string formattedName = equipmentData.itemName;
        if (equipmentData.itemName.Contains(" "))
            formattedName = equipmentData.itemName.Replace(' ', '_');

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{assetStoragePath}/Item{formattedName}.asset");

        AssetDatabase.CreateAsset(itemAsset, assetPathAndName);
        AddNewItemToDatabase<EquipmentItem>(itemAsset);
        AssetDatabase.Refresh();
        Selection.activeObject = itemAsset;
    }

    private void CreateFoodItem(FoodItemData foodData)
    {
        FoodItem itemAsset = ScriptableObject.CreateInstance<FoodItem>();
        string itemSlug = ConvertToSlug(foodData.itemName);

        // Configure new scriptable object
        itemAsset.itemName = foodData.itemName;
        itemAsset.itemSlug = itemSlug;
        itemAsset.itemDescription = foodData.itemDescription;
        itemAsset.itemValue = foodData.itemValue;
        itemAsset.itemType = foodData.itemType;
        itemAsset.rarityType = foodData.rarityType;
        itemAsset.itemIcon = foodData.itemIcon;

        // Save new asset.
        string formattedName = foodData.itemName;
        if (foodData.itemName.Contains(" "))
            formattedName = foodData.itemName.Replace(' ', '_');

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{assetStoragePath}/Item{formattedName}.asset");

        AssetDatabase.CreateAsset(itemAsset, assetPathAndName);
        AddNewItemToDatabase<FoodItem>(itemAsset);
        AssetDatabase.Refresh();
        Selection.activeObject = itemAsset;
    }

    private void AddNewItemToDatabase<T>(T newItem) where T : BaseItem
    {
        // TODO: Add check here to make sure item doesnt already exist. ( Maybe add that to the ItemDatabase object instead? )
        itemDatabase.AddItemToDatabase(newItem);
    }

    #endregion

    private string ConvertToSlug(string _itemName)
    {
        string lowerCase = _itemName.ToLower();
        string converted = lowerCase.Replace(' ', '_');
        return $"item_{converted}";
    }
}
