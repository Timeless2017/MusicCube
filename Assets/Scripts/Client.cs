using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

    private GameObject pfb_Player;

    private MapManager mapManager;
    private PlayerController player;

    void Start () {

        pfb_Player = Resources.Load<GameObject>("");
        GameObject go_Player = new GameObject("Player");


        player = new PlayerController();
        mapManager = new MapManager();
        player.Init(go_Player);
        mapManager.ReadJsonAndInit("Datas/music1");
	}
	

	void Update () {
        mapManager.Update();

    }

    private void CreatePlayer()
    {

    }


    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 80, 50), Time.time.ToString());
    }
}
