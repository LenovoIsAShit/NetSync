using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �����㲥���ͻ��˵���Ҳ���ָ����
/// </summary>
[Serializable]
public class ResData 
{
    public bool A;
    public bool D;
    public bool isInSky;
    public float yPos;
    public bool mouse;
    public Vector3_m Mpos;
    public bool active;

    public ResData()
    {
        Mpos = new Vector3_m();
    }
}
