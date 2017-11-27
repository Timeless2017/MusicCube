using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Test : MonoBehaviour {

    MapManager mapManager;
    PlayerController playerController;
    void Start () {
        PlayerModel.Instance.Initialize();
        playerController = new PlayerController();
        mapManager = new MapManager();
        mapManager.ReadJsonAndInit("Datas/First.json");
        playerController.CreatePlayer("Player/cube_car");
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), Time.time.ToString());
    }


    void Update()
    {
        playerController.Update();
        mapManager.Update();
    }
}
