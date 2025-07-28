using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家状态栏管理，只负责状态栏的显示
/// </summary>

public class PlayerState : MonoBehaviour
{
    float minx = 72f;

    /// <summary>
    /// 显示某个玩家死亡
    /// </summary>
    /// <param name="index"></param>
    public void Player_die(int index)
    {
        transform.GetChild(index - 1).GetChild(2).gameObject.SetActive(true);
    }


    /// <summary>
    /// 某个玩家血量减少
    /// </summary>
    public void Decrease_palyerHP(int index,float bl)
    {
        bl = 1 - bl;
        RectTransform rt = transform.GetChild(index-1).GetChild(1).GetChild(0).GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(-minx * bl, 0f);
        rt.offsetMin = new Vector2(-minx * bl, 0f);
    }

    

    /// <summary>
    /// 激活玩家面板
    /// </summary>
    /// <param name="index"></param>
    public void Add(int index)
    {
        transform.GetChild(index - 1).gameObject.SetActive(true);
    }

}
