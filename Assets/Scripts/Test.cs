using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour {


	void Start () {
		
	}


    void Update()
    {
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0), Space.Self);
    }
}
