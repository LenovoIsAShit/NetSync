using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ű�
/// 
///     ���ѡ�����׷��
///     Ѫ�����������ж����˺�(��ײ���)�趨
/// </summary>
public class Monster : MonoBehaviour
{
    int hp;                   //Ѫ��
    GameObject player ;       //һ��Ҫ׷������
    float speed;              //׷���ٶ�
    int damage;               //��ײ�˺�
    int score;                //�˹����������Ʒְ����ӵķ���

    private void Awake()
    {
        Defalut_init();
    }

    private void FixedUpdate()
    {
        Chasing();
    }


    /// <summary>
    /// ����ʱ���Ѿ�ȷ��׷��˭
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
    /// �ṩ������޸ĵİ취
    /// </summary>
    /// <param name="player"></param>
    public void Set_chasingAim(GameObject player)
    {
        this.player = player;
    }



    /// <summary>
    /// ��ײ����Ҽ�Ѫ
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
    /// Ĭ�ϳ�ʼ��
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
    /// ���ѡ�����׷��
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
    /// ����Ѫ
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
    /// �˹���������֪ͨ�Ʒְ�
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
    /// ���ڴ浵��ʼ��ʱ
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
