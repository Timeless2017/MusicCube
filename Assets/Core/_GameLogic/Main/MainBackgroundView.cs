using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBackgroundView : UIPanelBase {

    private Button btn_Strength, btn_Coin;
    private Toggle toggle_Level, toggle_Player, toggle_Match, toggle_Setting;
    //private List<Toggle> toggleList = new List<Toggle>();


    public MainBackgroundView()
    {
        layer = UILayer.First;
        skinPath = "View/Main/MainBackgroundView";
    }

    private void Init()
    {
        btn_Strength = skin.GetChildComponet<Button>("Btn_Strength");
        btn_Coin = skin.GetChildComponet<Button>("Btn_Coin");
        toggle_Level = skin.GetChildComponet<Toggle>("BottomBar/Toggle_Level");
        toggle_Player = skin.GetChildComponet<Toggle>("BottomBar/Toggle_Player");
        toggle_Match = skin.GetChildComponet<Toggle>("BottomBar/Toggle_Match");
        toggle_Setting = skin.GetChildComponet<Toggle>("BottomBar/Toggle_Setting");

        toggle_Level.onValueChanged.AddListener((bool active) => MainBackgroundController.Instance.SetPanelShowHide<LevelView>(active));

        MainBackgroundController.Instance.SetPanelShowHide<LevelView>(true);
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
