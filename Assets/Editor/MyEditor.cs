using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEditor;



public class MyEditor {

    [MenuItem("Tool/ArrangeGo")]
    static void CreateMap()
    {
        GameObject[] gos = Selection.gameObjects;
        for(int i = 0; i < gos.Length; i++)
        {
            gos[i].transform.position = new Vector3(i * 0.4f, 0, 0);
        }
    }



}


