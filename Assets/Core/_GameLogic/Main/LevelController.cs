using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapId
{
    First,
}

public class LevelController : SingletonController<LevelController> {

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }

    //隐藏LevelView、MainBackgroundView，
    public void EnterGame(MapId mapId)
    {
        UIManager.Instance.HidePanel<LevelView>();
        UIManager.Instance.HidePanel<MainBackgroundView>();
        globalDispatcher.dispatchEvent<MapId>(EventName.EnterGame, MapId.First);
    }
}
