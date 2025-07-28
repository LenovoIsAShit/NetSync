using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 主机收到询问后，返回查询
///     这是查询消息类型2
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
