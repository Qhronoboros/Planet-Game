using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    public AudioSource canvasSource;

    public Toggle godModeToggle;
    public static bool godMode;

    public Toggle unlockStagesToggle;
    public static bool unlockStages;

    public Toggle infCoinsToggle;
    public static bool infCoins;

    public Toggle infBreadToggle;
    public static bool infBread;

    public Toggle infLaunchesToggle;
    public static bool infLaunches;

    public Toggle infLivesToggle;
    public static bool infLives;

    public Toggle instaKillToggle;
    public static bool instaKill;

    private void Start()
    {
        godModeToggle.isOn = godMode = FindCheat("godmode");

        unlockStagesToggle.isOn = unlockStages = FindCheat("unlockstages");

        infCoinsToggle.isOn = infCoins = FindCheat("infcoins");

        infBreadToggle.isOn = infBread = FindCheat("infbread");

        infLaunchesToggle.isOn = infLaunches = FindCheat("inflaunches");

        infLivesToggle.isOn = infLives = FindCheat("inflives");

        instaKillToggle.isOn = instaKill = FindCheat("instakill");
    }
    
    public static void CheatStart()
    {
        godMode = FindCheat("godmode");

        unlockStages = FindCheat("unlockstages");

        infCoins = FindCheat("infcoins");

        infBread = FindCheat("infbread");

        infLaunches = FindCheat("inflaunches");

        infLives = FindCheat("inflives");

        instaKill = FindCheat("instakill");
    }

    public static bool FindCheat(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 0);
        }

        return PlayerPrefs.GetInt(key) == 1 ? true : false;
    }

    public void UpdateCheat(bool value, string key)
    {
        canvasSource.Play();
        PlayerPrefs.SetInt(key, System.Convert.ToInt32(value));
    }

    public void ToggleGodMode(bool value)
    {
        UpdateCheat(value, "godmode");
        godMode = value;
    }

    public void ToggleUnlockStages(bool value)
    {
        UpdateCheat(value, "unlockstages");
        unlockStages = value;
    }

    public void ToggleInfCoins(bool value)
    {
        UpdateCheat(value, "infcoins");
        infCoins = value;
    }

    public void ToggleInfBread(bool value)
    {
        UpdateCheat(value, "infbread");
        infBread = value;
    }

    public void ToggleInfLaunches(bool value)
    {
        UpdateCheat(value, "inflaunches");
        infLaunches = value;
    }

    public void ToggleInfLives(bool value)
    {
        UpdateCheat(value, "inflives");
        infLives = value;
    }

    public void ToggleInstaKill(bool value)
    {
        UpdateCheat(value, "instakill");
        instaKill = value;
    }
}
