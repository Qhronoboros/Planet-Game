using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public GameObject bread_parent;
    public GameObject coin_parent;

    // Materials
    public Material defaultMat;
    public Material damagedMat;
    public Material invincibleMat;
    public Material vignetteMat;
    public Material grayscaleMat;
    public Material whiteFadeMat;

    //score
    public GameObject score_text;
    private Text UIText;
    private float score = 0;
    //coin
    public GameObject coin_text;
    private Text UI_coin_text;
    private float coin = 0;
    //special
    public GameObject specialPlutoPref;
    public GameObject specialSaturnPref;
    public GameObject specialSunPref;
    public GameObject special_text;
    public GameObject special_child;
    private Text UI_special_text;
    private float special = 0;
    public float max_special = 5;
    //bread
    public float bread = 0;
    public Text breadtext;

    //star
    public int star = 0;
    public Text startext;
    public int star_max = 1;
    // Lifes
    public GameObject deadObj;
    public Text lifeText;
    public int lifes = 5;
    // Health Points
    private int health = 3;
    private int maxHealth = 3;
    public Image[] hearts;
    public Image heart;
    public Sprite full_heart;
    public Sprite empty_heart;
    //game over
    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject tempGameOver;
    public GameObject temp_stage_clear;
    public static bool playerDead = false;
    public static PlayerDeaths playerDeaths = PlayerDeaths.Alive;
    public Coroutine grayscaleCoroutine;
    // Stage Clear
    public static bool stageClear = false;
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
        //PlayerPrefs.DeleteAll();
        _instance = this;
        playerDead = false;
        stageClear = false;
        playerDeaths = PlayerDeaths.Alive;
        UIText = score_text.GetComponent<Text>();
        UI_coin_text = coin_text.GetComponent<Text>();
        UI_special_text = special_text.GetComponent<Text>();
        if(PlayerPrefs.HasKey("item2")){
            if(PlayerPrefs.GetInt("item2") == 1){
                Image[] temp = new Image[hearts.Length + 1] ;
                temp[0] = hearts[0];
                temp[1] = hearts[1];
                temp[2] = hearts[2];
                temp[3] = heart;
                hearts = temp;
                heart.gameObject.SetActive(true);
                maxHealth +=1;
                health+=1;
            }
        }
        get_max_special();
        set_special(special);
        set_coin(SaveGameManager.Instance.get_coin());
        set_bread(SaveGameManager.Instance.get_bread());
        // set_star(star);
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

    public float get_bread(){
        return this.bread;
        
    }
    public void set_bread(float bread_amount){
        this.bread = bread_amount;
        breadtext.text = this.bread.ToString();
    }
    public int get_star(){
        return star;
    }
    public void set_star(int star_amount){
        this.star = star_amount;
        startext.text = this.star.ToString() + "/ " + this.star_max.ToString();

    }
    //stage clear
    public void stage_clear(){
        stageClear = true;
        print("saveeeeed???????");
        SaveGameManager.Instance.save_coin(SaveGameManager.Instance.get_coin() + get_coin());
        SaveGameManager.Instance.unlock_level(SceneManager.GetActiveScene().buildIndex+1);
        SaveGameManager.Instance.Save();
        StartCoroutine(DelayRepairAnimation());
        playerInput.SwitchCurrentActionMap("EmptyMap");
        gameControls.SetActive(false);
        temp_stage_clear.SetActive(true);
    }

    IEnumerator DelayRepairAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        planets[0].GetComponent<Animator>().SetBool("IsRepaired", true);
    }

    // Health
    public void set_health(int game_health, string cause=""){
        if (!stageClear)
        {
            health = game_health;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
            if (health >= 0)
            {
                for (int i = 0; i < hearts.Length; i++)
                {
                    if (i < health)
                    {
                        hearts[i].sprite = full_heart;
                    }
                    else
                    {
                        hearts[i].sprite = empty_heart;
                    }
                }
            }

            if (health == 0)
            {
                if (cause == "projectile")
                {
                    playerDeaths = PlayerDeaths.Projectile;
                }
                else if (cause == "ring")
                {
                    playerDeaths = PlayerDeaths.Ring;
                }
                SetLifes(lifes - 1);
            }
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
            grayscaleCoroutine = StartCoroutine(grayscale());
            player.GetComponent<PlayerController>().animator.SetBool("Dead", true);
            PlayerController.helmet.transform.parent = null;
            PlayerController.helmet.SetActive(true);
            PlayerController.helmet.GetComponent<Rigidbody2D>().AddForce(0.001f * Random.insideUnitCircle.normalized, ForceMode2D.Impulse);
            player.GetComponent<Gravity>().gravity = false;
            playerInput.SwitchCurrentActionMap("EmptyMap");
            gameControls.SetActive(false);

            Debug.Log(playerDeaths);

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


    IEnumerator grayscale()
    {
        Instance.grayscaleMat.color = new Color(1, 1, 1, 1);
        player.GetComponent<SpriteRenderer>().material = grayscaleMat;

        while (grayscaleMat.color.r > 0.4f)
        {
            yield return new WaitForSeconds(0.05f);
            grayscaleMat.color -= new Color(0.02f, 0.02f, 0.02f, 0.0f);
        }
    }
}
