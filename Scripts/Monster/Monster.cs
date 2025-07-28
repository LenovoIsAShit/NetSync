using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 怪物脚本
/// 
///     随机选择玩家追逐
///     血量管理，死亡判定，伤害(碰撞玩家)设定
/// </summary>
public class Monster : MonoBehaviour
{
    int hp;                   //血量
    GameObject player ;       //一生要追逐的玩家
    float speed;              //追逐速度
    int damage;               //碰撞伤害
    int score;                //此怪物死亡，计分板增加的分数

    private void Awake()
    {
        Defalut_init();
    }

    private void FixedUpdate()
    {
        Chasing();
    }


    /// <summary>
    /// 生成时就已经确定追逐谁
    /// </summary>
    void Chasing()
    {
        if (player != null)
        {
            float dx = (player.transform.position.x - gameObject.transform.position.x);
            if (dx > 0)
            {
                transform.position += (speed * Time.deltaTime * transform.right);
            }
            else
            {
                transform.position -= (speed * Time.deltaTime * transform.right);
            }
        }
    }


    /// <summary>
    /// 提供给外界修改的办法
    /// </summary>
    /// <param name="player"></param>
    public void Set_chasingAim(GameObject player)
    {
        this.player = player;
    }



    /// <summary>
    /// 碰撞让玩家减血
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            var cmp = MGM.instance.statemanager.GetComponent<SM>();
            int id = collision.gameObject.GetComponent<player>().index;
            cmp.Player_injured(id, damage);
        }
    }



    /// <summary>
    /// 默认初始化
    /// </summary>
    public  void Defalut_init()
    {
        speed = 3f;
        hp = 100;
        player = null;
        damage = 10;
        score = 5;

        transform.position = MGM.instance.MonsterManager.transform.position;
        RandomChose();
    }


    /// <summary>
    /// 随机选择玩家追逐
    /// </summary>
    void RandomChose()
    {
        System.Random rd = new System.Random();
        var cmp = MGM.instance.statemanager.GetComponent<SM>();
        int mx = cmp.GetPlayernum();
        int res = rd.Next(1, mx+1);

        switch (res)
        {
            case 1:
                player = MGM.instance.p1;
                break;
            case 2:
                player = MGM.instance.p2;
                break;
            case 3:
                player = MGM.instance.p3;
                break;
            case 4:
                player = MGM.instance.p4;
                break;
            default:
                return;
        }
    }


    /// <summary>
    /// 增减血
    /// </summary>
    /// <param name="dx"></param>
    public void Change_hp(int dx)
    {
        hp -= dx;

        if (hp <= 0)
        {
            Monster_die();
        }

        float x = hp / 100f;
        x = 1 - x;
        RectTransform rt = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(-x, 0f);
        rt.offsetMin = new Vector2(-x, 0f);
    }


    /// <summary>
    /// 此怪物死亡，通知计分板
    /// </summary>
    public void Monster_die()
    {
        var cmp = MGM.instance.score.GetComponent<ScoreManager>();
        var cmp2 = MGM.instance.MonsterManager.GetComponent<MonsterManager>();
        cmp.Add(score);
        cmp2.Decrese_res();
        Destroy(this.gameObject);
    }



    /// <summary>
    /// 用于存档初始化时
    /// </summary>
    /// <param name="gd"></param>
    void Monster_Copy(GameObject gd)
    {
        var cmp = gd.GetComponent<Monster>();
        this.hp = cmp.hp;
        this.player=cmp.player;
        this.score = cmp.score;
        this.speed = cmp.speed;
        this.damage = cmp.damage;
    }
}
