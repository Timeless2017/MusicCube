using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class MapItem
{

    private MapType mapType;
    private JsonData tiles;
    private MapManager mapMgr;
    private Transform mapItem;

    public void Init(MapManager mapManager)
    {
        this.mapMgr = mapManager;
    }

    /// <summary>
    /// 获取mapItem数据
    /// </summary>
    /// <param name="map"></param>
    private void GetMapItemAttribute(JsonData map)
    {
        mapType = (MapType)Enum.Parse(typeof(MapType), (string)map["MapType"]);
        tiles = map["Tiles"];
    }

    /// <summary>
    /// 创建MapItem
    /// </summary>
    /// <param name="map"></param>
    public void CreateMapItem(JsonData map)
    {
        GetMapItemAttribute(map);
        //创建MapItem物体，作为瓷砖父物体
        mapItem = new GameObject("MapItem").transform;
        mapItem.position = CalRotatePoint(map);
        mapItem.eulerAngles = mapMgr.mapCreatePoint.eulerAngles;
        mapItem.SetParent(mapMgr.mapParent);
        //进入该mapItem的时间
        float enterTime = mapMgr.mapTime + mapMgr.tileWidth / 2;
        //创建瓷砖
        foreach (JsonData tile in tiles)
        {
            CreateTile(tile);
        }
        //根据mapType做相应处理
        if(mapType == MapType.Rotate)
        {
            float stayTime = mapMgr.mapTime - enterTime - mapMgr.tileWidth / 2;
            string rotateAxis = (string)map["RotateAxis"];
            float rotateSpeed = (float)(double)map["RotateSpeed"];
            //根据旋转时间计算离开mapItem时mapCreatePoint的位置与方向
            CalRotatedMapPoint(mapItem, stayTime, rotateAxis, rotateSpeed);
            //添加旋转事件到update中
            AddRotateAction(mapItem, rotateAxis, rotateSpeed);
            //根据进入mapItem的时间往回转到开始时间的角度
            SetStartRotate(mapItem, rotateAxis, rotateSpeed, enterTime);
        }
    }

    /// <summary>
    /// 创造瓷砖
    /// </summary>
    /// <param name="data"></param>
    private void CreateTile(JsonData data)
    {
        //获取data内数据
        TileType tileType = (TileType)Enum.Parse(typeof(TileType), data["TileType"].ToString());
        float tileTime = (float)(double)data["Time"];
        //改变MapCreatePoint位置及方向，计算瓷砖长度
        ChangeMapCreatePoint(tileType);
        float cubeLength = CalCubeLength(tileType, tileTime);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 startPos = mapMgr.mapCreatePoint.position;
        Vector3 targetPos = startPos + mapMgr.mapCreatePoint.forward * cubeLength;

        //左右跳的瓷砖
        if(tileType == TileType.LeftJump || tileType == TileType.RightJump)
        {
            cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.1f, cubeLength + mapMgr.speed * PlayerModel.Instance.JumpDeviationTime);
            cube.transform.position = startPos + (targetPos - startPos) / 2 + mapMgr.mapCreatePoint.forward * mapMgr.tileWidth / 2
                - mapMgr.mapCreatePoint.forward * mapMgr.speed * PlayerModel.Instance.JumpDeviationTime / 2;
        }
        //正常的瓷砖
        else if (tileType == TileType.None || tileType == TileType.Normal)
        {
            cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.1f, cubeLength);
            cube.transform.position = startPos + (targetPos - startPos) / 2 + mapMgr.mapCreatePoint.forward * mapMgr.tileWidth / 2;
        }
        //第一块瓷砖
        if (mapMgr.mapTime == 0)
        {
            cube.transform.localScale = new Vector3(mapMgr.tileWidth, 0.1f, cubeLength + mapMgr.tileWidth);
            cube.transform.position = startPos + (targetPos - startPos) / 2;
        }

        cube.transform.eulerAngles = mapMgr.mapCreatePoint.eulerAngles;
        cube.AddComponent<BoxCollider>();
        cube.transform.SetParent(mapItem);
        mapMgr.mapCreatePoint.position = targetPos;
        mapMgr.mapTime = tileTime;
    }

    /// <summary>
    /// 计算瓷砖长度
    /// </summary>
    /// <param name="tileType"></param>
    /// <param name="tileTime"></param>
    /// <returns></returns>
    private float CalCubeLength(TileType tileType, float tileTime)
    {
        float time = 0;
        if (tileType == TileType.None || tileType == TileType.Normal)
            time = tileTime - mapMgr.mapTime;
        else if (tileType == TileType.LeftJump || tileType == TileType.RightJump)
        {
            time = tileTime - mapMgr.mapTime - PlayerModel.Instance.StandJumpTime;
        } 
        else
            Debug.LogError("还没有处理该" + tileType + "的方法");
        if (time <= 0)
        {
            Debug.LogError("cubeLength <= 0");
            Debug.Log(tileTime);
        }
            
        
        return time * PlayerModel.Instance.Speed;
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
    private void CalRotatedMapPoint(Transform mapItem, float rotateTime, string rotateAxis, float rotateSpeed)
    {
        mapMgr.mapCreatePoint.SetParent(mapItem);
        Vector3 startAngle = mapItem.localEulerAngles;
        if (rotateAxis == "Y")
            mapItem.Rotate(new Vector3(0, rotateSpeed * (rotateTime + mapMgr.tileWidth / 2), 0));
        if (rotateAxis == "Z")
            mapItem.Rotate(new Vector3(0, 0, rotateSpeed * (rotateTime + mapMgr.tileWidth / 2)));
        mapMgr.mapCreatePoint.parent = null;
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
        if(mapType == MapType.Normal)
            return mapMgr.mapCreatePoint.position;

        RotatePoint rotatePoint = (RotatePoint)Enum.Parse(typeof(RotatePoint), (string)map["RotatePoint"]);
        if (rotatePoint == RotatePoint.Start)
            return mapMgr.mapCreatePoint.position;

        //mapCreatePoint是引用类型，操作之后要改成初始状态
        Vector3 startPos = mapMgr.mapCreatePoint.position;
        Vector3 startAngle = mapMgr.mapCreatePoint.eulerAngles;
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
        mapMgr.mapCreatePoint.eulerAngles = startAngle;
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
    /// 改变MapCreatePoint
    /// </summary>
    /// <param name="tileType"></param>
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
                    mapMgr.mapCreatePoint.right * mapMgr.jumpWidth;
                break;
            case TileType.RightJump:
                mapMgr.mapCreatePoint.position = mapMgr.mapCreatePoint.position +
                    mapMgr.mapCreatePoint.right * mapMgr.jumpWidth;
                break;
        }
    }
}
