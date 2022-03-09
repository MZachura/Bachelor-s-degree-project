using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;

public class ApproveDissaproveButtonBehaviour : MonoBehaviour
{
    public Button approveButton;
    public Button disapproveButton;
    public Text label;
    private Button PartButton;
    ColorBlock colors;
    public Text noteObject;
    public Text idLabel;
    private string dbName;
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
    public void ApproveClicked()
    {
        string note;
        string colorToPass = "green";
        PartButton = GameObject.Find(label.text).GetComponent<Button>();
        string nodeNumber = label.text.ToString();
        colors = PartButton.colors;
        colors.normalColor = new Color32(50, 168, 84, 150);
        PartButton.colors = colors;
        note = "";

        saveToDatabase(colorToPass, nodeNumber, note);
    }

    public void DisapproveClicked()
    {
        string note;
        string colorToPass = "red";
        note = noteObject.GetComponent<Text>().text.ToString();
        PartButton = GameObject.Find(label.text).GetComponent<Button>();
        string nodeNumber = label.text.ToString();
        colors = PartButton.colors;
        colors.normalColor = new Color32(176, 11, 22, 150);
        PartButton.colors = colors;
        saveToDatabase(colorToPass, nodeNumber, note);
    }

    public void saveToDatabase(string colorPassed, string node, string note)
    {
         int id =  Int32.Parse(idLabel.GetComponent<Text>().text.ToString());
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM QCNodes WHERE controll_ID = {id} AND node = '{node}'; ";
                using (IDataReader reader = command.ExecuteReader())
                {
                    int colIndex = reader.GetOrdinal("node");
                    if (reader.IsDBNull(colIndex))
                    {
                        reader.Close();
                        command.CommandText = $"INSERT INTO QCNodes (controll_ID, node , color, note) VALUES ({id}, '{node}', '{colorPassed}','{note}');";
                        command.ExecuteNonQuery();
                        Debug.Log("Notatka: "+note);
                    }
                    else
                    {
                        reader.Close();
                        command.CommandText = $"UPDATE QCNodes SET controll_ID = {id}, node = '{node}', color='{colorPassed}', note= '{note}' WHERE node = '{node}' AND controll_ID = {id};";
                        command.ExecuteNonQuery();
                        Debug.Log("Notatka: "+note);
                    }
                }


            }
            connection.Close();
        }
    }


}
