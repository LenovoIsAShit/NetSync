using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理所有UI对象
/// </summary>
public class PanelManager 
{

    Stack<BasePanel> stackPanel;
    public UIManager uimanager;
    BasePanel now_panel;

    public PanelManager()
    {
        stackPanel = new Stack<BasePanel>();
        uimanager = new UIManager();
        now_panel = null;
    }

    public void Push(BasePanel bp)
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnPause();
        }
        stackPanel.Push(bp);
        GameObject obj = uimanager.GetSingleUi(bp.base_uitype);
    }

    public void Pop()
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnExit();
            stackPanel.Pop();
        }
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnResume();
        }
    }

    public GameObject Top()
    {
        return uimanager.GetSingleUi(stackPanel.Peek().base_uitype);
    }
}
