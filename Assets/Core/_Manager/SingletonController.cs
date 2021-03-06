﻿using System;
using System.Collections.Generic;


public class SingletonController<T> : EventDispatcher
{

    protected GlobalDispatcher globalDispatcher = GlobalDispatcher.Instance;

    private static T ms_instance;
    public static T Instance
    {
        get
        {
            if (ms_instance == null)
            {
                ms_instance = (T)Activator.CreateInstance(typeof(T));
            }

            return ms_instance;
        }
    }


    public virtual void Initialize() { }

    public virtual void UnInitialize() { }
}