using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public void StartClicked()
    {
        SceneManager.LoadScene("ARControllView");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    public void ListClicked()
    {
        SceneManager.LoadScene("ListOfAllControlls");
        Screen.orientation = ScreenOrientation.Portrait;
    }
}