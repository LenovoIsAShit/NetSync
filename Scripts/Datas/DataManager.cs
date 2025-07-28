using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���ø������ݲ��ṩ��ѯ���
/// </summary>
public class DataManager : MonoBehaviour
{
    public weapon gun1_ak;
    public weapon gun2_de;

    public weapon GetWeapon(string name)
    {
        switch (name)
        {
            case "ak":
                return gun1_ak;
            case "de":
                return gun2_de;
            default:
                return null;
        }
    }
}
