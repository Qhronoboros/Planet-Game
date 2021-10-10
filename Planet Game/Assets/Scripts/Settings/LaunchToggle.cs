using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchToggle : MonoBehaviour
{
    public static bool invertedLaunch;
    public Toggle toggleUI;

    void Start()
    {
        if (!PlayerPrefs.HasKey("inverted_launch"))
        {
            PlayerPrefs.SetInt("inverted_launch", 0);
        }

        invertedLaunch = PlayerPrefs.GetInt("inverted_launch") == 1 ? true : false;

        toggleUI.isOn = invertedLaunch;
    }

    public void toggleInvertedLaunch(bool value)
    {
        PlayerPrefs.SetInt("inverted_launch", System.Convert.ToInt32(value));

        invertedLaunch = value;
    }
}
