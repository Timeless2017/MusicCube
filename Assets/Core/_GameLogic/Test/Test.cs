using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum TestEventType    
{
    Test1,
    Test2,
}

public class Test : MonoBehaviour {

    EventTrigger eventTrigger;

    void Start () {
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry myClick = new EventTrigger.Entry();
        myClick.eventID = EventTriggerType.PointerClick;    
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(Test1);
        myClick.callback.AddListener(callback);

        eventTrigger.triggers.Add(myClick);
    }
      

    public void Test1(BaseEventData date)
    {
        Debug.Log("Click");
    }

    void Update()
    {
        
    }
}
