using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 多人游戏进程管理器
/// </summary>
public class MPM : MonoBehaviour
{
    private void Start()
    {
        StartLoading();
    }


    /// <summary>
    /// 开始加载
    /// </summary>
    void StartLoading()
    {
        if (IsHost.ishost == true)
        {
            LoadServer();
        }
        else
        {
            LoadClient();
        }
        
    }


    /// <summary>
    /// 加载主机端
    /// </summary>
    void LoadServer()
    {
        MGM.instance.Nets.transform.GetChild(0).gameObject.SetActive(true);
        MGM.instance.Nets.transform.GetChild(1).gameObject.SetActive(true);

        Saver sv= MGM.instance.Saver.GetComponent<Saver>();
        GameData gd = sv.LoadGame();


        var cmp5 = MGM.instance.Controller.GetComponent<Controller>();
        if (gd != null) cmp5.Default_init();
        else cmp5.Default_init();
        //更新控制器

        var cmp = MGM.instance.statemanager.GetComponent<SM>();
        if (gd != null) cmp.GameData_init(gd);
        else cmp.Defalut_init(cmp5.GetPlayerNum());
        //更新玩家状态管理器


        var cmp1 = MGM.instance.score.GetComponent<ScoreManager>();
        if (gd != null)cmp1.GameDataInit(gd);
        else cmp1.Defalut_init();
        //更新计分板

        var cmp4 = MGM.instance.p1.GetComponent<player>();
        if (gd != null) cmp4.GameDataInit(gd);
        else cmp4.Default_init();
        //更新p1位置

        var cmp3 = MGM.instance.MonsterManager.GetComponent<MonsterManager>();
        if (gd != null) cmp3.GameData_init(gd);
        else cmp3.Defalut_init();
        //更新怪物管理器

        var cmp6 = MGM.instance.p1.GetComponent<player>();
        if (gd != null) cmp6.GameDataInit(gd);
        else cmp6.Default_init();
        //玩家P1初始化

    }


    /// <summary>
    /// 加载客户端
    /// </summary>
    void LoadClient()
    {
        Room.ui.gameObject.SetActive(true);
        //显示UI
        MGM.instance.Nets.transform.GetChild(1).gameObject.SetActive(true);
    }
}
