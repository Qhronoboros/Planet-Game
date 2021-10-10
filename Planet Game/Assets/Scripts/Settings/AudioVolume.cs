using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public static float masterVolume;
    public static float musicVolume;
    public static float ambianceVolume;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider ambianceSlider;

    public void SetVolumeMaster(float sliderValue)
    {
        masterVolume = sliderValue;
        Debug.Log(sliderValue);
        mixer.SetFloat("VolumeMaster", Mathf.Log10(sliderValue) * 20);
    }

    public void SetVolumeMusic(float sliderValue)
    {
        musicVolume = sliderValue;
        Debug.Log(sliderValue);
        mixer.SetFloat("VolumeMusic", Mathf.Log10(sliderValue) * 20);
    }

    public void SetVolumeAmbiance(float sliderValue)
    {
        ambianceVolume = sliderValue;
        Debug.Log(sliderValue);
        mixer.SetFloat("VolumeAmbiance", Mathf.Log10(sliderValue) * 20);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("master_volume"))
        {
            PlayerPrefs.SetFloat("master_volume", 0.8f);
        }

        if (!PlayerPrefs.HasKey("music_volume"))
        {
            PlayerPrefs.SetFloat("music_volume", 1.0f);
        }

        if (!PlayerPrefs.HasKey("ambiance_volume"))
        {
            PlayerPrefs.SetFloat("ambiance_volume", 1.0f);
        }

        masterVolume = PlayerPrefs.GetFloat("master_volume");
        musicVolume = PlayerPrefs.GetFloat("music_volume");
        ambianceVolume = PlayerPrefs.GetFloat("ambiance_volume");

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        ambianceSlider.value = ambianceVolume;

        SetVolumeMaster(masterVolume);
        SetVolumeMusic(musicVolume);
        SetVolumeAmbiance(ambianceVolume);

        Debug.Log(masterVolume.ToString() + " " + musicVolume.ToString() + " " + ambianceVolume);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("master_volume", masterVolume);
        PlayerPrefs.SetFloat("music_volume", musicVolume);
        PlayerPrefs.SetFloat("ambiance_volume", ambianceVolume);
    }
}
