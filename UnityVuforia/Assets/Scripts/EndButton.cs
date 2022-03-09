using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class EndButton : MonoBehaviour
{

    public void EndClicked()
    {
        SceneManager.LoadScene("MainMenu");
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
