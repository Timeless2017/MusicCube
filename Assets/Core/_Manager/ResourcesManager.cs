using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{

    public bool useAssetsBundleInEditor = false;

    /// <summary>
    /// 通过EditorResources后的完整路径(需后缀名)加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">EditorResources后的完整路径(需后缀名)</param>
    /// <returns></returns>
    public T LoadAssetByFullName<T>(string path) where T : Object
    {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE) && !UNITY_EDITOR
        path = path.Remove(path.LastIndexOf('.'));
        return AssetBundleManager.Instance.LoadAsset<T>(path);
#else
        if (useAssetsBundleInEditor)
        {
            path = path.Remove(path.LastIndexOf('.'));
            return AssetBundleManager.Instance.LoadAsset<T>(path);
        }
        else
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/EditorResources/" + path);
        }
#endif
    }

    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="pathName">EditorResources/Prefab后的路径</param>
    /// <returns></returns>
    public GameObject LoadPrefab(string pathName)
    {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE) && !UNITY_EDITOR
        return AssetBundleManager.Instance.LoadAsset<GameObject>(string.Format("prefab/{0}", pathName));
#else
        if (useAssetsBundleInEditor)
        {
            return AssetBundleManager.Instance.LoadAsset<GameObject>(string.Format("prefab/{0}", pathName));
        }
        else
        {
            string path = string.Format("Assets/EditorResources/Prefab/{0}.prefab", pathName);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
#endif
    }


    //public Sprite LoadSprite(string pathName)
    //{

    //}

}
