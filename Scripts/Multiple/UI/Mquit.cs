using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 多人游戏退出面板
/// </summary>
public class Mquit : BasePanel
{
    public string path = "prefabs/QuitMul";

    public Mquit() : base(new UIType("", ""))
    {
        base.base_uitype.path = path;
    }

}
