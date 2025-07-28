using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


/// <summary>
/// �������ģ�����ÿ����ҵĲ�������ָ�����Ǽ�����ҡ�
///     �������ʱ�Ѳ���ָ��浽��Ӧ������£��ټ���ִ��
/// </summary>
public class Controller : MonoBehaviour
{
    int index;          //���Ƶļ������
    int player_num;     //���е��������

    List<GameObject> players;       //���

    Camera cm;                      //���

    [HideInInspector]
    public ControllerData cd;          //������

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
    /// Ĭ�ϳ�ʼ��
    /// </summary>
    public void Default_init()
    {
        index = 1;
        player_num = 1;
        players = new List<GameObject>();
        players.Add(MGM.instance.p1);
        //Ĭ�ϼ������1

        cm= CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
    }

    private void Update()
    {
        //GetInput();
    }




    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    public int GetPlayerNum()
    {
        return player_num;
    }


    /// <summary>
    /// �����ұ��
    /// </summary>
    /// <returns></returns>
    public int GetIndex()
    {
        return index;
    }


    /// <summary>
    /// ���صļ�ⰴ��������Ҫ�����е���
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
    /// �������
    /// </summary>
    public void CleanUpInput()
    {
        cd.A = false;
        cd.D = false;
        cd.Mouse = false;
        cd.Space = false;
    }



    /// <summary>
    /// �����������
    /// </summary>
    public void UpdateAllPlayer()
    {

    }
}
