using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{   
    public GameObject score_text;
    private Text UIText;
    private double score = 0;
    private static GameManager manager_instance;
    public static GameManager Instance{
        get{
            if(manager_instance == null)
                Debug.LogError("you fucked");
            
            return manager_instance;
        }
    }

    private void Awake(){
        manager_instance = this;
        UIText = score_text.GetComponent<Text>();
    }

    public void setScore(double game_score){
        this.score = game_score;
        UIText.text = "Score: " + this.score;
    }
    public double getScore(){
        return this.score;
    }
}
