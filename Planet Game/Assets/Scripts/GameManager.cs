using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{   //score
    public GameObject score_text;
    private Text UIText;
    private float score = 0;
    //life
    private int life = 3;
    private int max_life = 3;
    public Image[] hearts;
    public Sprite full_heart;
    public Sprite empty_heart;
    //game over
    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject tempGameOver;
    public static bool playerDead = false;

    private static GameManager manager_instance;
    public static GameManager Instance{
        get{
            if(manager_instance == null)
                Debug.LogError("you fucked");
            
            return manager_instance;
        }
    }

    private void Awake(){
        playerDead = false;
        manager_instance = this;
        UIText = score_text.GetComponent<Text>();
    }

    //score
    public void setScore(float game_score){
        this.score = game_score;
        UIText.text = this.score.ToString();
    }
    public float getScore(){
        return this.score;
    }

    // life
    public void set_life(int game_life){
        this.life = game_life;

        if(this.life > max_life){
            this.life = max_life;
        }
        if(this.life >= 0){
            for(int i = 0; i < hearts.Length; i++){
                if(i<this.life){
                    hearts[i].sprite = full_heart;
                }else{
                    hearts[i].sprite = empty_heart;
                }
            }
        }

        if(this.life == 0){
            game_over();
        }
    }
    public int get_life(){
        return this.life;
    }
    //
    public void game_over(){
        //freeze_game();
        playerDead = true;
        Debug.Log("Death");
        playerInput.SwitchCurrentActionMap("EmptyMap");
        gameControls.SetActive(false);
        tempGameOver.SetActive(true);
    }
    //public void freeze_game(){
    //    Time.timeScale = 0f;
    //}
}
