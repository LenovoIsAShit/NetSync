using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ��ҳ���̿�����
/// </summary>
public class MainPM : MonoBehaviour
{

    private void Start()
    {
        LoadAllListener();
    }


    /// <summary>
    /// �������м����¼�
    /// </summary>
    void LoadAllListener()
    {
        MPmanager mp = MainGM.instance.MainPM.gameObject.GetComponent<MPmanager>();
        GameObject obj = mp.Get_Top_UI();
        
        Button lastgame = obj.transform.GetChild(1).GetComponent<Button>();
        lastgame.onClick.AddListener(Continue_game);
        //lastgame��ť

        Button online = obj.transform.GetChild(2).GetComponent<Button>();
        online.onClick.AddListener(Online_game);
        //online��ť

        Button quit = obj.transform.GetChild(3).GetComponent<Button>();
        quit.onClick.AddListener(Quit_game);
        //quit��ť
    }


    /// <summary>
    /// ��ҳ��ť-last game�������¼�
    /// </summary>
    void Continue_game()
    {
        IsHost.ishost = true;
        SceneManager.LoadScene("test");
    }

    /// <summary>
    /// ��ҳ��ť-Online�������¼�
    /// </summary>
    void Online_game()
    {
        IsHost.ishost = false;
        SceneManager.LoadScene("test");
    }

    /// <summary>
    /// ��ҳ��ť-Quit�������¼�
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
