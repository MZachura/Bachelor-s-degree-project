using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AddToDatabase : MonoBehaviour
{
    private int counterGreen = 0;
    private int counterRed = 0;
    private string color;
    private int numberOfParts = 0;
    public Text label;
    public Button endButton;
    public GameObject gameObject;
    ColorBlock greenColor;
    ColorBlock redColor;
    private string dbName;
    private GameObject gO;
    private Button partButton;
    private int counterNoColor = 0;
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
    public void EndingSequence()
    {
        int id = Int32.Parse(label.GetComponent<Text>().text.ToString());
        
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform Children = gameObject.transform.GetChild(i);

            if (Children.name.Contains("Part"))
            {
                partButton = GameObject.Find($"Part{i + 1}").GetComponent<Button>();
                numberOfParts++;
                greenColor.normalColor = new Color32(50, 168, 84, 150);
                redColor.normalColor = new Color32(176, 11, 22, 150);
            
                if (partButton.colors.normalColor == greenColor.normalColor)
                {
                    counterGreen++;
                }
                else if (partButton.colors.normalColor == redColor.normalColor)
                {
                    counterRed++;

                }
                else
                {
                    counterNoColor++;
                }


            }
            if (counterGreen + counterRed == numberOfParts)
            {
                InsertGoodOrBad(counterGreen, counterRed, id,counterNoColor);
            }
        }
    }

    public void InsertGoodOrBad(int green, int red, int id, int noColor)
    {
        if (red == 0)
        {
            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE QCstatus SET status = 1 WHERE controll_ID = {id};";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        } 
        else if (noColor == 0)
        {
            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE QCstatus SET status = 0 WHERE controll_ID = {id};";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        else
            {
            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE QCstatus SET status = NULL WHERE controll_ID = {id};";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        
    }


}
