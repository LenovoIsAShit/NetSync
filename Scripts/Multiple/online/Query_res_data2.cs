using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �����յ�ѯ�ʺ󣬷��ز�ѯ
///     ���ǲ�ѯ��Ϣ����2
/// </summary>
[Serializable]
public class Query_res_data2:Query_res_data
{
    public int playerId;
    public float join_time;

    public ServerFrameCache FC;
    
    public Query_res_data2()
    {
        FC = new ServerFrameCache();
    }
}
