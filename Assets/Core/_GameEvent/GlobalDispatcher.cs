using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDispatcher : EventDispatcher {

    private static GlobalDispatcher _instance;
    public static GlobalDispatcher Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GlobalDispatcher();
            return _instance;
        }
    }

}
