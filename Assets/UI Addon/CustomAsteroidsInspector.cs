using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomAsteroidsInspector : EditorWindow
{
    private VisualElement asteroidsElement;
    private VisualElement shipElement;

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


        //Setting up the dropdown menu that lets you browse through all instances of the AsteroidSettings ScriptableObject.
        string[] SettingsGUID = AssetDatabase.FindAssets("t: AsteroidSettings");
        AsteroidSettings[] Settings = new AsteroidSettings[SettingsGUID.Length];
        for (int i = 0; i < Settings.Length; i++)
        {
            Settings[i] = (AsteroidSettings)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(SettingsGUID[i]), typeof(AsteroidSettings));
        }

        DropdownField SettingsDropdown = new DropdownField("Settings");
        List<string> choices = new List<string>();
        foreach (AsteroidSettings astS in Settings)
        {
            choices.Add(astS.name);
        }
        SettingsDropdown.choices = choices;
        SettingsDropdown.index = 0;

        asteroidsElement.Add(Header);
        asteroidsElement.Add(SettingsDropdown);
        asteroidsElement.visible = false;
    }
}
