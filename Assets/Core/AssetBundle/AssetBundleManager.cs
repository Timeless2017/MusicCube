using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleManager
{

    #region 单例模式
    private static AssetBundleManager _instance;
    public static AssetBundleManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AssetBundleManager();
            return _instance;
        }
    }
    #endregion

    public AssetBundleManifest manifest;
    private Dictionary<string, AssetBundleRelation> nameBundleDict;

    public AssetBundleManager()
    {
        LoadManifest();
        nameBundleDict = new Dictionary<string, AssetBundleRelation>();
    }

    /// <summary>
    /// 加载Manifest文件
    /// </summary>
    private void LoadManifest()
    {
        string manifestPath = PathUtil.ASSETBUNDLE_PLATFORM_PATH + "/" + PathUtil.GetPlatformName();
        AssetBundle manifestAB = AssetBundle.LoadFromFile(manifestPath);
        manifest = manifestAB.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">EditorResources后的路径</param>
    /// <returns></returns>
    public T LoadAsset<T>(string path) where T : Object
    {
        //获取包名
        int lastSlashIndex = path.LastIndexOf('/');
        string folderName = path.Remove(lastSlashIndex);
        string bundleName = GetBundleName(folderName);
        //如果资源包还没加载，则加载
        if (!nameBundleDict.ContainsKey(bundleName))
            LoadAssetBundle(bundleName);
        //获取资源名，加载资源
        string assetName = path.Substring(lastSlashIndex + 1);
        return nameBundleDict[bundleName].LoadAsset<T>(assetName);
    }

    /// <summary>
    /// 获取包名[xxx/xxx/xxx.assetbundle(.u3d)]
    /// </summary>
    /// <param name="path"></param>
    /// <param name="extensionIsU3d"></param>
    /// <returns></returns>
    private string GetBundleName(string folderName, bool extensionIsU3d = false)
    {
        if (string.IsNullOrEmpty(folderName))
            return "";
        if (extensionIsU3d)
            return folderName.ToLower() + ".u3d";
        else
            return folderName.ToLower() + ".assetbundle";
    }


    #region 加载包
    /// <summary>
    /// 加载资源包
    /// </summary>
    /// <param name="path">xxx/xxx/xxx.assetbundle(.u3d)</param>
    private void LoadAssetBundle(string bundleName)
    {
        AssetBundleRelation assetBundleRelation = new AssetBundleRelation();
        nameBundleDict.Add(bundleName, assetBundleRelation);
        //加载依赖的包
        LoadDependenceAB(bundleName);
        //加载自己的包
        string bundlePath = PathUtil.ASSETBUNDLE_PLATFORM_PATH + "/" + bundleName;
        assetBundleRelation.LoadAssetBundle(bundlePath);
    }
    /// <summary>
    /// 加载依赖的包
    /// </summary>
    private void LoadDependenceAB(string bundleName)
    {
        string[] depedencies = manifest.GetAllDependencies(bundleName);
        foreach (string depedence in depedencies)
        {
            //添加依赖关系
            nameBundleDict[bundleName].AddDependence(depedence);
            //如果依赖的包还没加载，则加载
            if (!nameBundleDict.ContainsKey(depedence))
                LoadAssetBundle(depedence);
            //依赖的包添加被依赖关系
            nameBundleDict[depedence].AddReference(bundleName);
        }
    }
    #endregion

    #region 卸载包

    /// <summary>
    /// 卸载包
    /// </summary>
    public void UnloadAssetBundle(string path, bool isBundleName = false)
    {
        //获取包名
        string bundleName = path;
        if (!isBundleName)
            bundleName = GetBundleName(path);
        //安全校验
        if (!nameBundleDict.ContainsKey(bundleName))
        {
            Debug.Log(bundleName + "不在Dictionary中，可能未被加载或已被卸载");
            return;
        }
        //
        AssetBundleRelation relation = nameBundleDict[bundleName];
        string[] depedencies = relation.GetAllDependences();
        foreach (string depedence in depedencies)
        {
            nameBundleDict[depedence].RemoveReference(bundleName);
            UnloadAssetBundle(depedence, true);
        }
        if(relation.GetAllReferences().Length <= 0)
        {
            relation.UnloadAssetBundle();
            nameBundleDict.Remove(bundleName);
        }
    }

    /// <summary>
    /// 卸载文件夹下所有AssetBundle
    /// </summary>
    /// <param name="path"></param>
    public void UnloadFolder(string path)
    {
        string folderName = path.ToLower();
        List<string> subABNames = new List<string>();
        foreach (string tempName in nameBundleDict.Keys)
        {
            if(tempName.IndexOf(folderName) == 0)
                subABNames.Add(tempName);
        }
        foreach (string subABName in subABNames)
        {
            UnloadAssetBundle(subABName, true);
        }
    }



#endregion

    #region 调试方法
    /// <summary>
    /// 返回所有已加载的AssetBundle
    /// </summary>
    /// <returns></returns>
    public string[] GetAllLoadedAB()
    {
        List<string> allLoadedAB = new List<string>();
        foreach (string loadedAb in nameBundleDict.Keys)
        {
            allLoadedAB.Add(loadedAb);
        }
        return allLoadedAB.ToArray();
    }
    #endregion
}
