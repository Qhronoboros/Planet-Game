using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonTesting : MonoBehaviour
{
    public GameObject redSquare;
    public GameObject blueSquare;

    public void OnButtonRed(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            redSquare.SetActive(true);
        }
        else if (value.canceled)
        {
            redSquare.SetActive(false);
        }
    }

    public void OnButtonBlue(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            blueSquare.SetActive(true);
        }
        else if (value.canceled)
        {
            blueSquare.SetActive(false);
        }
    }
}
