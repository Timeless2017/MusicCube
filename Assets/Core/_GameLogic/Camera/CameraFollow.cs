using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //public float speed = 3.0f;
    //private float cameraSpeed;
    private GameObject player;

    void Start()
    {
        //cameraSpeed = Mathf.Sqrt(speed * speed / 2) / Mathf.Sqrt(2);
        player = GameObject.Find("Player");

    }


    void Update()
    {
        
        transform.position = player.transform.position + new Vector3(4.5f, 3f, -4.5f);
        //transform.Translate(new Vector3(-1, 0, 1) * Time.deltaTime * cameraSpeed, Space.World);
    }
}
