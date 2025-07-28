using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���״̬������ֻ����״̬������ʾ
/// </summary>

public class PlayerState : MonoBehaviour
{
    float minx = 72f;

    /// <summary>
    /// ��ʾĳ���������
    /// </summary>
    /// <param name="index"></param>
    public void Player_die(int index)
    {
        transform.GetChild(index - 1).GetChild(2).gameObject.SetActive(true);
    }


    /// <summary>
    /// ĳ�����Ѫ������
    /// </summary>
    public void Decrease_palyerHP(int index,float bl)
    {
        bl = 1 - bl;
        RectTransform rt = transform.GetChild(index-1).GetChild(1).GetChild(0).GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(-minx * bl, 0f);
        rt.offsetMin = new Vector2(-minx * bl, 0f);
    }

    

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="index"></param>
    public void Add(int index)
    {
        transform.GetChild(index - 1).gameObject.SetActive(true);
    }

}
