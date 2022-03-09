using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;

public class SpecyficControllScript : MonoBehaviour
{
    private string dbName;
    public GameObject ParentPanel;
    public GameObject CurrentPanel;
    public GameObject NotePanel;
    public GameObject prefabButton;
    public RectTransform Panel;
    public GameObject ScrollPanel;
    public Text label;
    public Text label1;
    private ColorBlock colors;
    public LeaderBoardScript Panelscript;
    private string conn;
    // Start is called before the first frame update
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
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLabelAndButtons(string id)
    {
        
        if (CurrentPanel.activeSelf)
        {
            ActivateNodeButtons(id);
            
            
        }
    }
    public void Backout()
    {
        if (NotePanel.activeSelf)
        {
            NotePanel.SetActive(false);
            CurrentPanel.SetActive(true);
        }
    }
    public void BackToLeaderBoard()
    {
        clearAll();
        if (CurrentPanel.activeSelf)
        { 
            CurrentPanel.SetActive(false);
            ParentPanel.SetActive(true);
            
        }
    }
    public void GetNote(string node)
    {
        if (!NotePanel.activeSelf)
        {
            NotePanel.SetActive(true);
            CurrentPanel.SetActive(false);
            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM QCNodes WHERE node = '{node}';";
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            label1.text = reader["note"].ToString();
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

    }
    public void ActivateNodeButtons(string id)
    {
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM QCNodes WHERE controll_ID = {id};";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int colIndex = reader.GetOrdinal("color");
                        if (reader.IsDBNull(colIndex))
                        {
                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(Panel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            label = tempButton.transform.Find("Text").GetComponent<Text>();
                            label.text = reader["node"].ToString();
                            tempButton.name = reader["node"].ToString();
                            colors = tempButton.colors;
                            colors.disabledColor = new Color32(255, 255, 255, 255);
                            tempButton.colors = colors;
                            
                            
                        } else if (reader["color"].ToString() == "green")
                        {
                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(Panel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            label = tempButton.transform.Find("Text").GetComponent<Text>();
                            label.text = reader["node"].ToString();
                            tempButton.name = reader["node"].ToString();
                            colors = tempButton.colors;
                            colors.disabledColor = new Color32(50, 168, 84, 150);
                            
                            tempButton.colors = colors;
                            tempButton.interactable = false;

                        }
                        else if (reader["color"].ToString() == "red")
                        {
                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(Panel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            label = tempButton.transform.Find("Text").GetComponent<Text>();
                            label.text = reader["node"].ToString();
                            tempButton.name = reader["node"].ToString();
                            colors = tempButton.colors;
                            colors.normalColor = new Color32(176, 11, 22, 150);
                            tempButton.colors = colors;
                            tempButton.onClick.AddListener(() => GetNote(tempButton.name));
                        }

                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }
    public void clearAll()
    {

        foreach (Transform child in ScrollPanel.transform)
        {
            GameObject.Destroy(child.gameObject);   
        }
       
    }
}
