using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>{

    //画板
    private GameObject canvas;
    //面板
    private Dictionary<UIPanelID, UIPanelBase> panelDict;
    //层级
    private Dictionary<UILayer, Transform> layerDict;


    public void Init()
    {
        InitLayer();
        panelDict = new Dictionary<UIPanelID, UIPanelBase>();
    }

    /// <summary>
    /// 初始化层级
    /// </summary>
    private void InitLayer()
    {
        //画布
        canvas = GameObject.Find("Canvas");
        if (canvas == null)
            Debug.LogError("找不到Canvas");
        layerDict = new Dictionary<UILayer, Transform>();
        foreach (UILayer ul in System.Enum.GetValues(typeof(UILayer)))
        {
            string name = ul.ToString();
            Transform transform = canvas.transform.FindChild(name);
            layerDict.Add(ul, transform);
        }
    }

#region 面板方法

    //UIManager只负责面板的加载、显示与隐藏，其他面板方法由对应Controller调用

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    public void ShowPanel<T>(params object[] args) where T : UIPanelBase
    {
        UIPanelID id = (UIPanelID)System.Enum.Parse(typeof(UIPanelID), typeof(T).ToString());
        //如果该面板没被加载过，则加载
        if (!panelDict.ContainsKey(id))
            LoadPanel<T>();
        //如果该面板正显示着，则不理
        if (panelDict[id].isShow)
            return;
        panelDict[id].skin.SetActive(true);
        panelDict[id].isShow = true;
        panelDict[id].OnShow(args);
    }

    /// <summary>
    /// 加载面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void LoadPanel<T>() where T : UIPanelBase
    {
        UIPanelID id = (UIPanelID)System.Enum.Parse(typeof(UIPanelID), typeof(T).ToString());
        UIPanelBase panel = System.Activator.CreateInstance(typeof(T)) as UIPanelBase;
        panelDict.Add(id, panel);
        //加载皮肤
        string skinPath = panel.skinPath;
        GameObject skin = ResourcesManager.Instance.LoadPrefab(skinPath);
        if (skin == null)
            Debug.LogError("找不到路径" + skinPath + "的面板");
        panel.skin = GameObject.Instantiate(skin);
        //设置父物体
        panel.skin.transform.SetParent(layerDict[panel.layer], false);
        //初始化，获取控件
        panel.OnLoad();
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HidePanel<T>() where T : UIPanelBase
    {
        UIPanelID id = (UIPanelID)System.Enum.Parse(typeof(UIPanelID), typeof(T).ToString());
        if (!panelDict.ContainsKey(id))
            return;
        if (!panelDict[id].isShow)
            return;
        panelDict[id].OnHide();
        panelDict[id].isShow = false;
        panelDict[id].skin.SetActive(false);
    }

#endregion

}
