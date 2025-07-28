using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulQuit_1 : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    void Init()
    {
        Button save=transform.GetChild(0).GetChild(0).GetComponent<Button>();
        save.onClick.AddListener(Save);

        Button quit= transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);

        Button cancel=transform.GetChild(0).GetChild(2).GetComponent<Button>();
        cancel.onClick.AddListener(Cancel); 
    }

    void Save()
    {

        MGM.instance.Saver.GetComponent<Saver>().SaveGame();
    }

    void Quit()
    {
        #if UNITY_EDITOR
                // 如果在编辑器中运行，就停止播放  
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 
                Application.Quit();  
        #endif
    }

    void Cancel()
    {
        var cmp= MGM.instance.UImanager.GetComponent<MultiUImanager>();
        GameObject obj = cmp.pm.Top();
        cmp.pm.Pop();
        MonoBehaviour.Destroy(obj);
    }
}
