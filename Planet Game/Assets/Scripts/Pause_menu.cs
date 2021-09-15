using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{
    public void Resume(){
        Time.timeScale = 1f;
    }
    public void Pause(){
        //pause_ui.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Reload(){
        Resume();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

    }
    public void Quit(){
        Application.Quit(); 
    }
    public void GoToNextStage()
    {
        SceneManager.LoadSceneAsync(GameManager.Instance.nextStage, LoadSceneMode.Single);
    }
}
