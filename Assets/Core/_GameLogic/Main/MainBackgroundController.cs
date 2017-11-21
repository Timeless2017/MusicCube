using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBackgroundController : SingletonController<MainBackgroundController> {

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }

    public void SetPanelShowHide<T>(bool active) where T : UIPanelBase
    {
        if (active)
            UIManager.Instance.ShowPanel<T>();
        else
            UIManager.Instance.HidePanel<T>();
    }

}
