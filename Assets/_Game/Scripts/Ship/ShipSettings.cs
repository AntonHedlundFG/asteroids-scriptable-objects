using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Variables;

[CreateAssetMenu(fileName = "new ShipSettings", menuName = "ScriptableObjects/Ship Settings")]
public class ShipSettings : ScriptableObject
{
    [Range(0f, 10f)] public float Throttle;
    [Range(0f, 10f)] public float Rotation;
    public IntVariable Health;
}
