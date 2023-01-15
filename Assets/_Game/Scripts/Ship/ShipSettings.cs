using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Variables;

[CreateAssetMenu(fileName = "new ShipSettings", menuName = "ScriptableObjects/Ship Settings")]
public class ShipSettings : ScriptableObject
{
    public FloatVariable Throttle;
    public FloatVariable Rotation;
    public IntVariable Health;
}
