using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// �����ṩһ��api�����������߼�����
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
        //�ӵ�


        var cmp = MGM.instance.weapon_maker.GetComponent<WeapongMaker>();
        //cmp.InMaking(now, t_rem);
        //����������


        t_rem= now;
        //��ʱ



        //�����������
    }


}
