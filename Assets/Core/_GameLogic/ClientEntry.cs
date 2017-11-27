using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientEntry : MonoBehaviour {

    private AudioSource bgAudioSource;
    private AudioSource uiAudioSource;

    //是否在Editor环境下使用AssetBundle
    public bool useAssetsBundleInEditor = false;


    void Start () {
        bgAudioSource = transform.Find("AudioBackground").GetComponent<AudioSource>();
        uiAudioSource = transform.Find("AudioUI").GetComponent<AudioSource>();
        AudioManager.Instance.Init(bgAudioSource, uiAudioSource);

        ResourcesManager.Instance.useAssetsBundleInEditor = useAssetsBundleInEditor;

        Client.Instance.Init(this);

        DontDestroyOnLoad(this);
    }
	

	void Update () {
		if(Client.Instance.inited)
        {
            Client.Instance.Update();
        }
	}
}
