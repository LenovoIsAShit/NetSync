using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// ÎäÆ÷ÀàÅäÖÃÎÄ¼ş
/// </summary>
[CreateAssetMenu(fileName = "weapon", menuName = "CustomData/weapon")]
[Serializable]
public class weapon : ScriptableObject
{
    public string weapon_name;
    public string bomb_path;
    public string prefab_path;

    public int damage;
    public float shoot_speed;
    public float bomb_speed;
}
