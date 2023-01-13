using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomAsteroidsInspector : EditorWindow
{
    private VisualElement _asteroidsRoot;
    private VisualElement _shipRoot;

    private VisualElement _asteroidSettingRoot;
    private DropdownField _asteroidSettingDropdown;

    [MenuItem("Tools/Asteroids Inspector")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<CustomAsteroidsInspector>();
        window.titleContent = new GUIContent("Asteroids");
    }

    public void CreateGUI()
    {
        _asteroidsRoot = new VisualElement();
        _shipRoot = new VisualElement();

        rootVisualElement.Add(GenerateRadioButtons());

        GenerateAsteroidGUI();
        rootVisualElement.Add(_asteroidsRoot);
        _asteroidsRoot.visible = false;

        //TODO make a GenerateShipGUI() method etc.

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

    public void ShowAsteroidsGUI(ChangeEvent<bool> evt) => _asteroidsRoot.visible = evt.newValue;
    public void ShowShipGUI(ChangeEvent<bool> evt) => _shipRoot.visible = evt.newValue;
    public void GenerateAsteroidGUI()
    {
        Label Header = new Label("Asteroid Settings");
        _asteroidsRoot.Add(Header);

        _asteroidSettingDropdown = DropdownOfAllAssetsOfType<AsteroidSettings>("Asteroid Settings Asset");
        _asteroidsRoot.Add(_asteroidSettingDropdown);
        _asteroidSettingDropdown.RegisterValueChangedCallback(ChooseAsteroidSetting);
    }
    public void ChooseAsteroidSetting(ChangeEvent<string> evt)
    {
        if (_asteroidSettingDropdown.index < 0) { return; }
        if (_asteroidSettingRoot != null) { _asteroidsRoot.Remove(_asteroidSettingRoot); }

        AsteroidSettings[] allSettings = GetAssetsOfType<AsteroidSettings>();
        AsteroidSettings currentSetting = allSettings[_asteroidSettingDropdown.index];

        _asteroidSettingRoot = new VisualElement();
        
        //HERE IS WHERE I SHOULD POPULATE THE ASTEROID SETTING VIEW

        _asteroidsRoot.Add(_asteroidSettingRoot);

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
