using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �����������
/// </summary>
public class MonsterManager : MonoBehaviour
{
    int num;        //һ��Ҫɱ�Ĺ���
    public int res;        //����Ҫɱ�Ĺ���
    int make_need;  //����Ҫ���ɵ�

    float t_rem=0;    //��һ�����ɹ����ʱ��
    float t_clock=1f;  //���ɹ����ʱ����


    private void FixedUpdate()
    {
        Monster_making();
    }






    /// <summary>
    /// ����һֻ����
    /// </summary>
    public void Decrese_res()
    {
        res--;
        NextLevel();
    }


    bool NextLevel_Check()
    {
        if (res == 0) return true;
        else return false;
    }


    /// <summary>
    /// ��һ�أ�֪ͨscoremanager
    /// </summary>
    void NextLevel()
    {
        if (NextLevel_Check())
        {
            var cmp = MGM.instance.score.GetComponent<ScoreManager>();
            cmp.NextLevel();

            num = cmp.level_now * 2;
            res = num;
            make_need = num;
        }
    }


    /// <summary>
    /// Ĭ�ϳ�ʼ��
    /// </summary>
    public void Defalut_init()
    {
        var cmp = MGM.instance.score.GetComponent<ScoreManager>();
        num = cmp.level_now * 2;
        res = num;
        make_need = num;
    }


    /// <summary>
    /// �浵��ʼ��
    /// </summary>
    /// <param name="gd"></param>
    public void GameData_init(GameData gd)
    {
        res = gd.killneed;
        num = gd.level * 2;
        make_need = res ;
    }


    public void Monster_making()
    {
        if (make_need > 0&&Time.time-t_rem>=t_clock)
        {
            make_need--;
            t_rem = Time.time;
            GameObject obj = Instantiate(Resources.Load<GameObject>("prefabs/monster"));
            var cmp = obj.GetComponent<Monster>();
            cmp.Defalut_init();
        }
    }
}
