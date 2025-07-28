using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// һ��ʱ���Զ��������������ͨ����ײ���
/// </summary>
public class WeapongMaker : MonoBehaviour
{
    bool isTakenAway;       //�Ƿ�����
    bool ok;                //�Ƿ��Ѿ�����

    float t_rem;            //�ڱ�����ʱ��ʱ��
    weapon wp;              //���ɵ���������
    string wp_now;
    float t_ok;             //��Ҫ���ɵ�ʱ��

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        InMaking(Time.time,t_rem);
    }

    /// <summary>
    /// Ĭ�ϳ�ʼ��
    /// </summary>
    void Init()
    {
        t_rem = Time.time;
        ok = false;
        isTakenAway = true;
        //t_ok = 8f;
        t_ok = 2f;
    }


    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    weapon RandomWeapon()
    {
        System.Random rd =new  System.Random();
        double res = rd.NextDouble();
        var cmp = MGM.instance.dataManager.GetComponent<DataManager>();
        if (res >= 0.5)
        {
            wp_now = "de";
            return cmp.GetWeapon("de");
        }
        else
        {
            wp_now = "ak";
            return cmp.GetWeapon("ak");
        }
    }


    /// <summary>
    /// ��ײ���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ok)
        {
            var cmp = other.GetComponent<player>();
            cmp.Change_weapon(wp_now);
            isTakenAway = true;
            ok = false;
            t_rem = Time.time;
            Change_image(null);
        }
    }



    /// <summary>
    /// ÿ֡��Ҫ�쳵�Ƿ����������
    /// </summary>
    public void InMaking(float now,float t_rem)
    {
        if (now - t_rem >= t_ok && isTakenAway == true)
        {
            wp = RandomWeapon();
            Change_image(wp);
            isTakenAway = false;
            ok = true;
        }
    }


    /// <summary>
    /// �ı�ͼƬ
    /// </summary>
    void Change_image(weapon wp)
    {
        Image ima = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        if (wp != null)
        {
            ima.sprite = Resources.Load<Sprite>(wp.prefab_path);
        }
        else
        {
            ima.sprite = null;
        }
    }
}
