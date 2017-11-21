using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class AssetBundleEditor
{

    

    #region 自动标记
    //思路
    //1.找到资源保存的文件夹
    //2.遍历所有的文件系统
    //4.如果访问的是文件夹，再继续访问里面所有的文件系统，直到找到文件（递归）
    //5.找到文件，修改它的assetbundle labels label是该文件EditorResources之后的文件夹名
    //6.用AssetBundleImporter类 修改名称和后缀

    private static string DirectoryName = "EditorResources";


    [MenuItem("AssetBundle/设置AssetBundle标签")]
    public static void SetAssetBundleLabels()
    {
        //移除所有没有使用的标记
        AssetDatabase.RemoveUnusedAssetBundleNames();

        //1.找到资源保存的文件夹
        string assetDirectory = Application.dataPath + "/" + DirectoryName;

        DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);
        //遍历文件夹里的所有文件系统
        OnFileSystemInfo(directoryInfo);
        AssetDatabase.Refresh();
        Debug.Log("设置成功");
    }

    /// <summary>
    /// 遍历文件夹里的所有文件系统
    /// </summary>
    private static void OnFileSystemInfo(DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.Exists)
        {
            Debug.LogError(directoryInfo.FullName + "不存在");
            return;
        }
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (FileSystemInfo tmpFileSystemInfo in fileSystemInfos)
        {
            FileInfo fileInfo = tmpFileSystemInfo as FileInfo;
            if (fileInfo == null)
            {
                //代表强转失败，不是文件，而是文件夹
                //所以要再继续访问里面的文件系统，直到找到文件（递归）
                OnFileSystemInfo(tmpFileSystemInfo as DirectoryInfo);
            }
            else
            {
                //代表是文件
                //5.找到文件 就要修改他的 assetbundle labels
                SetLabels(fileInfo);
            }
        }
    }

    /// <summary>
    /// 修改资源文件的assetbundle labels
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="moduleName"></param>
    /// <param name="namePathDict"></param>
    private static void SetLabels(FileInfo fileInfo)
    {
        //对unity自身生成的meta文件，无视
        if (fileInfo.Extension == ".meta")
            return;
        //获取包名
        string bundleName = GetBundleName(fileInfo);
        //Debug.Log(bundleName);
        if (string.IsNullOrEmpty(bundleName))
            return;
        //得到文件的AssetImporter
        int index = fileInfo.FullName.IndexOf("Assets");
        string assetPath = fileInfo.FullName.Substring(index);
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
        //用 AssetImporter类修改名称和后缀
        assetImporter.assetBundleName = bundleName.ToLower();
        if (fileInfo.Extension == ".unity") //场景
            assetImporter.assetBundleVariant = "u3d";
        else
            assetImporter.assetBundleVariant = "assetbundle";
    }

    /// <summary>
    /// 根据FileInfo获取包名
    /// </summary>
    private static string GetBundleName(FileInfo fileInfo)
    {
        string windowPath = fileInfo.FullName;
        //转化为unity可识别的路径
        string unityPath = windowPath.Replace(@"\", "/");
        //截取fullName在EditorResources之后的部分
        string assetDirectory = Application.dataPath + "/" + DirectoryName;
        string subName = unityPath.Substring(assetDirectory.Length + 1);
        if (!subName.Contains("/"))
        {
            Debug.LogError(subName + "不应该放在" + DirectoryName + "文件夹下");
            return "";
        }
        int index = subName.LastIndexOf('/');
        return subName.Remove(index);
    }
    #endregion



    #region 打包

    [MenuItem("AssetBundle/打包当前平台AssetBundles")]
    private static void BuildAllAssetBundles()
    {
#if UNITY_IOS
        BuildAssetsBundle(BuildTarget.iOS);
#elif UNITY_ANDROID
        BuildAssetsBundle(BuildTarget.Android);
#elif UNITY_STANDALONE
        BuildAssetsBundle(BuildTarget.StandaloneWindows);
#else
        Debug.Log("Just for iOS,Android and Standalone only.");
        return;
#endif
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 打包方法
    /// </summary>
    /// <param name="target"></param>
    public static void BuildAssetsBundle(BuildTarget target)
    {
        

        if (Directory.Exists(PathUtil.ASSETBUNDLE_COMMON_PATH + "/"))
        {
            Directory.Delete(PathUtil.ASSETBUNDLE_COMMON_PATH + "/", true);
            File.Delete(PathUtil.ASSETBUNDLE_COMMON_PATH + ".meta");
            AssetDatabase.Refresh();
        }
        Directory.CreateDirectory(string.Format("{0}/StreamingAssets/AssetBundles/{1}/", Application.dataPath, PathUtil.GetPlatformName()));
        AssetDatabase.Refresh();


        //string outPath = string.Format("{0}/StreamingAssets/{1}", Application.dataPath, PathUtil.GetPlatformName());
        string outPath = string.Format("Assets/StreamingAssets/AssetBundles/{0}/", PathUtil.GetPlatformName());
        Debug.Log(outPath);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, target);
    }

    #endregion

    #region 一键删除

    [MenuItem("AssetBundle/删除所有AssetBundle")]
    private static void DelectAssetBundles()
    {
        string outPath = string.Format("Assets/StreamingAssets/AssetBundles/{0}/",PathUtil.GetPlatformName());

        Directory.Delete(outPath, true);
        File.Delete(outPath + ".meta");

        AssetDatabase.Refresh();
    }

    #endregion

}
