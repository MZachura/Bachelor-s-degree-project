using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPosition : MonoBehaviour
{
    public Button newButton;

    void Update()
    {
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        newButton.transform.position = newPosition;
    }
}
