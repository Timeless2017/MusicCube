using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleRelation
{
    private AssetBundle assetBundle = null;


    public AssetBundleRelation()
    {
        dependenceBundleList = new List<string>();
        referenceBundleList = new List<string>();
    }

    #region 依赖关系
    /// <summary>
    /// 所有依赖的包名
    /// </summary>
    private List<string> dependenceBundleList;

    /// <summary>
    /// 添加依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void AddDependence(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
            return;
        if (dependenceBundleList.Contains(bundleName))
            return;
        dependenceBundleList.Add(bundleName);
    }

    /// <summary>
    /// 移除依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void RemoveDependence(string bundleName)
    {
        if (dependenceBundleList.Contains(bundleName))
            dependenceBundleList.Remove(bundleName);
    }

    /// <summary>
    /// 获取所有依赖关系
    /// </summary>
    /// <returns></returns>
    public string[] GetAllDependences()
    {
        return dependenceBundleList.ToArray();
    }


    #endregion

    #region 被依赖关系

    /// <summary>
    /// 所有被依赖的包名
    /// </summary>
    private List<string> referenceBundleList;

    /// <summary>
    /// 添加被依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void AddReference(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
            return;
        if (dependenceBundleList.Contains(bundleName))
            return;
        referenceBundleList.Add(bundleName);
    }

    /// <summary>
    /// 移除被依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool RemoveReference(string bundleName)
    {
        if (referenceBundleList.Contains(bundleName))
        {
            referenceBundleList.Remove(bundleName);
            //移除一个包的时候，要做一个判断
            //还有没有被别的包所依赖
            //有，无所谓
            if (referenceBundleList.Count > 0)
                return false;
            //没有，就需要释放这个AssetBundle
            else
                return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 获取所有被依赖关系
    /// </summary>
    /// <returns></returns>
    public string[] GetAllReferences()
    {
        return referenceBundleList.ToArray();
    }

    #endregion

    /// <summary>
    /// 加载包
    /// </summary>
    /// <param name="bundlePath"></param>
    public void LoadAssetBundle(string bundlePath)
    {
        assetBundle = AssetBundle.LoadFromFile(bundlePath);
    }

    /// <summary>
    /// 卸载包
    /// </summary>
    public void UnloadAssetBundle()
    {
        assetBundle.Unload(false);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string assetName) where T : Object
    {
        if (assetBundle == null)
        {
            Debug.LogError("当前assetBundle为空，无法获取" + assetName + "资源");
            return null;
        }
        return assetBundle.LoadAsset<T>(assetName);
    }

}
