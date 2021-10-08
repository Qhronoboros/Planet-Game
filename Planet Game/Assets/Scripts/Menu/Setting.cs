using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{   
    public Toggle muteToggle;

    public void reset_data(){
        PlayerPrefs.DeleteAll();
        print("data cleared");
    }
    public void mute(){
        if(muteToggle.isOn){
            PlayerPrefs.SetInt("Sound", 1);
            GameManager.Instance.sound_setting();
        }
        if(!muteToggle.isOn){
            PlayerPrefs.SetInt("Sound", 0);
            GameManager.Instance.sound_setting();
        }
        
        print(check_muted());
    }
    public int check_muted(){
        if(!PlayerPrefs.HasKey("Sound")){
            PlayerPrefs.SetInt("Sound", 1);
        }
        return PlayerPrefs.GetInt("Sound");
    }
    void Start(){
        if(check_muted() == 1){
            muteToggle.isOn = true;
        }else{
            muteToggle.isOn = false;
        }
        
    }


}
