using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͨ��UIType���ʵ��
/// </summary>
public class UIManager
{
    Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        dicUI = new Dictionary<UIType, GameObject>();
    }


    /// <summary>
    /// ��ȡһ��ui����
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
    /// ɾ��ĳ��ui����
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
