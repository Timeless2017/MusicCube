using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : Singleton<Client> {

    //private GameObject pfb_Player;

    //private MapManager mapManager;
    //private PlayerController player;
    private ClientEntry clientEntry;

    public bool inited = false;

    public void Init(ClientEntry clientEntry)
    {
        this.clientEntry = clientEntry;

        UIManager.Instance.Init();

        LogicModules.Instance.Initialize();

        inited = true;

        GlobalDispatcher.Instance.dispatchEvent(EventName.ShowStartView);
    }

    void Start () {

        //pfb_Player = Resources.Load<GameObject>("");
        //GameObject go_Player = new GameObject("Player");


        //player = new PlayerController();
        //mapManager = new MapManager();
        //player.Init(go_Player);
        //mapManager.ReadJsonAndInit("Datas/music1");
	}
	

	public void Update () {
        //mapManager.Update();
        GlobalDispatcher.Instance.dispatchEvent<float>(EventName.GameLoop, Time.deltaTime);
    }

    private void CreatePlayer()
    {

    }


    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 10, 80, 50), Time.time.ToString());
    //}
}
