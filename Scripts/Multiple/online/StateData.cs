using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 玩家本地数据
/// </summary>
[Serializable]
public class StateData 
{
    public  int Id;
    public Vector3_m Pos;
    public Vector3_m MousePos;
    public bool Mouse;
    public bool Space;
    public int hp;
    public string wp;
    public bool die;
    public bool active;
    public float yPos;

    public StateData()
    {
        Pos = new Vector3_m();
        MousePos = new Vector3_m();
        wp = "";
    }
}
