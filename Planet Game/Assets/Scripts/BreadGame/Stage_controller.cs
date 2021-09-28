using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_controller : MonoBehaviour
{   
    public Transform camera;
    public GameObject popup;
    public GameObject stage_info_obj;
    public Text popup_text;
    public GameObject asteroid_prefab;
    public GameObject Asteroids;
    public GameObject Enemy;
    public GameObject fox_prefab;
    public GameObject planet_prefab;
    public GameObject Planet;
    public GameObject JoyStick_obj;
    public GameObject jumpStick_obj;
    public GameObject shoot_obj;
    public bool movement_popup = false;
    public bool coin_popup = false;
    public bool shoot_popup = false;
    public bool jump_popup = false;
    public bool spawning = false;
    public bool spawn_fox = false;
    public bool spawn_planet = false;
    public float asteroid_spawn_distance = 13.0f;

    public void Resume(){
        Time.timeScale = 1f;
    }
    public void Pause(){
        //pause_ui.SetActive(true);
        Time.timeScale = 0.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(stage_info_text());
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if(camera.position.x >= 15.0f && !movement_popup ){
            movement_popup = true;
            Pause();
            popup_text.text = "Use the left JoyStick to move";
            popup.SetActive(true);
        }

        if(camera.position.x >= 35.0f && !coin_popup ){
            coin_popup = true;
            Pause();
            popup_text.text = "Collect coins to get Score";
            popup.SetActive(true);
        }
        if(camera.position.x >= 79.0f && spawning == false ){
            StartCoroutine(asteroidSpawn());
        }    
        if(camera.position.x >= 80.0f && !shoot_popup ){
            shoot_popup = true;
            Pause();
            popup_text.text = "Evade or Shoot asteroids with the shoot button on the right";
            popup.SetActive(true);
        }
        if(camera.position.x >= 120.0f && !jump_popup ){
            jump_popup = true;
            Pause();
            popup_text.text = "Drag and release the right joystick to launch";
            popup.SetActive(true);
        }
        if(camera.position.x >= 275.5f && !spawn_planet ){
            spawn_planet = true;
            GameObject planeten = Instantiate(this.planet_prefab,new Vector2(300.0f,-4.4f) , this.transform.rotation);
            planeten.transform.SetParent(Planet.transform);
        }   
        if(camera.position.x >= 299.5f && !spawn_fox ){
            spawn_fox = true;
            GameObject fox = Instantiate(this.fox_prefab,new Vector2(camera.position.x + asteroid_spawn_distance, 0f) , this.transform.rotation);
            fox.transform.SetParent(Enemy.transform);
        }                
    }

    IEnumerator stage_info_text()
    {
        yield return new WaitForSeconds(0.5f);
        stage_info_obj.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        stage_info_obj.SetActive(false);
    }
    IEnumerator asteroidSpawn()
    {   
        spawning = true;
        // Instantiate
        int amount =  Random.Range(1, 3);
        float wait_time =  Random.Range(2.0f, 10.0f);
        instantiate_asteroid(amount);
        yield return new WaitForSeconds(wait_time);
        spawning = false;
        
        
    }

    public void instantiate_asteroid(int amount){
        for(int i = 0 ; i < amount; i++){
            float pos_y =  Random.Range(-4.0f, 4.0f);
            GameObject asteroid = Instantiate(this.asteroid_prefab,new Vector2(camera.position.x + asteroid_spawn_distance, pos_y) , this.transform.rotation);
            asteroid.transform.SetParent(Asteroids.transform);
        }
    }

    public void blink_image(){
        if(movement_popup && !coin_popup && !shoot_popup && !jump_popup){
            StartCoroutine(blink(JoyStick_obj));
        }
        if(movement_popup && shoot_popup && !jump_popup){
            StartCoroutine(blink(shoot_obj));
        }
        if(movement_popup && shoot_popup && jump_popup){
            StartCoroutine(blink(jumpStick_obj));
        }
    }



    IEnumerator blink(GameObject blink_obj)
    {   
        blink_obj.GetComponent<Image>().material = Game_Manager.Instance.ui_blinking_mat;
        yield return new WaitForSeconds(5f);
        blink_obj.GetComponent<Image>().material = null;
    }
}