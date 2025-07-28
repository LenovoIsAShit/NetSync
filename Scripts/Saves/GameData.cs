using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 游戏存档
/// </summary>
[Serializable]
public class GameData
{
    public Vector3 pos;
    public string wp;
    public int hp;
    //主机玩家数据

    public int score;
    public int level;
    public int killneed;
    //全局数据
}
