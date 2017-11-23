using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Run,
    Jump,
    Fly
}

public class PlayerController {

    private bool isLeft = false;
    private float speed = PlayerModel.Instance.Speed;
    private float standJumpTime = PlayerModel.Instance.StandJumpTime;

    private GameObject _root;

    private PlayerState state;

    private Ray ray;
    private RaycastHit hit;
    //射线检测不到地面的时间
    private float rayNoGround;
    private float noGorundToDropTime;
    
    private Rigidbody m_rigidbody;

	public void Init (GameObject root) {
        _root = root;
        state = PlayerState.Run;
    }

    private float time;

    public void CreatePlayer(string playerModelPath)
    {
        GameObject player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        GameObject playerPrefab = ResourcesManager.Instance.LoadPrefab(playerModelPath);
        GameObject playerModel = GameObject.Instantiate(playerPrefab);
        playerModel.transform.SetParent(player.transform);
        playerModel.transform.localPosition = Vector3.zero;
        playerModel.transform.localEulerAngles = new Vector3(0, 180, 0);
        Init(player);
    }

	public void Update () {

        switch (state)
        {
            case PlayerState.Run:
                noGorundToDropTime = 0.3f;
                _root.transform.Translate(Vector3.forward * Time.deltaTime * speed);
                break;
            case PlayerState.Jump:
                noGorundToDropTime = standJumpTime;
                JumpUpdate();
                break;
            case PlayerState.Fly:
                break;
        }
        
        ray = new Ray(_root.transform.position, -_root.transform.up);
        bool onTheGround = Physics.Raycast(ray, out hit, 10f);
        //因为有时候地面之间会有很小的缝隙，不想检测不到地面立刻添加重力，设定成一段时间没检测到地面则添加重力
        if (onTheGround)
            rayNoGround = 0;
        else
            rayNoGround += Time.deltaTime;
        if (m_rigidbody == null && rayNoGround >= noGorundToDropTime)
        {
            m_rigidbody = _root.AddComponent<Rigidbody>();
        }


        if(onTheGround && _root.transform.parent != hit.collider.transform.parent)
        {
            _root.transform.SetParent(hit.collider.transform.parent);
            _root.transform.eulerAngles = hit.collider.transform.parent.eulerAngles;
            _root.transform.localPosition = new Vector3(_root.transform.localPosition.x, 0.18f, _root.transform.localPosition.z);
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            Swerve();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartJump(true);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartJump(false);
        }


        if (Input.GetMouseButtonDown(0))
        {
            time = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(time);
            time = 0;
        }

        if (Input.GetMouseButton(0))
        {
            time += Time.deltaTime;
        }
	}

    /// <summary>
    /// 转向
    /// </summary>
    private void Swerve()
    {
        state = PlayerState.Run;
        if (isLeft)
            _root.transform.Rotate(new Vector3(0, 90, 0));
        else
            _root.transform.Rotate(new Vector3(0, -90, 0));
        isLeft = !isLeft;
    }


    #region 角色跳跃

    private bool isLeftJump;
    private float lastJumpTime;
    private Vector3 startJumpPos;

    /// <summary>
    /// 开始跳
    /// </summary>
    private void StartJump(bool isLeftJump)
    {
        //只有角色在行走的时候才可以开始跳，在跳跃过程中或飞行中则不行（可能会改）
        if (state != PlayerState.Run)
            return;
        this.isLeftJump = isLeftJump;
        state = PlayerState.Jump;
        startJumpPos = _root.transform.position;
        lastJumpTime = Time.time;
    }

    private void JumpUpdate()
    {
        if (state != PlayerState.Jump)
            return;
        float jumpedTime = Time.time - lastJumpTime;

        //高度
        float height;
        if (jumpedTime <= standJumpTime / 2)
            height = 9.8f * jumpedTime * jumpedTime / 2;
        else
            height = 9.8f * (standJumpTime - jumpedTime) * (standJumpTime - jumpedTime) / 2;
        if (isLeftJump)
        {
            _root.transform.position = startJumpPos + _root.transform.up * height - _root.transform.right * speed * jumpedTime;
        }
        else
        {
            _root.transform.position = startJumpPos + _root.transform.up * height + _root.transform.right * speed * jumpedTime;
        }
        //跳跃结束
        if (jumpedTime >= standJumpTime)
        {
            _root.transform.position = new Vector3(_root.transform.position.x, startJumpPos.y, _root.transform.position.z);
            state = PlayerState.Run;
        }
    }

#endregion
}
