using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No GameManager instance");

            return _instance;
        }
    }

    // Important GameObjects
    public GameObject player;
    public GameObject playerPlanet;
    public List<GameObject> planets;
    public CameraController cameraController;
    public GameObject warning;

    //score
    public GameObject score_text;
    private Text UIText;
    private float score = 0;
    //coin
    public GameObject coin_text;
    private Text UI_coin_text;
    private float coin = 0;
    //special
    public GameObject special_text;
    public GameObject special_child;
    private Text UI_special_text;
    private float special = 0;
    public float max_special = 5;
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
    public GameObject temp_stage_clear;
    public static bool playerDead = false;
    // Stage Clear
    public bool stageClear = false;
    public string nextStage = "stage2 Testing";
    // Vignette
    public float maxIntensity = 0.45f;


    private void Awake(){
        _instance = this;
        playerDead = false;
        UIText = score_text.GetComponent<Text>();
        UI_coin_text = coin_text.GetComponent<Text>();
        UI_special_text = special_text.GetComponent<Text>();
        get_max_special();
        set_special(special);
    }

    // Set the planet the player is orbiting
    public void setPlayerPlanet(GameObject planet)
    {
        playerPlanet = planet;
    }

    // Get the planet the player is orbiting
    public GameObject getPlayerPlanet()
    {
        if (!playerPlanet)
        {
            playerPlanet = player.GetComponent<PlayerController>().mainPlanetObj;
        }
        return playerPlanet;
    }

    //score
    public void setScore(float game_score){
        this.score = game_score;
        UIText.text = this.score.ToString();
    }
    public float getScore(){
        return this.score;
    }
    public void set_coin(float coin_obj){
        this.coin = coin_obj;
        UI_coin_text.text = this.coin.ToString();
    }
    public float get_coin(){
        return this.coin;
    }
    public void set_special(float special_obj){
        this.special = special_obj;
        UI_special_text.text = this.special.ToString() + "/" + this.max_special.ToString();
        if(this.special == this.max_special){
            stage_clear();
        }
    }
    public float get_special(){
        return this.special;
    }
    public float get_max_special(){
        // max_special = special_child.transform.childCount;
        return max_special;
    }
    //stage clear
    public void stage_clear(){
        stageClear = true;
        playerInput.SwitchCurrentActionMap("EmptyMap");
        gameControls.SetActive(false);
        temp_stage_clear.SetActive(true);
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
        if (!stageClear)
        {
            playerDead = true;
            Debug.Log("Death");
            player.GetComponent<Gravity>().gravity = false;
            playerInput.SwitchCurrentActionMap("EmptyMap");
            gameControls.SetActive(false);
            tempGameOver.SetActive(true);
        }
    }
    //public void freeze_game(){
    //    Time.timeScale = 0f;
    //}
}
