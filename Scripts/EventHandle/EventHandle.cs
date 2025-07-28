using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// 负责提供一个api，调用所有逻辑函数
/// </summary>
public class EventHandle : MonoBehaviour
{
    public static float t_rem;

    public static Dictionary<int, GameObject> bombs;

    private void Awake()
    {
        bombs=new Dictionary<int, GameObject>();
    }

    public static void ElseLogic(float now,float rem)
    {
        foreach (var item in bombs)
        {
            //var bomb = item.Value.GetComponent<bomb>();
            //bomb.Move(now - t_rem);
        }
        //子弹


        var cmp = MGM.instance.weapon_maker.GetComponent<WeapongMaker>();
        //cmp.InMaking(now, t_rem);
        //武器生成器


        t_rem= now;
        //置时



        //更新所有玩家
    }


}
