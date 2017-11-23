using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonController<GameController>{
    MapManager mapManager;
    PlayerController playerCtrl;
    private float playTime = 0;
    private float offsetTime = 0f;

    public override void Initialize()
    {
        base.Initialize();
        globalDispatcher.addEventListener<MapId>(EventName.EnterGame, EnterGame);
        globalDispatcher.addEventListener<float>(EventName.GameLoop, Update);
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
        globalDispatcher.removeEventListener<MapId>(EventName.EnterGame, EnterGame);
        globalDispatcher.removeEventListener<float>(EventName.GameLoop, Update);
    }

    //显示GameView，创建地图，创建角色
    private void EnterGame(MapId mapId)
    {
        UIManager.Instance.ShowPanel<GameView>();
        mapManager = new MapManager();
        mapManager.ReadJsonAndInit("Datas/" + mapId.ToString() + ".json");
        playerCtrl = new PlayerController();
        playerCtrl.CreatePlayer("Player/cube_car");
        
        SetGamePause(true);
    }

    public void SetGamePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            AudioManager.Instance.SetMute(true);
        }
        else
        {
            Time.timeScale = 1;
            AudioManager.Instance.SetMute(false);
            AudioManager.Instance.PlayBackground("Audio/First.mp3");

            Camera.main.GetComponent<Animator>().speed = 1;
        }
    }

    private void Update(float deltaTime)
    {
        if (mapManager != null && offsetTime <= 0)
            mapManager.Update();
        if (playerCtrl != null)
        {
            playTime += deltaTime;
            offsetTime -= deltaTime;
            if (offsetTime <= 0)
                playerCtrl.Update();
        }
        
        Debug.Log(playTime);
    }

}
