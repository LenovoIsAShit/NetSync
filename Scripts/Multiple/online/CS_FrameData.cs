using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 客户端帧数据
/// </summary>
public class CS_FrameData
{
    public int frame_mark;//结果帧编号

    public Dictionary<int, StateData> dic;

    public CS_FrameData()
    {
        dic = new Dictionary<int, StateData>();
    }

    public StateData Get(int index)
    {
        if(dic.ContainsKey(index))return dic[index];
        return null;
    }

    public void Set(int index, StateData cd)
    {
        if (!dic.ContainsKey(index)) dic.Add(index, cd);
        else dic[index] = cd;
    }
}
