using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomAsteroidsInspector : EditorWindow
{
    private struct AsteroidGUI
    {
        public Label Header;

        public DropdownField SettingsDropdown;
        AsteroidSettings[] Settings;

        public AsteroidGUI(string header)
        {
            Header = new Label(header);
            

            //Setting up the dropdown menu that lets you browse through all instances of the AsteroidSettings ScriptableObject.
            string[] SettingsGUID = AssetDatabase.FindAssets("t: AsteroidSettings");
            Settings = new AsteroidSettings[SettingsGUID.Length];
            for (int i = 0; i < Settings.Length; i++)
            {
                Settings[i] = (AsteroidSettings) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(SettingsGUID[i]), typeof(AsteroidSettings));
            }

            SettingsDropdown = new DropdownField("Settings");
            List<string> choices = new List<string>();
            foreach(AsteroidSettings astS in Settings)
            {
                choices.Add(astS.name);
            }
            SettingsDropdown.choices = choices;
            SettingsDropdown.index = 0;
            
            
        }
    }

    private struct ShipGUI
    {
        public Label Header;

        public ShipGUI(string header)
        {
            Header = new Label(header);
        }
    }

    private AsteroidGUI aGUI;
    private ShipGUI sGUI;

    [MenuItem("Tools/Asteroids Inspector")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<CustomAsteroidsInspector>();
        window.titleContent = new GUIContent("Asteroids");

        
    }

    public void CreateGUI()
    {
        aGUI = new AsteroidGUI("Asteroid Settings");
        sGUI = new ShipGUI("Ship Settings");

        GroupBox radioButtons = new GroupBox("Tool choice:");

        RadioButton asteroidButton = new RadioButton("Asteroids");
        asteroidButton.RegisterValueChangedCallback(CreateAsteroidGUI);
        radioButtons.Add(asteroidButton);

        RadioButton shipButton = new RadioButton("Ships");
        shipButton.RegisterValueChangedCallback(CreateShipGUI);
        radioButtons.Add(shipButton);

        rootVisualElement.Add(radioButtons);
    }
    public void CreateAsteroidGUI(ChangeEvent<bool> evt)
    {
        if(!evt.newValue)
        {
            rootVisualElement.Remove(aGUI.Header);
            rootVisualElement.Remove(aGUI.SettingsDropdown);
        }

        if (evt.newValue)
        {
            rootVisualElement.Add(aGUI.Header);
            rootVisualElement.Add(aGUI.SettingsDropdown);
        }
    }

    public void CreateShipGUI(ChangeEvent<bool> evt)
    {
        if (!evt.newValue)
        {
            rootVisualElement.Remove(sGUI.Header);
        }

        if (evt.newValue)
        {
            rootVisualElement.Add(sGUI.Header);
        }
    }
}
