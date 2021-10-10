using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class main_menu : MonoBehaviour
{   
    public Button next_button;
    public Button prev_button;
    public Text stagetext;
    public static string sceneName = "";
    public static int selected_stage_index;

    public AudioSource unlockTrack;
    public AudioSource lockTrack;

    // Start is called before the first frame update
    void Start()
    {
        //SaveGameManager.Instance.unlock_level(3);
        //SaveGameManager.Instance.unlock_level(2);
        //SaveGameManager.Instance.unlock_level(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(){
        if(SaveGameManager.Instance.check_level_unlocked(selected_stage_index)){
            unlockTrack.Play();
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
        else
        {
            lockTrack.Play();
        }
        
    }
}
