using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : Singleton<Client> {

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

	public void Update () {
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
