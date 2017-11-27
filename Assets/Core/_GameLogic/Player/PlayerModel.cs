using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class PlayerModel : Singleton<PlayerModel> {

    private float speed;
    public float Speed
    {
        get { return speed; }
    }

    private float standJumpTime;
    public float StandJumpTime
    {
        get { return standJumpTime; }
    }

    private float jumpDeviationTime;
    public float JumpDeviationTime
    {
        get { return jumpDeviationTime; }
    }

    public override void Initialize()
    {
        base.Initialize();
        ReadJsonAndInit();
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }

    private void ReadJsonAndInit()
    {
        string jsonText = ResourcesManager.Instance.LoadAssetByFullName<TextAsset>("Datas/PlayerAttribute.json").text;
        JsonData rootJson = JsonMapper.ToObject(jsonText);
        speed = (float)(double)rootJson["PlayerSpeed"];
        standJumpTime = (float)(double)rootJson["StandJumpTime"];
        jumpDeviationTime = (float)(double)rootJson["JumpDeviationTime"];
    }
}
