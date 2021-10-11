using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenMenu : MonoBehaviour
{
    public GameObject hiddenMenu;
    public GameObject setting;
    public GameObject leftButton;
    public GameObject rightButton;

    private string lastPressed = "";
    private int counter = 0;

    public void LeftPressed()
    {
        Check("left");
    }

    public void RightPressed()
    {
        Check("right");
    }

    public void Check(string current)
    {
        if (lastPressed == "")
        {
            counter += 1;
        }
        else if (lastPressed == "left" && current == "right" || lastPressed == "right" && current == "left")
        {
            counter += 1;
        }
        else
        {
            counter = 0;
        }

        lastPressed = current;

        if (counter >= 6)
        {
            // Sound effect
            counter = 0;
            lastPressed = "";
            hiddenMenu.SetActive(true);
            setting.SetActive(false);
        }
    }
}
