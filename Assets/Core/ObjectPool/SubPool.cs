using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool {

    private GameObject prefab;
    private List<PoolItemBase> objList = new List<PoolItemBase>();

    public SubPool(PoolItemBase poolItem)
    {
        this.prefab = poolItem.root;
    }

    public string PoolName
    {
        get { return prefab.name; }
    }

    public GameObject Spawn()
    {
        PoolItemBase obj = null;
        foreach (PoolItemBase item in objList)
        {
            if (!item.root.activeSelf)
            {
                obj = item;
                break;
            }
        }
        if(obj == null)
        {
            obj = new PoolItemBase();
            obj.root = GameObject.Instantiate(prefab);
            objList.Add(obj);
        }
        obj.root.SetActive(true);
        obj.Spawn();
        return obj.root;
    }

    public void UnSpawn(GameObject gameObject)
    {
        foreach (PoolItemBase item in objList)
        {
            if (gameObject == item.root)
            {
                item.UnSpawn();
                item.root.SetActive(false);
                return;
            }
        }
    }


}
