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
    public GameObject playerPref;
    public GameObject playerPlanet;
    public List<GameObject> planets;
    public CameraController cameraController;
    public GameObject stage;
    public GameObject asteroidParent;

    // Materials
    public Material defaultMat;
    public Material damagedMat;
    public Material invincibleMat;
    public Material vignetteMat;

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
    // Lifes
    public GameObject deadObj;
    public Text lifeText;
    public int lifes = 5;
    // Health Points
    private int health = 3;
    private int maxHealth = 3;
    public Image[] hearts;
    public Sprite full_heart;
    public Sprite empty_heart;
    //game over
    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject tempGameOver;
    public GameObject temp_stage_clear;
    public static bool playerDead = false;
    public static PlayerDeaths playerDeaths = PlayerDeaths.Alive;
    // Stage Clear
    public bool stageClear = false;
    public string nextStage = "stage2 Testing";
    // Vignette
    public float maxIntensity = 0.45f;
    // Player start position
    public Vector3 startPos = new Vector3(0, 16, 0);
    // Visible collectables on screen
    public List<GameObject> collectablesOnScreen = new List<GameObject>();

    public enum PlayerDeaths
    {
        Alive,
        Projectile,
        Border,
        Ring
    }


    private void Awake(){
        _instance = this;
        playerDead = false;
        playerDeaths = PlayerDeaths.Alive;
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
        UI_special_text.text = this.special.ToString() + "/ " + this.max_special.ToString();
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
        planets[0].GetComponent<Animator>().SetBool("IsRepaired", true);
        playerInput.SwitchCurrentActionMap("EmptyMap");
        gameControls.SetActive(false);
        temp_stage_clear.SetActive(true);
    }

    // Health
    public void set_health(int game_health, string cause=""){
        health = game_health;

        if(health > maxHealth){
            health = maxHealth;
        }
        if(health >= 0){
            for(int i = 0; i < hearts.Length; i++){
                if(i<health){
                    hearts[i].sprite = full_heart;
                }else{
                    hearts[i].sprite = empty_heart;
                }
            }
        }

        if(health == 0){
            if (cause == "projectile")
            {
                playerDeaths = PlayerDeaths.Projectile;
            }
            else if (cause == "ring")
            {
                playerDeaths = PlayerDeaths.Ring;
            }
            SetLifes(lifes - 1);
            //game_over();
        }
    }
    public int get_life(){
        return health;
    }

    // Lifes
    public void SetLifes(int value)
    {
        bool loseLife = value < lifes;

        lifes = value;
        lifeText.text = value.ToString() + "x";

        if (loseLife)
        {
            playerDead = true;
            player.GetComponent<PlayerController>().animator.SetBool("Dead", true);
            player.GetComponent<Gravity>().gravity = false;
            playerInput.SwitchCurrentActionMap("EmptyMap");
            gameControls.SetActive(false);

            if (lifes <= 0)
            {
                Debug.Log("Game Over");
                tempGameOver.SetActive(true);
            }
            else
            {
                Debug.Log("Death");
                deadObj.SetActive(true);
                // Restart
                // player receives invincibility after + invincibility animation
            }
        }
    }

    // Game Over
    public void game_over(){
        if (!stageClear)
        {
            SetLifes(lifes - 1);

        }
    }
    //public void freeze_game(){
    //    Time.timeScale = 0f;
    //}

    public void restartLevel()
    {
        // Restart when not game over
    }
}
