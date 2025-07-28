using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGM : SingleAutoMono<MGM>
{
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    //玩家

    public GameObject score;
    //计分板
    public GameObject player_state;
    //玩家状态栏
    public GameObject dataManager;
    //数据配置管理
    public GameObject statemanager;
    //玩家状态管理中心
    public GameObject Saver;
    //存档管理
    public GameObject Controller;
    //操作控制
    public GameObject weapon_maker;
    //武器随机生成器
    public GameObject MonsterManager;
    //怪物生成器
    public GameObject Camera;
    //相机
    public GameObject UImanager;
    //UI管理
    public GameObject Nets;
    //网络
}
