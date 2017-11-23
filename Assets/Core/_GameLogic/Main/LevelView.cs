using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelView : UIPanelBase {

    private Button firstLevel;

    public LevelView()
    {
        layer = UILayer.First;
        skinPath = "View/Main/LevelView";
    }

    public override void OnLoad()
    {
        base.OnLoad();
        firstLevel = skin.GetChildComponet<Button>("Grid/LevelItem/Image");
        firstLevel.onClick.AddListener(() => LevelController.Instance.EnterGame(MapId.First));
    }

}
