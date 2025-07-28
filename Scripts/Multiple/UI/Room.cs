using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����б�
/// </summary>
public class Room : MonoBehaviour
{
    List<string> list;

    public static string host_ip;

    public static GameObject ui;

    int n;  //Ŀǰ�м���


    private void Awake()
    {
        ui = gameObject;
        LoadAllEvent();
        gameObject.SetActive(false);
    }










    /// <summary>
    /// ��������ӵ�UI
    /// </summary>
    public void AddToUi(string ip)
    {
        GameObject room = GetRoom(ip);
    }

    /// <summary>
    /// ��÷���Ԥ���
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
    /// ���ذ�ť�������¼���
    /// </summary>
    public void Back()
    {

    }
    /// <summary>
    /// ��ѯ���䣨�����¼���
    /// </summary>
    public void Refresh()
    {
        var client = MGM.instance.Nets.transform.GetChild(1).GetComponent<Client>();
        client.StartBroadcast();
    }
    /// <summary>
    /// ���뷿�䣨�����¼���
    /// </summary>
    public void Join()
    {
        var client = MGM.instance.Nets.transform.GetChild(1).GetComponent<Client>();
        client.Set_hostip(host_ip);
        client.JoinRoom();

        gameObject.SetActive(false);
    }
    /// <summary>
    /// �������м����¼�
    /// </summary>
    public void LoadAllEvent()
    {
        Button back = transform.GetChild(2).GetComponent<Button>();
        back.onClick.AddListener(Back);
        //����

        Button join = transform.GetChild(3).GetComponent<Button>();
        join.onClick.AddListener(Join);
        //���뷿��

        Button refresh = transform.GetChild(4).GetComponent<Button>();
        refresh.onClick.AddListener(Refresh);
        //ˢ��
    }



    /// <summary>
    /// ����host_ip
    /// </summary>
    /// <param name="ip"></param>
    public static void Setip(string ip)
    {
        host_ip = ip;
        var client= MGM.instance.Nets.transform.GetChild(1).GetComponent<Client>();
        client.Set_hostip(ip);
    }


    /// <summary>
    /// ���õ�i�������λ��
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
