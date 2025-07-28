using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 所有UI对象的基类 
/// </summary>
public abstract class BasePanel
{
    public UIType base_uitype;

    public BasePanel(UIType base_uitype)
    {
        this.base_uitype = base_uitype;
    }



    /// <summary>
    /// 类似awake，进入时
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 暂停时
    /// </summary>
    public virtual void OnPause() { }

    /// <summary>
    /// 停止暂停，继续时
    /// </summary>
    public virtual void OnResume() { }

    /// <summary>
    /// 退出时
    /// </summary>
    public virtual void OnExit() { }
}
