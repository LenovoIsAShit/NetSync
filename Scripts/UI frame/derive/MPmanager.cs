using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ö÷Ò³UI¹ÜÀíÆ÷
/// </summary>
public class MPmanager : MonoBehaviour
{
    public PanelManager pm;

    private void Awake()
    {
        pm =  new PanelManager();

        pm.Push(new MainPagePanel());

        
    }

    public GameObject Get_Top_UI()
    {
        return pm.Top();
    }
}
