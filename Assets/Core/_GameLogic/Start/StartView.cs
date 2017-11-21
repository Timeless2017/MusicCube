using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartView : UIPanelBase {

    private void Init()
    {
        EventTrigger trigger = skin.AddComponent<EventTrigger>();

        EventTrigger.Entry myClick = new EventTrigger.Entry();
        myClick.eventID = EventTriggerType.PointerClick;
        myClick.callback.AddListener((data) => StartController.Instance.EnterMenuView());
        trigger.triggers.Add(myClick);
    }



    public StartView()
    {
        layer = UILayer.First;
        skinPath = "View/Start/StartView";
    }

#region 生命周期

    public override void OnLoad()
    {
        base.OnLoad();
        Init();
    }

    public override void OnShow(params object[] args)
    {
        base.OnShow(args);
    }

    public override void OnHide()
    {
        base.OnHide();
    }

#endregion
}
