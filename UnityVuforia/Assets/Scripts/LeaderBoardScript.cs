using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class LeaderBoardScript : MonoBehaviour
{
    private string dbName;
    private int all;
    private int negative;
    private float percentage;
    public Text positivePercentage;
    public GameObject prefabButton;
    public RectTransform ParentPanel;
    public GameObject Canvas;
    public GameObject ScrollPanel;
    private Text label;
    private ColorBlock colors;
    public GameObject Panel;
    public SpecyficControllScript Panelscript;
    private string conn;
    void Awake()
    {
        dbName = "QC.s3db";
        string filepath = Application.persistentDataPath + "/" + dbName;
        if (!File.Exists(filepath))
        {

            // UNITY_ANDROID
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/QC.s3db");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        conn = "URI=file:" + filepath;
    }
    void Start()
    {
        readLeaderboard();
        countPercentage();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void readLeaderboard() {
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM QCstatus;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int colIndex = reader.GetOrdinal("status");
                        if (reader.IsDBNull(colIndex))
                        {
                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(ParentPanel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            label = tempButton.transform.Find("Text").GetComponent<Text>();
                            label.text = reader["controll_ID"].ToString();
                            tempButton.name = reader["controll_ID"].ToString();
                            tempButton.onClick.AddListener(() => ButtonClicked(tempButton.name));
                        }
                        else if (bool.Parse(reader["status"].ToString()))
                        {
                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(ParentPanel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            tempButton.name = reader["controll_ID"].ToString();
                            label = tempButton.transform.Find("Text").GetComponent<Text>();
                            label.text = reader["controll_ID"].ToString();
                            colors = tempButton.colors;
                            colors.normalColor = new Color32(50, 168, 84, 150);
                            
                            tempButton.colors = colors;
                            tempButton.onClick.AddListener(() => ButtonClicked(tempButton.name));
                        }
                        else if (!bool.Parse(reader["status"].ToString()))
                        {
                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(ParentPanel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            tempButton.name = reader["controll_ID"].ToString();
                            label = tempButton.transform.Find("Text").GetComponent<Text>();
                            label.text = reader["controll_ID"].ToString();
                            colors = tempButton.colors;
                            colors.normalColor = new Color32(176, 11, 22, 150);
                            tempButton.colors = colors;
                            tempButton.onClick.AddListener(() => ButtonClicked(tempButton.name));
                        }
                        
                    }
                    reader.Close();
                    
                }
            }
            connection.Close();
        }
    }
   
    void ButtonClicked(string id)
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
            if (Panel.activeSelf == true)
            {
                
                Panelscript = Panel.GetComponent<SpecyficControllScript>();
                Panelscript.SetLabelAndButtons(id);
            }
            Canvas.SetActive(false);
        }
     
    }
        public void countPercentage()
    {
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT COUNT(controll_ID) FROM QCstatus;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        all = Int32.Parse(reader["COUNT(controll_ID)"].ToString());
                    }
                    reader.Close();
                }
                command.CommandText = $"SELECT COUNT(controll_ID) FROM QCstatus WHERE status = 0 OR status IS NULL;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        negative = Int32.Parse(reader["COUNT(controll_ID)"].ToString());
                        
                    }
                    reader.Close();
                }

            }
            connection.Close();
        }
        percentage = ((float)negative/(float)all) * 100;
        percentage = 100 - percentage;
        positivePercentage.text = percentage.ToString();
    }
   
}
