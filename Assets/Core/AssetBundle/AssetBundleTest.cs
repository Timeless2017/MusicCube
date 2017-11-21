using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleTest : MonoBehaviour {


	void Start () {
        /*
        GameObject pfb1 = AssetBundleManager.Instance.LoadAsset<GameObject>("01/Cube");
        Instantiate(pfb1);
        //依赖其他包的包
        GameObject pfb2 = AssetBundleManager.Instance.LoadAsset<GameObject>("02/Obj/Card");
        Instantiate(pfb2);
        //带子物体的资源
        GameObject pfb3 = AssetBundleManager.Instance.LoadAsset<GameObject>("01/Car/Obj");
        Instantiate(pfb3);
        //卸载包
        //AssetBundleManager.Instance.UnloadAssetBundle("02/Obj");
        //卸载文件夹下所有包
        AssetBundleManager.Instance.UnloadFolder("01");
        //还没写
        //AssetBundleManager.Instance.LoadAsset<UnityEngine.SceneManagement.Scene>("01/01");
        */
        GameObject pfb1 = AssetBundleManager.Instance.LoadAsset<GameObject>("Prefab/Player/cube_dice");
        Instantiate(pfb1);




        string[] allLoadedAB = AssetBundleManager.Instance.GetAllLoadedAB();
        foreach (string loadedAB in allLoadedAB)
        {
            Debug.Log(loadedAB);
        }
    }
	
}
