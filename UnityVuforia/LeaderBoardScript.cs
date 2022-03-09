using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

private string dbName;

public class LeaderBoardScript : MonoBehaviour
{
    void Awake()
    {
        dbName = "URI = file:" + Application.dataPath + "/Database/QC.s3db";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
