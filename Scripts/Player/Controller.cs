using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


/// <summary>
/// 控制中心，缓存每个玩家的操作，并指定你是几号玩家。
///     输入操作时把操作指令缓存到对应的玩家下，再集中执行
/// </summary>
public class Controller : MonoBehaviour
{
    int index;          //控制的几号玩家
    int player_num;     //现有的玩家数量

    List<GameObject> players;       //玩家

    Camera cm;                      //相机

    [HideInInspector]
    public ControllerData cd;          //输入检测

    player p
    {
        get
        {
            return players[index-1].GetComponent<player>();
        }
    }

    private void Awake()
    {
        cm = MGM.instance.Camera.GetComponent<CinemachineBrain>().GetComponent<Camera>();
    }

    public void Change_Camera_Lookat(int i)
    {
        var cm_now = MGM.instance.Camera.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        cm_now.Follow = SM.Get_player(i).transform;
        cm_now.LookAt = SM.Get_player(i).transform;
    }


    /// <summary>
    /// 默认初始化
    /// </summary>
    public void Default_init()
    {
        index = 1;
        player_num = 1;
        players = new List<GameObject>();
        players.Add(MGM.instance.p1);
        //默认加入玩家1

        cm= CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
    }

    private void Update()
    {
        //GetInput();
    }




    /// <summary>
    /// 获得玩家数量
    /// </summary>
    /// <returns></returns>
    public int GetPlayerNum()
    {
        return player_num;
    }


    /// <summary>
    /// 获得玩家标号
    /// </summary>
    /// <returns></returns>
    public int GetIndex()
    {
        return index;
    }


    /// <summary>
    /// 本地的检测按键，不需要被集中调用
    /// </summary>
    public void GetInput()
    {
        if(cd== null)cd = new ControllerData();

        if (Input.GetKey(KeyCode.A))cd.A = true;
        if (Input.GetKey(KeyCode.D))cd.D = true;
        if (Input.GetKeyDown(KeyCode.Space)) cd.Space = true;
        if (Input.GetKeyDown(KeyCode.Mouse0))cd.Mouse = true;
        Vector3 mpos = cm.ScreenToWorldPoint(Input.mousePosition);
        cd.MousePos.Assign(mpos);
    }


    /// <summary>
    /// 清空输入
    /// </summary>
    public void CleanUpInput()
    {
        cd.A = false;
        cd.D = false;
        cd.Mouse = false;
        cd.Space = false;
    }



    /// <summary>
    /// 更新所有玩家
    /// </summary>
    public void UpdateAllPlayer()
    {

    }
}
