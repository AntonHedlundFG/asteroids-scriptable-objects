using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomAsteroidsInspector : EditorWindow
{
    private VisualElement asteroidsElement;
    private VisualElement shipElement;
    private VisualElement asteroidSettingsElement;

    [MenuItem("Tools/Asteroids Inspector")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<CustomAsteroidsInspector>();
        window.titleContent = new GUIContent("Asteroids");
    }

    public void CreateGUI()
    {
        asteroidsElement = new VisualElement();
        shipElement = new VisualElement();

        rootVisualElement.Add(GenerateRadioButtons());

        GenerateAsteroidGUI();
        rootVisualElement.Add(asteroidsElement);
        asteroidsElement.visible = false;

    }
    private GroupBox GenerateRadioButtons()
    {
        GroupBox radioButtons = new GroupBox("Tool choice:");

        RadioButton asteroidButton = new RadioButton("Asteroids");
        asteroidButton.RegisterValueChangedCallback(ShowAsteroidsGUI);
        radioButtons.Add(asteroidButton);

        RadioButton shipButton = new RadioButton("Ships");
        shipButton.RegisterValueChangedCallback(ShowShipGUI);
        radioButtons.Add(shipButton);

        return radioButtons;
    }

    public void ShowAsteroidsGUI(ChangeEvent<bool> evt) => asteroidsElement.visible = evt.newValue;
    public void ShowShipGUI(ChangeEvent<bool> evt) => shipElement.visible = evt.newValue;
    public void GenerateAsteroidGUI()
    {
        Label Header = new Label("Asteroid Settings");
        asteroidsElement.Add(Header);

        asteroidsElement.Add(DropdownOfAllAssetsOfType<AsteroidSettings>("Asteroid Settings Asset"));
    }



    //Creates a Dropdown field containing all assets of a specific ScriptableObject type.
    public static DropdownField DropdownOfAllAssetsOfType<T>(string header) where T : ScriptableObject
    {
        T[] assets = GetAssetsOfType<T>();
        List<string> choices = new List<string>();
        foreach (T asset in assets)
        {
            choices.Add(asset.name);
        }
        DropdownField dropdown = new DropdownField(header);
        dropdown.choices = choices;
        return dropdown;
    }

    //Returns a list of all asset instances of a specific ScriptableObject type.
    public static T[] GetAssetsOfType<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"t: {typeof(T).Name}");
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            a[i] = (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]), typeof(T));
        }
        return a;
    }
}
