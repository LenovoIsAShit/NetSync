using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �������Զ��̳�MonoBehavior
/// </summary>
public class SingleAutoMono<T> : MonoBehaviour
{
    public static T instance;

    public void Awake()
    {
        instance = this.gameObject.GetComponent<T>();
    }
}
