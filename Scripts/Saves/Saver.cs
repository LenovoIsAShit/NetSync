using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// �ṩ��/��������
/// </summary>
public class Saver :MonoBehaviour
{
    string path = Application.dataPath+"/"+"Save"+"/"+"save.json";
    public void SaveGame()
    {
        GameData gd = Get_now_GameData();
        string json = JsonUtility.ToJson(gd);
        File.WriteAllText(path, json);
    }

    public GameData LoadGame()
    {
        string json=File.ReadAllText(path);
        if (json == "") return null;
        GameData gd=JsonUtility.FromJson<GameData>(json);
        return gd;
    }


    /// <summary>
    /// ��ȡ��ǰ��Ϸ��Ϣ
    /// </summary>
    /// <returns></returns>
    GameData Get_now_GameData()
    {
        GameData gd = new GameData();
        gd.pos = MGM.instance.p1.transform.position;
        gd.wp = MGM.instance.p1.GetComponent<player>().wp_now;
        gd.hp = MGM.instance.p1.GetComponent<player>().hp;
        //�����������

        gd.score = MGM.instance.score.GetComponent<ScoreManager>().score_now;
        gd.level = MGM.instance.score.GetComponent<ScoreManager>().level_now;
        //�ؿ����

        gd.killneed = MGM.instance.MonsterManager.GetComponent<MonsterManager>().res;

        return gd;
    }
}
