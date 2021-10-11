using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchToggle : MonoBehaviour
{
    public static bool invertedLaunch;
    public Toggle toggleUI;
    public AudioSource canvasSource;

    void Start()
    {
        if (!PlayerPrefs.HasKey("inverted_launch"))
        {
            PlayerPrefs.SetInt("inverted_launch", 0);
        }

        toggleUI.isOn = invertedLaunch = PlayerPrefs.GetInt("inverted_launch") == 1 ? true : false;
    }

    public void toggleInvertedLaunch(bool value)
    {
        canvasSource.Play();

        PlayerPrefs.SetInt("inverted_launch", System.Convert.ToInt32(value));

        invertedLaunch = value;
    }
}
