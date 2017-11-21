using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItemBase
{
    public GameObject root;
    public float lastUnSpawnTime;

    public virtual void Spawn()
    {

    }

    public virtual void UnSpawn()
    {

    }

}
