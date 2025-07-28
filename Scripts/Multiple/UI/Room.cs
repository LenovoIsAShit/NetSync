using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 房间列表
/// </summary>
public class Room : MonoBehaviour
{
    List<string> list;

    public static string host_ip;

    public static GameObject ui;

    int n;  //目前有几个


    private void Awake()
    {
        ui = gameObject;
        LoadAllEvent();
        gameObject.SetActive(false);
    }










    /// <summary>
    /// 将房间添加到UI
    /// </summary>
    public void AddToUi(string ip)
    {
        GameObject room = GetRoom(ip);
    }

    /// <summary>
    /// 获得房间预设件
    /// </summary>
    /// <param name="ip_and_port"></param>
    /// <returns></returns>
    GameObject GetRoom(string ip)
    {
        n++;
        GameObject room = Instantiate(Resources.Load<GameObject>("prefabs/room"));
        room.transform.SetParent(Room.ui.gameObject.transform.GetChild(1));
        Set_i_room(n, room);
        TMP_Text tx = room.transform.GetChild(0).GetComponent<TMP_Text>();
        tx.text = ip;


        return room;
    }





    /// <summary>
    /// 返回按钮（监听事件）
    /// </summary>
    public void Back()
    {

    }
    /// <summary>
    /// 查询房间（监听事件）
    /// </summary>
    public void Refresh()
    {
        var client = MGM.instance.Nets.transform.GetChild(1).GetComponent<Client>();
        client.StartBroadcast();
    }
    /// <summary>
    /// 加入房间（监听事件）
    /// </summary>
    public void Join()
    {
        var client = MGM.instance.Nets.transform.GetChild(1).GetComponent<Client>();
        client.Set_hostip(host_ip);
        client.JoinRoom();

        gameObject.SetActive(false);
    }
    /// <summary>
    /// 挂载所有监听事件
    /// </summary>
    public void LoadAllEvent()
    {
        Button back = transform.GetChild(2).GetComponent<Button>();
        back.onClick.AddListener(Back);
        //返回

        Button join = transform.GetChild(3).GetComponent<Button>();
        join.onClick.AddListener(Join);
        //加入房间

        Button refresh = transform.GetChild(4).GetComponent<Button>();
        refresh.onClick.AddListener(Refresh);
        //刷新
    }



    /// <summary>
    /// 设置host_ip
    /// </summary>
    /// <param name="ip"></param>
    public static void Setip(string ip)
    {
        host_ip = ip;
        var client= MGM.instance.Nets.transform.GetChild(1).GetComponent<Client>();
        client.Set_hostip(ip);
    }


    /// <summary>
    /// 设置第i个房间的位置
    /// </summary>
    /// <param name="i"></param>
    /// <param name="room"></param>
    public void Set_i_room(int i,GameObject room)
    {
        float miny = 0.85f, maxy = 0.9f;
        float dy = 0.05f;

        var rt = room.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.1f, miny - (i - 1) * (0.1f));
        rt.anchorMax = new Vector2(0.9f, maxy - (i - 1) * (0.1f));
        rt.offsetMax = new Vector2(0f, 0f);
        rt.offsetMin = new Vector2(0f, 0f);
    }
}
