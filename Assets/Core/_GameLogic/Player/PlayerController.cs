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
    public float speed = 1f;

    private GameObject _root;

    private PlayerState state;

    private Ray ray;
    private RaycastHit hit;
    //射线检测不到地面的时间
    private float rayNoGround;
    private float noGorundToDropTime = 0.3f;
    
    private Rigidbody m_rigidbody;

	public void Init (GameObject root) {
        _root = root;
        state = PlayerState.Run;
    }

    private float time;


	void Update () {

        switch (state)
        {
            case PlayerState.Run:
                _root.transform.Translate(Vector3.forward * Time.deltaTime * speed);
                break;
            case PlayerState.Jump:
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

    private Vector3 jumpTargetPos;

    /// <summary>
    /// 开始跳
    /// </summary>
    private void StartJump(bool isLeftJump)
    {
        //只有角色在行走的时候才可以开始跳，在跳跃过程中或飞行中则不行（可能会改）
        if (state != PlayerState.Run)
            return;
        state = PlayerState.Jump;
        if (isLeftJump)
        {
            //jumpTargetPos = transform.position + -transform.right * 
        }
        else
        {

        }

    }

    private void JumpUpdate()
    {

    }

#endregion
}
