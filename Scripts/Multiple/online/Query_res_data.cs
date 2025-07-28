using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 主机收到询问后，返回查询
///     这是查询消息类型1
/// </summary>
[Serializable]
public class Query_res_data 
{
    public int type;
    public string ip;
    public bool isHost;
}
