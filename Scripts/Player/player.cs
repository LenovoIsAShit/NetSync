using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����
///     ������ϰ����򣬵ڼ�����ң���ǰ��������ǰѪ���������жϣ�֪ͨ���״̬�������ģ�
/// </summary>
public class player : MonoBehaviour
{
    public Vector3 Mpos; //���λ��
    public int index;  //��Һ�
    public weapon wp;   //��ǰ����
    public string wp_now;//�������л�
    public int hp;      //��ǰѪ��
    public bool die;    //�Ƿ�����
    public bool mouse;  //�Ƿ����
    public bool space;  //�Ƿ񰴿ո�

    /// <summary>
    /// ���߱���
    /// </summary>
    float t_rem;//�����һʱ��
    public bool inAir;//�Ƿ��ڿ�
    Rigidbody2D rd;//������� 



    private void OnCollisionEnter2D(Collision2D collision)
    {
        inAir = false;
    }




    /// <summary>
    /// ��ֵ����
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
    /// ����HP
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
        //ģ���Ϸ���ʾ

        var cmp = MGM.instance.player_state.GetComponent<PlayerState>();
        cmp.Decrease_palyerHP(index, hp / 100f);
        //�Ʒְ���ʾ
    }



    /// <summary>
    /// �ı�Ѫ��,�Լ��Ʒְ�Ѫ����ʾ
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
        //ģ���Ϸ���ʾ

        var cmp = MGM.instance.player_state.GetComponent<PlayerState>();
        cmp.Decrease_palyerHP(index, hp / 100f);
        //�Ʒְ���ʾ
    }

    /// <summary>
    /// �������
    /// </summary>
    public void Player_die()
    {
        var cmp = MGM.instance.player_state.GetComponent<PlayerState>();
        cmp.Player_die(index);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="wp"></param>
    public void Change_weapon(string  wp)
    {
        this.wp_now = wp;

        this.wp = MGM.instance.dataManager.GetComponent<DataManager>().GetWeapon(wp);


        Change_weapon();
    }

    /// <summary>
    /// �������������������ת180���ٳ���
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
    /// ���������ָ����ʵ����һ���ӵ�s
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
    /// �鿴�Ƿ��ڹ涨�����϶ʱ��֮�����
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
    /// �浵��ʼ��
    /// </summary>
    /// <param name="gd"></param>
    public void GameDataInit(GameData gd)
    {
        Default_init();
        Set_state(gd.pos, 1, gd.wp, gd.hp, false);
    }


    /// <summary>
    /// Ĭ�ϳ�ʼ��
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
    /// �ı�����
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
    /// �����ƶ�
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
    /// ��Ծ
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
    /// ��ʱֻͬ����Щ
    /// </summary>
    /// <param name="sd"></param>
    public void SetStateData(StateData sd)
    {
        transform.position = sd.Pos.AssignToVector3();
        //λ��
        Change_dir(sd.MousePos.AssignToVector3());
        //���λ��
        if (sd.Mouse) Shoot();
        //���
        if (sd.Space)
        {
            transform.position = new Vector3(transform.position.x, sd.yPos, transform.position.z);
            inAir = true;
        }
        //��Ծ��
    }
}
