using UnityEngine;
using System.Collections;

public class MouseShow : MonoBehaviour {

    public static bool isOn = false;

    void Update()
    {
        if (isOn)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

}
