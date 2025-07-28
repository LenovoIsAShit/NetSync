using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多人游戏UI管理
/// </summary>
public class MultiUImanager : MonoBehaviour
{
    public PanelManager pm;

    private void Awake()
    {
        pm = new PanelManager();
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pm.Push(new MulQuit());
        }

        
    }

    public GameObject Get_Top_UI()
    {
        return pm.Top();
    }


}
