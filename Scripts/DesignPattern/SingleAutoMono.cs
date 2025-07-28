using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// µ¥Àý£¬×Ô¶¯¼Ì³ÐMonoBehavior
/// </summary>
public class SingleAutoMono<T> : MonoBehaviour
{
    public static T instance;

    public void Awake()
    {
        instance = this.gameObject.GetComponent<T>();
    }
}
