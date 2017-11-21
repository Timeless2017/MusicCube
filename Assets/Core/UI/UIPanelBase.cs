using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase {

    public string skinPath;
    public GameObject skin;
    public UILayer layer;
    public bool isShow;

    public virtual void OnLoad() { }

    public virtual void OnShow(params object[] args) {
    }

    public virtual void OnHide() {
    }

}
