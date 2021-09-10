using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    public void change_scene(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //single or Additive(load this scene without destroying the previous one)
        SceneManager.LoadSceneAsync("GravityTest", LoadSceneMode.Single);
    }
}
