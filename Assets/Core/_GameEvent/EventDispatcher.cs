/*
    Event Manager
* 
*/

using System;
using UnityEngine;
using System.Collections.Generic;

public class EventDispatcher
{
    public Dictionary<ValueType, Delegate> eventTable;

    public EventDispatcher()
    {

    }

    // Add event listener
    public void addEventListener(ValueType eventType, Action method)
    {
        if (!recordEvent(eventType, method)) return;
        eventTable[eventType] = Delegate.Combine((Action)eventTable[eventType], method);
    }

    public void addEventListener<T>(ValueType eventType, Action<T> method)
    {
        if (!recordEvent(eventType, method)) return;
        eventTable[eventType] = Delegate.Combine((Action<T>)eventTable[eventType], method);
    }

    public void addEventListener<T, U>(ValueType eventType, Action<T, U> method)
    {
        if (!recordEvent(eventType, method)) return;
        eventTable[eventType] = Delegate.Combine((Action<T, U>)eventTable[eventType], method);
    }

    public void addEventListener<T, U, V>(ValueType eventType, Action<T, U, V> method)
    {
        if (!recordEvent(eventType, method)) return;
        eventTable[eventType] = Delegate.Combine((Action<T, U, V>)eventTable[eventType], method);
    }

    // Remove event listener
    public void removeEventListener(ValueType eventType, Action method)
    {
        if (!removeEvent(eventType, method)) return;
        eventTable[eventType] = (Action)Delegate.Remove((Action)eventTable[eventType], method);
        removeType(eventType);
    }

    public void removeEventListener<T>(ValueType eventType, Action<T> method)
    {
        if (!removeEvent(eventType, method)) return;
        eventTable[eventType] = (Action<T>)Delegate.Remove((Action<T>)eventTable[eventType], method);
        removeType(eventType);
    }

    public void removeEventListener<T, U>(ValueType eventType, Action<T, U> method)
    {
        if (!removeEvent(eventType, method)) return;
        eventTable[eventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)eventTable[eventType], method);
        removeType(eventType);
    }

    public void removeEventListener<T, U, V>(ValueType eventType, Action<T, U, V> method)
    {
        if (!removeEvent(eventType, method)) return;
        eventTable[eventType] = (Action<T, U, V>)Delegate.Remove((Action<T, U, V>)eventTable[eventType], method);
        removeType(eventType);
    }

    // Dispatch an event
    public void dispatchEvent(ValueType eventType)
    {
        if (eventTable == null)
            return;
        if (!eventTable.ContainsKey(eventType)) return;
        Delegate method;
        if (eventTable.TryGetValue(eventType, out method))
        {
            Action CallBack = method as Action;
            if (CallBack != null)
            {
                CallBack();
            }
        }
    }


    public void dispatchEvent<T>(ValueType eventType, T arg)
    {
        if (eventTable == null)
            return;
        if (!eventTable.ContainsKey(eventType))
            return;
        Delegate method;
        if (eventTable.TryGetValue(eventType, out method))
        {
            Action<T> CallBack = method as Action<T>;
            if (CallBack != null)
            {
                CallBack(arg);
            }
        }
    }

    public void dispatchEvent<T, U>(ValueType eventType, T arg1, U arg2)
    {
        if (eventTable == null)
            return;
        if (!eventTable.ContainsKey(eventType)) return;
        Delegate method;
        if (eventTable.TryGetValue(eventType, out method))
        {
            Action<T, U> CallBack = method as Action<T, U>;
            if (CallBack != null)
            {
                CallBack(arg1, arg2);
            }
        }
    }

    public void dispatchEvent<T, U, V>(ValueType eventType, T arg1, U arg2, V arg3)
    {
        if (eventTable == null)
            return;
        if (!eventTable.ContainsKey(eventType)) return;
        Delegate method;
        if (eventTable.TryGetValue(eventType, out method))
        {
            Action<T, U, V> CallBack = method as Action<T, U, V>;
            if (CallBack != null)
            {
                CallBack(arg1, arg2, arg3);
            }
        }
    }

    // record event, if it doesn't already exists
    private bool recordEvent(ValueType eventType, Delegate method)
    {
        if (eventTable == null)
            eventTable = new Dictionary<ValueType, Delegate>();
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, null);
        }
        Delegate d = eventTable[eventType];
        if (d != null)
        {
            if (d.GetType() != method.GetType())
            {
                return false;
            }
            eventTable[eventType] = Delegate.Remove(eventTable[eventType], method);
        }
        return true;
    }

    private bool removeEvent(ValueType eventType, Delegate method)
    {
        if (eventTable == null)
            return false;
        if (eventTable.ContainsKey(eventType))
        {
            Delegate d = eventTable[eventType];

            if (d == null)
            {
                return false;
            }
            else if (d.GetType() != method.GetType())
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    private void removeType(ValueType eventType)
    {
        if (this.eventTable.ContainsKey(eventType) && (this.eventTable[eventType] == null))
        {
            this.eventTable.Remove(eventType);
        }
    }
}