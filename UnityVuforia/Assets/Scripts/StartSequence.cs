using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Panel2;
    public Text ControllNumber;
    public Text ControllNumberLabel;
    public Button button;

    public void OpenPanel()
    {
        if (Panel2 != null)
        {
            string numberOfControll = ControllNumber.GetComponent<Text>().text.ToString();
            ControllNumberLabel.text = numberOfControll;
            Panel2.SetActive(true);
        }

    }
    public void ClosePanel()
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
            OpenPanel();
        }
    }

   
}
