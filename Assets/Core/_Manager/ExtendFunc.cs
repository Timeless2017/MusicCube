using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendFunc {

    public static GameObject FindExt(this GameObject obj, string path)
    {
        Transform go = obj.transform.Find(path);
        if (null == go)
        {
            return null;
        }
        else
        {
            return go.gameObject;
        }
    }

    public static T GetChildComponet<T>(this GameObject obj, string path)
    {
        return obj.FindExt(path).GetComponent<T>();
    }
}
