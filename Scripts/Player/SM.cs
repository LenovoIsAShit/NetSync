using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���״̬��������
/// </summary>
public class SM : MonoBehaviour
{
    public static int player_num;     //��ǰ�������
    int max_num;        //����������
    public Dictionary<int, bool> player_active;    //��¼����Ƿ񼤻�

    private void Awake()
    {
        Defalut_init(1);
    }



    /// <summary>
    /// ����Ŀǰ�������
    /// </summary>
    /// <returns></returns>
    public int GetPlayernum()
    {
        return player_num;
    }

    /// <summary>
    /// ��ʼ��
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
    /// �浵��ʼ��
    /// </summary>
    /// <param name="gd"></param>
    public void GameData_init(GameData gd)
    {
        Defalut_init(1);
        

    }


    /// <summary>
    /// �������
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
    /// �������
    /// </summary>
    public void Set_player_die(int index)
    {
        GameObject player = Get_player(index);

        var cmp = player.GetComponent<player>();
        cmp.Player_die();
    }


    /// <summary>
    /// �����ң�״̬������Ҷ�Ҫ����
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
    /// ������
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
    /// ������״̬
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
    /// ����һ�����
    /// ����֡��Ϣ��������¼ӵ��Ǹ���ҵı�ţ�
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
