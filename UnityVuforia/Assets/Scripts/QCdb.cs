using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using System.IO;

public class QCdb : MonoBehaviour
{
    public Text ControllNumberLabel;
    private string dbName;
    private Button PartButton;
    private ColorBlock colors;
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
        CreateDB();
    }

    public void CreateDB()
    {
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS QCstatus (controll_ID INT PRIMARY KEY, status BOOLEAN);";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS QCNodes (controll_ID INT PRIMATY KEY, node NVARCHAR(7), color NVARCHAR(6), note NVARCHAR(1000));";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    public void InsertOrRead()
    {
        string label = ControllNumberLabel.GetComponent<Text>().text.ToString();
        int label1 = Int32.Parse(label);
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM QCstatus WHERE controll_ID LIKE {label1};";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.IsDBNull(0))
                    {
                        doInsert(label1);
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            int id = Int32.Parse(reader["controll_ID"].ToString());
                        }
                        reader.Close();
                        nodeRead(label1);
                    }
                }
            }
            connection.Close();
        }
    }
    public void doInsert(int id)
    {
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO QCstatus (controll_ID) VALUES ({id}) ;";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        InsertOrRead();
    }

    public void nodeRead(int id)
    {
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM QCNodes WHERE controll_ID = {id}; ";
                using (IDataReader reader = command.ExecuteReader())
                {
                    int colIndex = reader.GetOrdinal("color");
                    if (reader.IsDBNull(colIndex))
                    {
                        Debug.Log("No colors found");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            PartButton = GameObject.Find(reader["node"].ToString()).GetComponent<Button>();
                            if (reader["color"].ToString() == "green")
                            {
                                colors = PartButton.colors;
                                colors.normalColor = new Color32(50, 168, 84, 150);
                                PartButton.colors = colors;
                            }
                            else if(reader["color"].ToString() == "red")
                            {
                                colors = PartButton.colors;
                                colors.normalColor = new Color32(176, 11, 22, 150);
                                PartButton.colors = colors;
                            }
                            Debug.Log("Wczytaj kolory i dodaj do buttonów");
                        }
                        reader.Close();
                    }
                }
                    command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }
}
