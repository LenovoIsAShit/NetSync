using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 管理怪物生成
/// </summary>
public class MonsterManager : MonoBehaviour
{
    int num;        //一共要杀的怪物
    public int res;        //还需要杀的怪物
    int make_need;  //还需要生成的

    float t_rem=0;    //上一次生成怪物的时间
    float t_clock=1f;  //生成怪物的时间间隔


    private void FixedUpdate()
    {
        Monster_making();
    }






    /// <summary>
    /// 减少一只怪物
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
    /// 下一关，通知scoremanager
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
    /// 默认初始化
    /// </summary>
    public void Defalut_init()
    {
        var cmp = MGM.instance.score.GetComponent<ScoreManager>();
        num = cmp.level_now * 2;
        res = num;
        make_need = num;
    }


    /// <summary>
    /// 存档初始化
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
