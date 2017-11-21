using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class MapItem
{

    private MapType mapType;
    private MapManager mapMgr;
    private Transform mapItem;

    public void Init(MapManager mapManager)
    {
        this.mapMgr = mapManager;
    }

    public void CreateTiles(JsonData map)
    {
        mapType = (MapType)Enum.Parse(typeof(MapType), (string)map["MapType"]);
        switch (mapType)
        {
            case MapType.Normal:
                CreateMapItem(map, mapMgr.mapCreatePoint, mapMgr.mapParent, mapMgr.mapCreatePoint.position);
                break;
            case MapType.Rotate:
                Vector3 mapPos = CalRotatePoint(map);
                CreateMapItem(map, mapMgr.mapCreatePoint, mapMgr.mapParent, mapPos);
                break;
        }
    }

    /// <summary>
    /// 创建MapItem
    /// </summary>
    /// <param name="data"></param>
    /// <param name="mapCreatePoint"></param>
    /// <param name="parent"></param>
    /// <param name="mapPos"></param>
    private void CreateMapItem(JsonData data, Transform mapCreatePoint, Transform parent, Vector3 mapPos)
    {
        mapItem = new GameObject("MapItem").transform;
        JsonData tiles = data["Tiles"];
        mapItem.position = mapPos;
        mapItem.eulerAngles = CalMapItemAngle(tiles);
        mapItem.SetParent(parent);

        float enterTime = mapMgr.mapTime + mapMgr.tileWidth / 2;

        foreach (JsonData tile in tiles)
        {
            TileType tileType = (TileType)Enum.Parse(typeof(TileType), (string)tile["TileType"]);
            float time = (float)(double)tile["Time"];
            float distance = (time - mapMgr.mapTime) * mapMgr.speed;
            ChangeMapCreatePoint(tileType);
            CreateTile(distance, tileType);
            mapMgr.mapTime = time;
        }
        MapType mapType = (MapType)Enum.Parse(typeof(MapType), (string)data["MapType"]);
        if (mapType == MapType.Rotate)
        {
            string rotateAxis = (string)data["RotateAxis"];
            float rotateSpeed = (float)(double)data["RotateSpeed"];
            AddRotateAction(mapItem, rotateAxis, rotateSpeed);
            CalRotatedMapPoint(mapCreatePoint, mapItem, mapMgr.mapTime - enterTime, rotateAxis, rotateSpeed);
            SetStartRotate(mapItem, rotateAxis, rotateSpeed, enterTime);
        }
        else if (mapType == MapType.Move)
        {

        }
    }


    private Vector3 CalMapItemAngle(JsonData tiles)
    {
        TileType firstTileType = (TileType)Enum.Parse(typeof(TileType), (string)tiles[0]["TileType"]);
        Vector3 startAngle = mapMgr.mapCreatePoint.eulerAngles;
        bool startLeft = mapMgr.isLeft;
        ChangeMapCreatePoint(firstTileType);
        Vector3 resuleAngle = mapMgr.mapCreatePoint.eulerAngles;
        mapMgr.mapCreatePoint.eulerAngles = startAngle;
        mapMgr.isLeft = startLeft;
        return resuleAngle;
    }

    /// <summary>
    /// 添加旋转事件
    /// </summary>
    /// <param name="map"></param>
    /// <param name="rotateAxis"></param>
    /// <param name="rotateSpeed"></param>
    private void AddRotateAction(Transform map, string rotateAxis, float rotateSpeed)
    {
        if (rotateAxis == "Y")
            mapMgr.rotateUpdate += () => map.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        if (rotateAxis == "Z")
            mapMgr.rotateUpdate += () => map.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    /// <summary>
    /// 设置旋转地图开始位置，以保证到达该地图时正好对准
    /// </summary>
    /// <param name="mapItem"></param>
    /// <param name="rotateAxis"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="enterTime"></param>
    private void SetStartRotate(Transform mapItem, string rotateAxis, float rotateSpeed, float enterTime)
    {
        if (rotateAxis == "Y")
            mapItem.Rotate(new Vector3(0, -rotateSpeed * enterTime, 0));
        if (rotateAxis == "Z")
            mapItem.Rotate(new Vector3(0, 0, -rotateSpeed * enterTime));
    }

    /// <summary>
    /// 获取旋转后的MapCreatePoint，好接着该point继续生成地图
    /// </summary>
    private void CalRotatedMapPoint(Transform mapCreatePoint, Transform mapItem, float rotateTime, string rotateAxis, float rotateSpeed)
    {
        mapCreatePoint.SetParent(mapItem);
        Vector3 startAngle = mapItem.localEulerAngles;
        if (rotateAxis == "Y")
            mapItem.Rotate(new Vector3(0, rotateSpeed * (rotateTime + mapMgr.tileWidth / 2), 0));
        if (rotateAxis == "Z")
            mapItem.Rotate(new Vector3(0, 0, rotateSpeed * (rotateTime + mapMgr.tileWidth / 2)));
        mapCreatePoint.parent = null;
        mapItem.localEulerAngles = startAngle;
    }

    /// <summary>
    /// 计算可旋转地图的旋转点
    /// </summary>
    /// <param name="map"></param>
    /// <param name="mapCreatePoint"></param>
    /// <returns></returns>
    private Vector3 CalRotatePoint(JsonData map)
    {
        RotatePoint rotatePoint = (RotatePoint)Enum.Parse(typeof(RotatePoint), (string)map["RotatePoint"]);
        if (rotatePoint == RotatePoint.Start)
            return mapMgr.mapCreatePoint.position;

        //mapCreatePoint是引用类型，操作之后要改成初始状态
        Vector3 startPos = mapMgr.mapCreatePoint.position;
        Vector3 startDir = mapMgr.mapCreatePoint.forward;
        bool startIsLeft = mapMgr.isLeft;
        float startTime = mapMgr.mapTime;

        JsonData mapItems = map["Tiles"];
        foreach (JsonData mapItem in mapItems)
        {
            TileType tileType = (TileType)Enum.Parse(typeof(TileType), (string)mapItem["TileType"]);
            float time = (float)(double)mapItem["Time"];
            ChangeMapCreatePoint(tileType);
            Vector3 targetPos = mapMgr.mapCreatePoint.position + mapMgr.mapCreatePoint.forward * (time - mapMgr.mapTime) * mapMgr.speed;
            mapMgr.mapTime = time;

            mapMgr.mapCreatePoint.position = targetPos;

        }
        Vector3 endPos = mapMgr.mapCreatePoint.position;
        //mapCreatePoint是引用类型，操作之后要改成初始状态
        mapMgr.mapCreatePoint.position = startPos;
        mapMgr.mapCreatePoint.forward = startDir;
        mapMgr.isLeft = startIsLeft;
        mapMgr.mapTime = startTime;

        if (rotatePoint == RotatePoint.Middle)
        {
            return startPos + (endPos - startPos) / 2;
        }
        Debug.LogWarning("没有处理该rotatePoint的方法");
        return Vector3.zero;

    }



    /// <summary>
    /// 创建地图Tile
    /// </summary>
    private void CreateTile(float distance, TileType tileType)
    {
        //根据方向与距离创建地图块，设置位置与角度，父物体，添加碰撞器
        Vector3 targetPos = mapMgr.mapCreatePoint.position + mapMgr.mapCreatePoint.forward * distance;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = mapMgr.mapCreatePoint.position + (targetPos - mapMgr.mapCreatePoint.position) / 2 + 
            mapMgr.mapCreatePoint.forward * mapMgr.tileWidth / 2;
        cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.1f, distance);
        //当左右跳是生成的瓷砖会稍微长一些
        if(tileType == TileType.LeftJump || tileType  == TileType.RightJump)
        {
            cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.1f, distance + 0.4f);
            cube.transform.position = mapMgr.mapCreatePoint.position + (targetPos - mapMgr.mapCreatePoint.position) / 2 +
            mapMgr.mapCreatePoint.forward * (mapMgr.tileWidth / 2 - 0.2f);
        }
        if (mapType == MapType.Rotate)
        {
            cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.09f, distance);
        }
        //第一块瓷砖比较特殊
        if (mapMgr.firstTile)
        {
            cube.transform.position = mapMgr.mapCreatePoint.position + (targetPos - mapMgr.mapCreatePoint.position) / 2;
            cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.1f, distance + mapMgr.tileWidth);
            mapMgr.firstTile = false;
        }
        cube.transform.eulerAngles = mapMgr.mapCreatePoint.eulerAngles;
        cube.AddComponent<BoxCollider>();
        mapMgr.mapCreatePoint.position = targetPos;
        cube.transform.SetParent(mapItem);
    }

    private void ChangeMapCreatePoint(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.None:
                break;
            case TileType.Normal:
                if (mapMgr.isLeft)
                    mapMgr.mapCreatePoint.Rotate(new Vector3(0, 90, 0), Space.Self);
                else
                    mapMgr.mapCreatePoint.Rotate(new Vector3(0, -90, 0), Space.Self);
                mapMgr.isLeft = !mapMgr.isLeft;
                break;
            case TileType.LeftJump:
                mapMgr.mapCreatePoint.position = mapMgr.mapCreatePoint.position -
                    mapMgr.mapCreatePoint.right * (mapMgr.jumpWidth + mapMgr.tileWidth);
                break;
            case TileType.RightJump:
                mapMgr.mapCreatePoint.position = mapMgr.mapCreatePoint.position +
    mapMgr.mapCreatePoint.right * (mapMgr.jumpWidth + mapMgr.tileWidth);
                break;
        }
    }
}
