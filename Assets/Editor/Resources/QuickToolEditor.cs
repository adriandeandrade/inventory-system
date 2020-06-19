using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class QuickToolEditor : EditorWindow
{
    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickToolEditor>();
        window.titleContent = new GUIContent("QuickTool");
        window.minSize = new Vector2(250f, 50f);
    }

    private void OnEnable()
    {
        // Reference to the root of the window.
        var root = rootVisualElement;

        // Associates a stylesheet to our root. Thanks to inheritance, all root's children will have access to it
        root.styleSheets.Add(Resources.Load<StyleSheet>("QuickTool_Style"));

        // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("QuickTool_Main");
        quickToolVisualTree.CloneTree(root);

        // Queries all the buttons (via type) in our root and passes them
        // in the SetupBox method;
        var toolButtons = root.Query<Button>();
        toolButtons.ForEach(SetupButton);
    }

    private void SetupButton(Button button)
    {
        // Reference to the VisualElement inside the button that serves as the button's icon.
        var buttonIcon = button.Q(className: "quicktool-button-icon");

        // Icon's path in our project.
        var iconPath = "Icons/" + button.parent.name + "-icon";

        // Loads the actual asset from the above path.
        var iconAsset = Resources.Load<Texture2D>(iconPath);

        // Applies the above asset as a background image for the icon;
        buttonIcon.style.backgroundImage = iconAsset;

        // Instantiates our primitive object on a left click
        button.clickable.clicked += () => Debug.Log("Editor button was clicked!");

        // Sets the tooltip to the button itself.
        button.tooltip = button.parent.name;
    }
}
