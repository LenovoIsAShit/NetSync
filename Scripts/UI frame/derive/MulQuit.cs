using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����˳����
/// </summary>
public class MulQuit : BasePanel
{
    public string path = "prefabs/QuitMul";

    public MulQuit() : base(new UIType("", ""))
    {
        base.base_uitype.path = path;
    }

    public override void OnExit()
    {
        
    }

}
