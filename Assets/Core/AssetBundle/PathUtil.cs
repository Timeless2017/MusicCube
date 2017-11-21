using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 路径
/// </summary>
public class PathUtil
{


    /// <summary>
    /// 获取对应平台的名字
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
#if UNITY_ANDROID
        return "Android";
#elif UNITY_IOS
        return "IOS";
#elif UNITY_STANDALONE
        return "Windows";
#else
        return "";
#endif

    }
    public static string ASSETBUNDLE_COMMON_PATH = Application.dataPath + "/StreamingAssets/AssetBundles";


    //不用直接使用Application.streamingAssetsPath,这样的出来的安卓版路径只能用www加载
    public static string ASSETBUNDLE_PLATFORM_PATH
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + "/StreamingAssets/AssetBundles/" + GetPlatformName() + "/";
#elif UNITY_IOS
            return  Application.dataPath + "/Raw/AssetBundles/"+GetPlatformName()+"/";
#elif UNITY_ANDROID
			return Application.dataPath+"!assets/AssetBundles/"+GetPlatformName()+"/";
#elif UNITY_STANDALONE
			return Application.dataPath + "/StreamingAssets/AssetBundles/"+GetPlatformName()+"/";
#else
            Debug.Log("Just for iOS,Android and Standalone only.");
            return "";
#endif
        }
    }

}
