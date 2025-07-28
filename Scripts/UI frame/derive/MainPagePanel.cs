using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPagePanel : BasePanel
{
    public string path = "prefabs/MainPage";

    public MainPagePanel() : base(new UIType("", ""))
    {
        base.base_uitype.path = path;
    }


}
