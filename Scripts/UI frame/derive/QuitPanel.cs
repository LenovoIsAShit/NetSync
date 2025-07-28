using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÍË³öÃæ°å
/// </summary>
public class QuitPanel : BasePanel
{
    public string path="prefabs/Quit";

    public QuitPanel() : base(new UIType("",""))
    {
        base.base_uitype.path = path;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
