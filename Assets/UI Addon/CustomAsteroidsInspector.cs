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

        public AsteroidGUI(string header)
        {
            Header = new Label(header);
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
        }

        if (evt.newValue)
        {
            rootVisualElement.Add(aGUI.Header);
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
