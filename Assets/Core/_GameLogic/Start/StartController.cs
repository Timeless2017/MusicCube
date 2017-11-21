using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : SingletonController<StartController> {

    public override void Initialize()
    {
        globalDispatcher.addEventListener(EventName.ShowStartView, ShowStartView);
    }

    public override void UnInitialize()
    {
        globalDispatcher.removeEventListener(EventName.ShowStartView, ShowStartView);
    }

    private void ShowStartView()
    {
        UIManager.Instance.ShowPanel<StartView>();
    }

    public void EnterMenuView()
    {
        //Debug.Log("StartViewClick");
        UIManager.Instance.HidePanel<StartView>();
        UIManager.Instance.ShowPanel<MainBackgroundView>();
    }


}
