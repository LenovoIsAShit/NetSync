using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理：
///     此玩家上半身朝向，第几号玩家，当前武器，当前血量，死亡判断（通知玩家状态管理中心）
/// </summary>
public class player : MonoBehaviour
{
    public Vector3 Mpos; //鼠标位置
    public int index;  //玩家号
    public weapon wp;   //当前武器
    public string wp_now;//方便序列化
    public int hp;      //当前血量
    public bool die;    //是否死亡
    public bool mouse;  //是否按鼠标
    public bool space;  //是否按空格

    /// <summary>
    /// 工具变量
    /// </summary>
    float t_rem;//射击上一时刻
    public bool inAir;//是否腾空
    Rigidbody2D rd;//刚体组件 



    private void OnCollisionEnter2D(Collision2D collision)
    {
        inAir = false;
    }




    /// <summary>
    /// 赋值操作
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="index"></param>
    /// <param name="wp"></param>
    /// <param name="hp"></param>
    /// <param name="die"></param>
    public void Set_state(Vector3 pos, int index, string wp, int hp, bool die)
    {
        this.transform.position = pos;
        this.index = index;

        Change_weapon(wp);
        Set_HP(hp);

        this.die = die;
     }


    /// <summary>
    /// 设置HP
    /// </summary>
    /// <param name="hp"></param>
    public void Set_HP(int hp)
    {
        this.hp = hp;

        float x = hp / 100f;
        x = 1 - x;
        RectTransform rt = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(-x, 0f);
        rt.offsetMin = new Vector2(-x, 0f);
        //模型上方显示

        var cmp = MGM.instance.player_state.GetComponent<PlayerState>();
        cmp.Decrease_palyerHP(index, hp / 100f);
        //计分板显示
    }



    /// <summary>
    /// 改变血量,以及计分板血量显示
    /// </summary>
    /// <param name="dx"></param>
    public void Change_hp(int dx)
    {
        hp -= dx;

        if (hp <= 0)
        {
            Player_die();
        }

        float x = hp / 100f;
        x = 1 - x;
        RectTransform rt = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(-x, 0f);
        rt.offsetMin = new Vector2(-x, 0f);
        //模型上方显示

        var cmp = MGM.instance.player_state.GetComponent<PlayerState>();
        cmp.Decrease_palyerHP(index, hp / 100f);
        //计分板显示
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    public void Player_die()
    {
        var cmp = MGM.instance.player_state.GetComponent<PlayerState>();
        cmp.Player_die(index);
    }

    /// <summary>
    /// 更换武器
    /// </summary>
    /// <param name="wp"></param>
    public void Change_weapon(string  wp)
    {
        this.wp_now = wp;

        this.wp = MGM.instance.dataManager.GetComponent<DataManager>().GetWeapon(wp);


        Change_weapon();
    }

    /// <summary>
    /// 跟换朝向，如果到背后，则翻转180度再朝向
    /// </summary>
    /// <param name="vec"></param>
    public void Change_dir(Vector3 pos)
    {
        Transform t = transform.GetChild(2).GetComponent<Transform>();
        pos.Set(pos.x, pos.y, 0f);
        Vector3 p = transform.position;
        p.Set(p.x, p.y, 0f);
        Vector3 vec1 = (pos - p).normalized;

        t.rotation = Quaternion.LookRotation(vec1) * Quaternion.Euler(0f, -90f, 0f);
        transform.GetChild(3).rotation = t.rotation;

        this.Mpos = pos;
    }


    /// <summary>
    /// 射击，向所指方向实例化一个子弹s
    /// </summary>
    public void Shoot()
    {
        if (wp != null)
        {
            GameObject b = Instantiate(Resources.Load<GameObject>(wp.bomb_path));
            b.transform.position = transform.position;
            var cmp = b.GetComponent<bomb>();
            cmp.Set(wp.damage, wp.bomb_speed, transform.GetChild(2).right);
        }
    }


    /// <summary>
    /// 查看是否在规定射击间隙时间之后射击
    /// </summary>
    bool Shoot_check()
    {
        float t_now = Time.time;
        if (t_now - t_rem >= wp.shoot_speed)
        {
            t_rem = t_now;
            return true;
        }
        return false;
    }


    /// <summary>
    /// 存档初始化
    /// </summary>
    /// <param name="gd"></param>
    public void GameDataInit(GameData gd)
    {
        Default_init();
        Set_state(gd.pos, 1, gd.wp, gd.hp, false);
    }


    /// <summary>
    /// 默认初始化
    /// </summary>
    public void Default_init()
    {
        t_rem = Time.time;
        inAir = false;
        rd = GetComponent<Rigidbody2D>();
        index = 1;
        hp = 100;
        die = false;

        wp = null;
    }


    /// <summary>
    /// 改变武器
    /// </summary>
    void Change_weapon()
    {
        if (this.wp != null)
        {
            transform.GetChild(3).gameObject.SetActive(true);
            Image wp = transform.GetChild(3).GetChild(0).GetComponent<Image>();
            wp.sprite = Resources.Load<Sprite>(this.wp.prefab_path);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }
    }



    /// <summary>
    /// 左右移动
    /// </summary>
    /// <param name="isRight"></param>
    public void Move(bool isRight)
    {
        if (isRight)
        {
            transform.position += (Time.deltaTime * 5f * transform.right);
        }
        else
        {
            transform.position -= (Time.deltaTime * 5f * transform.right);
        }
    }


    /// <summary>
    /// 跳跃
    /// </summary>
    public void Jump()
    {
        if (inAir == false)
        {
            rd.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            inAir = true;
        }
    }



    /// <summary>
    /// 暂时只同步这些
    /// </summary>
    /// <param name="sd"></param>
    public void SetStateData(StateData sd)
    {
        transform.position = sd.Pos.AssignToVector3();
        //位置
        Change_dir(sd.MousePos.AssignToVector3());
        //鼠标位置
        if (sd.Mouse) Shoot();
        //射击
        if (sd.Space)
        {
            transform.position = new Vector3(transform.position.x, sd.yPos, transform.position.z);
            inAir = true;
        }
        //跳跃中
    }
}
