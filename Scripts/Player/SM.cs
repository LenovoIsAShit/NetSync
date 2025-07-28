using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 玩家状态管理中心
/// </summary>
public class SM : MonoBehaviour
{
    public static int player_num;     //当前玩家数量
    int max_num;        //最大玩家数量
    public Dictionary<int, bool> player_active;    //记录玩家是否激活

    private void Awake()
    {
        Defalut_init(1);
    }



    /// <summary>
    /// 返回目前玩家数量
    /// </summary>
    /// <returns></returns>
    public int GetPlayernum()
    {
        return player_num;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Defalut_init(int player_num)
    {
        SM.player_num = player_num;
        player_active = new Dictionary<int, bool>();
        max_num = 4;
        for(int i = 1; i <= player_num - 1; i++)
        {
            AddPlayer();
        }
    }



    /// <summary>
    /// 存档初始化
    /// </summary>
    /// <param name="gd"></param>
    public void GameData_init(GameData gd)
    {
        Defalut_init(1);
        

    }


    /// <summary>
    /// 玩家受伤
    /// </summary>
    /// <param name="index"></param>
    /// <param name="damage"></param>
    public void Player_injured(int index,int damage)
    {
        GameObject player = Get_player(index);

        var cmp = player.GetComponent<player>();
        cmp.Change_hp(damage);
    }


    /// <summary>
    /// 玩家死亡
    /// </summary>
    public void Set_player_die(int index)
    {
        GameObject player = Get_player(index);

        var cmp = player.GetComponent<player>();
        cmp.Player_die();
    }


    /// <summary>
    /// 添加玩家，状态栏和玩家都要激活
    /// </summary>
    public int AddPlayer()
    {
        player_num++;
        switch (player_num)
        {
            case 2:
                MGM.instance.p2.SetActive(true);
                MGM.instance.player_state.GetComponent<PlayerState>().Add(player_num);
                break;
            case 3:
                MGM.instance.p3.SetActive(true);
                MGM.instance.player_state.GetComponent<PlayerState>().Add(player_num);
                break;
            case 4:
                MGM.instance.p4.SetActive(true);
                MGM.instance.player_state.GetComponent<PlayerState>().Add(player_num);
                break;
        }
        return player_num;
    }


    /// <summary>
    /// 获得玩家
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static GameObject Get_player(int index)
    {
        switch (index)
        {
            case 1:
                return MGM.instance.p1;
            case 2:
                return MGM.instance.p2;
            case 3:
                return MGM.instance.p3;
            case 4:
                return MGM.instance.p4;
            default:
                return null;
        }
    }    


    /// <summary>
    /// 获得玩家状态
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static StateData Get_player_sd(int index)
    {
        GameObject p = Get_player(index);
        StateData sd = new StateData();
        var cmp = p.GetComponent<player>();
        sd.hp = cmp.hp;
        sd.Id = index;
        sd.Pos.Assign(p.transform.position);
        sd.MousePos.Assign(cmp.Mpos);
        
        sd.Mouse = cmp.mouse;
        sd.Space = cmp.space;
        sd.wp = cmp.wp_now;
        sd.die = cmp.die;
        if (Client.player_active.ContainsKey(index)) sd.active = Client.player_active[index];
        else sd.active = false;

        

        return sd;
    }


    /// <summary>
    /// 增加一个玩家
    /// 返回帧信息（编号是新加的那个玩家的编号）
    /// </summary>
    /// <returns></returns>
    public Query_res_data2 GetAllState()
    {
        Query_res_data2 res = new Query_res_data2();

        //AddPlayer();

        res.playerId = player_num + 1;
        res.join_time = Time.time;


        for(int i = 1; i <= max_num; i++)
        {
            res.FC.Setplayer(i, true, Get_player_sd(i));
        }

        res.type = 2;

        return res;
    }
}
