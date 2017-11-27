using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public enum MapType
{
    Normal,
    Move,
    Rotate,
    End
}

public enum RotatePoint
{
    Start,
    Middle
}

public enum TileType
{
    None,       //不改变pos跟dir
    Normal,     //不改变pos，改变dir
    LeftJump,   //改变pos跟dir
    RightJump,  //改变pos跟dir
    Handicap    //生成障碍
}



public class MapManager {

    public bool isLeft = false;
    public bool firstTile = true;

    public float speed = PlayerModel.Instance.Speed;
    public float mapTime = 0f;
    public float tileWidth;
    public float jumpWidth;
    public Transform mapCreatePoint;
    public Transform mapParent;

    public List<MapItem> mapItems = new List<MapItem>();


    public event Action rotateUpdate = null;
    public event Action moveUpdate = null;

    //地图分为3个层次：
    //1，整个地图map
    //2，地图块MapItem，组成map的元素，移动以及旋转以mapItem为单位
    //3，瓷砖tile，组成mapItem的元素，方块

    /// <summary>
    /// 根据配置文件动态创建地图
    /// </summary>
    /// <param name="mapPath"></param>
    public void ReadJsonAndInit(string mapPath)
    {
        string jsonText = ResourcesManager.Instance.LoadAssetByFullName<TextAsset>(mapPath).text;
        JsonData rootData = JsonMapper.ToObject(jsonText);

        tileWidth = (float)(double)rootData["Width"];
        jumpWidth = PlayerModel.Instance.Speed * PlayerModel.Instance.StandJumpTime;

        //用来控制地图生成的位置与方向
        mapCreatePoint = new GameObject("MapCreatePoint").transform;
        mapCreatePoint.position = new Vector3(0, -0.18f, 0);
        mapCreatePoint.forward = Vector3.forward;
        //地图元素的父物体
        mapParent = new GameObject("MapParent").transform;
        mapParent.position = mapCreatePoint.position;

        JsonData maps = rootData["Map"];
        foreach (JsonData map in maps)
        {
            MapItem mapItem = new MapItem();
            mapItem.Init(this);
            mapItem.CreateMapItem(map);
            mapItems.Add(mapItem);
        }
    }

    public void Update()
    {
        if (rotateUpdate != null)
            rotateUpdate();
        if (moveUpdate != null)
            moveUpdate();
    }



}
