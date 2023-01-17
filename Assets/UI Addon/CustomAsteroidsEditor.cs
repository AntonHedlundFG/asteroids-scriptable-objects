using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomAsteroidsEditor : EditorWindow
{
    private VisualElement _asteroidsRoot;
    private VisualElement _shipRoot;

    private VisualElement _asteroidSettingRoot;
    private DropdownField _asteroidSettingDropdown;

    private VisualElement _shipSettingRoot;
    private DropdownField _shipSettingDropdown;



    [MenuItem("Tools/Asteroids Inspector")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<CustomAsteroidsEditor>();
        window.titleContent = new GUIContent("Asteroids");
    }

    public void CreateGUI()
    {
        StyleSheet _styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Addon/AsteroidsUIStyleSheet.uss");
        if(_styleSheet == null) { return; }
        rootVisualElement.styleSheets.Add(_styleSheet);

        _asteroidsRoot = new VisualElement();
        _shipRoot = new VisualElement();

        Label toolLabel = new Label("Tool choice:");
        toolLabel.AddToClassList("asteroids-header");
        rootVisualElement.Add(toolLabel);

        rootVisualElement.Add(GenerateRadioButtons());

        GenerateAsteroidGUI();
        rootVisualElement.Add(_asteroidsRoot);
        _asteroidsRoot.visible = false;

        
        GenerateShipGUI();
        rootVisualElement.Add(_shipRoot);
        _shipRoot.visible = false;

    }
    private GroupBox GenerateRadioButtons()
    {
        GroupBox radioButtons = new GroupBox();
        radioButtons.AddToClassList("asteroids-buttongroup");

        RadioButton asteroidButton = new RadioButton("Asteroids");
        asteroidButton.RegisterValueChangedCallback(ShowAsteroidsGUI);
        asteroidButton.AddToClassList("asteroids-radiobutton");
        radioButtons.Add(asteroidButton);

        RadioButton shipButton = new RadioButton("Ships");
        shipButton.RegisterValueChangedCallback(ShowShipGUI);
        shipButton.AddToClassList("asteroids-radiobutton");
        radioButtons.Add(shipButton);
        
        return radioButtons;
    }

    public void ShowAsteroidsGUI(ChangeEvent<bool> evt)
    {
        _asteroidsRoot.visible = evt.newValue;
        if (evt.newValue == false)
        {
            _asteroidsRoot.BringToFront();
        }
    }
    public void ShowShipGUI(ChangeEvent<bool> evt)
    {
        _shipRoot.visible = evt.newValue;
        if (evt.newValue == false)
        {
            _shipRoot.BringToFront();
        }
    }

    public void GenerateAsteroidGUI()
    {
        _asteroidSettingDropdown = DropdownOfAllAssetsOfType<AsteroidSettings>("Asteroid Settings Asset");
        _asteroidSettingDropdown.AddToClassList("asteroids-dropdown");
        _asteroidsRoot.Add(_asteroidSettingDropdown);
        _asteroidSettingDropdown.RegisterValueChangedCallback(ChooseAsteroidSetting);
    }
    public void GenerateShipGUI()
    {
        _shipSettingDropdown = DropdownOfAllAssetsOfType<ShipSettings>("Ship Settings Asset");
        _shipSettingDropdown.AddToClassList("asteroids-dropdown");
        _shipRoot.Add(_shipSettingDropdown);
        _shipSettingDropdown.RegisterValueChangedCallback(ChooseShipSetting);

    }
    public void ChooseAsteroidSetting(ChangeEvent<string> evt)
    {
        if (_asteroidSettingDropdown.index < 0) { return; } //Do nothing if no item has been chosen
        if (_asteroidSettingRoot != null) { _asteroidsRoot.Remove(_asteroidSettingRoot); } //Remove previously shown visual element
        

        //Finds the currently chosen AsteroidSettings instance in the Dropdown menu
        AsteroidSettings[] allSettings = GetAssetsOfType<AsteroidSettings>();
        AsteroidSettings currentSetting = allSettings[_asteroidSettingDropdown.index];

        //Resets the visual element
        _asteroidSettingRoot = new VisualElement();
        _asteroidSettingRoot.AddToClassList("asteroids-serializeddata");

        //Iterate through all visible values in the chosen AsteroidSettings instance, and show them in the editor. Also binds them so they are editable
        SerializedObject so = new SerializedObject(currentSetting);
        SerializedProperty sp = so.GetIterator();
        while (sp.NextVisible(true))
        {
            if (sp.name == "m_Script") { continue; } //For some reason the reference to the C# Script shows up with this iterator. This manually hides this reference
            if (sp.depth > 0) { continue; } //Hides the internal float values of Vector2's
            
            switch(sp.propertyType)
            {
                case SerializedPropertyType.Float:
                    PropertyField pf = new PropertyField(sp, sp.name);
                    pf.AddToClassList("asteroids-slider");
                    pf.Bind(so);
                    _asteroidSettingRoot.Add(pf);
                    break;

                case SerializedPropertyType.Vector2:
                    MinMaxSlider mms = new MinMaxSlider(sp.name);
                    mms.lowLimit = 0f;
                    mms.highLimit = 10f;
                    mms.bindingPath = sp.propertyPath;
                    mms.Bind(so);
                    _asteroidSettingRoot.Add(mms);
                    break;
            }
        }

        //Finally, add the visual element to the root
        _asteroidsRoot.Add(_asteroidSettingRoot);

    }
    public void ChooseShipSetting(ChangeEvent<string> evt)
    {
        if (_shipSettingDropdown.index < 0) { return; } //Do nothing if no item has been chosen
        if (_shipSettingRoot != null) { _shipRoot.Remove(_shipSettingRoot); } //Remove previously shown visual element
        

        //Finds the currently chosen ShipSettings instance in the Dropdown menu
        ShipSettings[] allSettings = GetAssetsOfType<ShipSettings>();
        ShipSettings currentSetting = allSettings[_shipSettingDropdown.index];

        //Resets the visual element
        _shipSettingRoot = new VisualElement();
        _shipSettingRoot.AddToClassList("asteroids-serializeddata");

        AddThrottleAndRotation(currentSetting, _shipSettingRoot);
        AddHealth(currentSetting, _shipSettingRoot);

        //Finally, add the visual element to the root
        _shipRoot.Add(_shipSettingRoot);
    }

    private void AddThrottleAndRotation(ShipSettings setting, VisualElement rootVE)
    {
        SerializedObject so = new SerializedObject(setting);
        SerializedProperty spThrottle = so.FindProperty("Throttle");
        SerializedProperty spRotation = so.FindProperty("Rotation");
        PropertyField pfThrottle = new PropertyField(spThrottle, "Throttle");
        PropertyField pfRotation = new PropertyField(spRotation, "Rotation");
        pfThrottle.AddToClassList("asteroids-slider");
        pfRotation.AddToClassList("asteroids-slider");
        pfThrottle.Bind(so);
        pfRotation.Bind(so);
        rootVE.Add(pfThrottle);
        rootVE.Add(pfRotation);
    }
    private void AddHealth(ShipSettings setting, VisualElement rootVE)
    {
        SerializedObject so = new SerializedObject(setting.Health);
        SerializedProperty spHealth = so.FindProperty("BaseValue");
        PropertyField pfHealth = new PropertyField(spHealth, "Starting Health");
        pfHealth.AddToClassList("asteroids-intfield");
        pfHealth.Bind(so);
        _shipSettingRoot.Add(pfHealth);
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
