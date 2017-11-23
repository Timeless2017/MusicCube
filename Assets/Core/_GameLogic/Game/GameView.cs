using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameView : UIPanelBase {

    private Button btn_Strength, btn_Coin;
    private GameObject pauseMask;

    public GameView()
    {
        layer = UILayer.First;
        skinPath = "View/Game/GameView";
    }

    private void Init()
    {
        btn_Strength = skin.GetChildComponet<Button>("Btn_Strength");
        btn_Coin = skin.GetChildComponet<Button>("Btn_Coin");
        pauseMask = skin.FindExt("PauseMask");

        EventTrigger pauseTrigger = pauseMask.AddComponent<EventTrigger>();
        EventTrigger.Entry pauseClick = new EventTrigger.Entry();
        pauseClick.eventID = EventTriggerType.PointerClick;
        pauseClick.callback.AddListener((BaseEventData data) => {
            GameController.Instance.SetGamePause(false);
            pauseMask.SetActive(false);
            });

        pauseTrigger.triggers.Add(pauseClick);
    }

    public override void OnLoad()
    {
        base.OnLoad();
        Init();
    }
}
