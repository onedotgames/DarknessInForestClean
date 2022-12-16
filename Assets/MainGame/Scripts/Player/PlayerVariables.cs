using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Variables")]
public class PlayerVariables : ScriptableObject
{
    public float MoveSpeed;
    public float InputSensitivity;
    public int PlayerMaxHealth;
    public float WalkAcceleration;
    public float GroundDeceleration;
}
