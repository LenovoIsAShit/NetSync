using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������Ϸ���̹�����
/// </summary>
public class MPM : MonoBehaviour
{
    private void Start()
    {
        StartLoading();
    }


    /// <summary>
    /// ��ʼ����
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
    /// ����������
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
        //���¿�����

        var cmp = MGM.instance.statemanager.GetComponent<SM>();
        if (gd != null) cmp.GameData_init(gd);
        else cmp.Defalut_init(cmp5.GetPlayerNum());
        //�������״̬������


        var cmp1 = MGM.instance.score.GetComponent<ScoreManager>();
        if (gd != null)cmp1.GameDataInit(gd);
        else cmp1.Defalut_init();
        //���¼Ʒְ�

        var cmp4 = MGM.instance.p1.GetComponent<player>();
        if (gd != null) cmp4.GameDataInit(gd);
        else cmp4.Default_init();
        //����p1λ��

        var cmp3 = MGM.instance.MonsterManager.GetComponent<MonsterManager>();
        if (gd != null) cmp3.GameData_init(gd);
        else cmp3.Defalut_init();
        //���¹��������

        var cmp6 = MGM.instance.p1.GetComponent<player>();
        if (gd != null) cmp6.GameDataInit(gd);
        else cmp6.Default_init();
        //���P1��ʼ��

    }


    /// <summary>
    /// ���ؿͻ���
    /// </summary>
    void LoadClient()
    {
        Room.ui.gameObject.SetActive(true);
        //��ʾUI
        MGM.instance.Nets.transform.GetChild(1).gameObject.SetActive(true);
    }
}
