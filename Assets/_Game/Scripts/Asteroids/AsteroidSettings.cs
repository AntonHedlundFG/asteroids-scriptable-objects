using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Variables;

[CreateAssetMenu(fileName = "AsteroidSettings", menuName = "ScriptableObjects/Asteroid Settings")]
public class AsteroidSettings : ScriptableObject
{
    public Vector2 Force = new Vector2(2f, 6f);
    public Vector2 Size = new Vector2(1f, 10f);
    public Vector2 Torque = new Vector2(1f, 5f);

    /* OLD DATA TYPES
    [Range(0.5f, 10f)] public float MinForce = 2f;
    [Range(0.5f, 10f)] public float MaxForce = 6f;
    [Range(0.1f, 3f)] public float MinSize = 0.1f;
    [Range(0.1f, 3f)] public float MaxSize = 1f;
    [Range(0f, 3f)] public float MinTorque = 0.1f;
    [Range(0f, 3f)] public float MaxTorque = 0.5f;
    */

    public float MinForce => Force.x;
    public float MaxForce => Force.y;
    public float MinSize => Size.x * 0.2f;
    public float MaxSize => Size.y * 0.2f;
    public float MinTorque => Torque.x * 0.1f;
    public float MaxTorque => Torque.y * 0.1f;
}
