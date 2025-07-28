using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 一段时间自动生成随机武器，通过碰撞获得
/// </summary>
public class WeapongMaker : MonoBehaviour
{
    bool isTakenAway;       //是否被拿走
    bool ok;                //是否已经生成

    float t_rem;            //在被拿走时的时间
    weapon wp;              //生成的武器类型
    string wp_now;
    float t_ok;             //需要生成的时间

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        InMaking(Time.time,t_rem);
    }

    /// <summary>
    /// 默认初始化
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
    /// 随机生成武器
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
    /// 碰撞获得
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
    /// 每帧都要检车是否可以生成了
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
    /// 改变图片
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
