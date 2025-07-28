using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 主页流程控制器
/// </summary>
public class MainPM : MonoBehaviour
{

    private void Start()
    {
        LoadAllListener();
    }


    /// <summary>
    /// 挂载所有监听事件
    /// </summary>
    void LoadAllListener()
    {
        MPmanager mp = MainGM.instance.MainPM.gameObject.GetComponent<MPmanager>();
        GameObject obj = mp.Get_Top_UI();
        
        Button lastgame = obj.transform.GetChild(1).GetComponent<Button>();
        lastgame.onClick.AddListener(Continue_game);
        //lastgame按钮

        Button online = obj.transform.GetChild(2).GetComponent<Button>();
        online.onClick.AddListener(Online_game);
        //online按钮

        Button quit = obj.transform.GetChild(3).GetComponent<Button>();
        quit.onClick.AddListener(Quit_game);
        //quit按钮
    }


    /// <summary>
    /// 主页按钮-last game，监听事件
    /// </summary>
    void Continue_game()
    {
        IsHost.ishost = true;
        SceneManager.LoadScene("test");
    }

    /// <summary>
    /// 主页按钮-Online，监听事件
    /// </summary>
    void Online_game()
    {
        IsHost.ishost = false;
        SceneManager.LoadScene("test");
    }

    /// <summary>
    /// 主页按钮-Quit，监听事件
    /// </summary>
    void Quit_game()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();  
        #endif
    }
}
