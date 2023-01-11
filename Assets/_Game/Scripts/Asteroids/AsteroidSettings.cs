using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variables;

[CreateAssetMenu(fileName = "AsteroidSettings", menuName = "ScriptableObjects/Asteroid Settings")]
public class AsteroidSettings : ScriptableObject
{
    [Range(0.5f, 10f)] public float MinForce = 2f;
    [Range(0.5f, 10f)] public float MaxForce = 6f;
    [Range(0.1f, 3f)] public float MinSize = 0.1f;
    [Range(0.1f, 3f)] public float MaxSize = 1f;
    [Range(0f, 3f)] public float MinTorque = 0.1f;
    [Range(0f, 3f)] public float MaxTorque = 0.5f;
}
