using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class PanelOpener : MonoBehaviour
{
    public GameObject Panel;
    public Text label;
    public Button button;
    public InputField note;

    public void OpenPanel()
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
        }

        if(Panel.activeSelf)
        {
            label.text = button.name;
        }
    }
    public void ClosePanel()
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);  
        }
    }
    public void ClearFileds()
    {
        note.text = "";
    }
}