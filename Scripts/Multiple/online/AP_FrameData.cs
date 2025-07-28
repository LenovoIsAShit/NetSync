using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ����ÿһ֡���ᷢ�͵�
/// ������ҵ�֡�������ݣ�����=���ͻ��ˣ�
/// </summary>
[Serializable]
public class AP_FrameData
{
    public string host_ip;
    public int frame_mark;//֡��
    public ResData P1;
    public ResData P2;
    public ResData P3;
    public ResData P4;

    public AP_FrameData()
    {
        P1 = new ResData();
        P2 = new ResData();
        P3 = new ResData();
        P4 = new ResData();
    }

    public ResData Get_resdata(int i)
    {
        switch (i)
        {
            case 1:
                return P1;
            case 2:
                return P2;
            case 3:
                return P3;
            case 4:
                return P4;
        }
        return null;
    }

    public void Set(int i,ResData rd)
    {
        ResData r = Get_resdata(i);
        r.A=rd.A;
        r.D = rd.D;
        r.isInSky = rd.isInSky;
        r.yPos = rd.yPos;
        r.mouse = rd.mouse;
        r.Mpos = rd.Mpos;
        r.active= rd.active;
    }
}
