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
    // Also restart after Game Over
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

    public void RestartNormal()
    {
        GameManager.playerDead = false;
        GameManager.Instance.player.GetComponent<Gravity>().gravity = true;
        GameManager.Instance.playerInput.SwitchCurrentActionMap("PlayerControls");
        GameManager.Instance.gameControls.SetActive(true);
        GameManager.Instance.deadObj.SetActive(false);

        BorderDetector.borders = new List<GameObject>();

        Destroy(GameManager.Instance.player);
        GameManager.Instance.player = Instantiate(GameManager.Instance.playerPref);
    }
}
