using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过UIType获得实例
/// </summary>
public class UIManager
{
    Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        dicUI = new Dictionary<UIType, GameObject>();
    }


    /// <summary>
    /// 获取一个ui对象
    /// </summary>
    /// <param name="ut"></param>
    /// <returns></returns>
    public GameObject GetSingleUi(UIType ut)
    {
        if (!dicUI.ContainsKey(ut))
        {
            dicUI.Add(ut, MonoBehaviour.Instantiate(Resources.Load<GameObject>(ut.path)));
        }

        return dicUI[ut];
    }
    
    /// <summary>
    /// 删除某个ui对象
    /// </summary>
    /// <param name="ut"></param>
    public void DestroyuUi(UIType ut)
    {
        if (dicUI.ContainsKey(ut))
        {
            GameObject obj = dicUI[ut];
            dicUI.Remove(ut);
            MonoBehaviour.Destroy(obj);
        }
    }
}
