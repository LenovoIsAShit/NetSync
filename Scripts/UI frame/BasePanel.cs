using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����UI����Ļ��� 
/// </summary>
public abstract class BasePanel
{
    public UIType base_uitype;

    public BasePanel(UIType base_uitype)
    {
        this.base_uitype = base_uitype;
    }



    /// <summary>
    /// ����awake������ʱ
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// ��ͣʱ
    /// </summary>
    public virtual void OnPause() { }

    /// <summary>
    /// ֹͣ��ͣ������ʱ
    /// </summary>
    public virtual void OnResume() { }

    /// <summary>
    /// �˳�ʱ
    /// </summary>
    public virtual void OnExit() { }
}
