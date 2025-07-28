using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 玩家操作指令 （客户端=》主机）
/// </summary>
[Serializable]
public class ControllerData 
{
    public int Id;
    public bool A;
    public bool D;
    public bool Space;
    public bool Mouse;
    public Vector3_m MousePos;
}
