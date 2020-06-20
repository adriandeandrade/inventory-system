using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Inventory;
using UnityEditor.UIElements;

public class DatabaseViewerEditor : EditorWindow
{
    private string databaseViewerPath = "ItemEditor_DatabaseView";
    private string objectFieldTemplatePath = "ItemEditor_ItemObject_Template";

    [MenuItem("Inventory Tools/Database Viewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<DatabaseViewerEditor>();
        window.titleContent = new GUIContent("Database Viewer");
        window.minSize = new Vector2(465f, 600f);
    }

    private void OnEnable()
    {
        rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("ItemEditor_DatabaseView_Style"));
        ItemDatabase itemDatabase = AssetDatabase.LoadAssetAtPath("Assets/Scripts/Inventory/Objects/ItemDatabase.asset", typeof(ItemDatabase)) as ItemDatabase;

        rootVisualElement.Clear();
        var visualTree = Resources.Load<VisualTreeAsset>(databaseViewerPath);
        visualTree.CloneTree(rootVisualElement);

        VisualElement fieldContainer = rootVisualElement.Q(name: "item-object-field-container");

        Foldout itemFoldOut = rootVisualElement.Q(name: "item-foldout") as Foldout;
        var objectFieldTemplate = Resources.Load<VisualTreeAsset>(objectFieldTemplatePath);

        foreach(BaseItem item in itemDatabase.items)
        {
            VisualElement itemObject = objectFieldTemplate.CloneTree();

            ObjectField itemObjectField = itemObject.Q(name: "item-object-field") as ObjectField;
            itemObjectField.objectType = typeof(BaseItem);
            itemObjectField.value = item;
            itemObjectField.label = item.itemName;

            itemFoldOut.Add(itemObject);
        }
    }
}
