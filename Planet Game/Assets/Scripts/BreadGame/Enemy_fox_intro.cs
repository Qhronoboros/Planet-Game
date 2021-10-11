using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_fox_intro : MonoBehaviour
{
//healthbar
    public GameObject health_bar;
    private float speed = 6f;
    private float second_speed = 2f;
    private Vector2 target;
    private Vector2 position;
    // public bool follow = false;
    private Vector2 initial_position = new Vector2(306.0f,0f);
    public int health ;
    public int max_health = 6;
    public bool in_action = false;
    public bool planet_destroyed = false;
    public int action = 0;
    bool pos_1 = false;
    bool pos_2 = false;
    bool pos_3 = false;


    // Shooting
    public GameObject projectilePrefab;
    public float timeLastProjectile = 0.0f;
    public float shootDelay = 3.0f;
    public float shootDelayBurst = 0.4f;

    public bool damaged = false;
    public Coroutine lastCoroutine;

    // On hit bullet
    public void OnHit()
    {
        if (planet_destroyed)
        {
            if (damaged)
            {
                StopCoroutine(lastCoroutine);
                damaged = false;
            }
            lastCoroutine = StartCoroutine(Damaged());

            if (Cheats.instaKill)
            {
                health = 0;
            }
            else
            {
                health -= 1;
            }

            health_bar.GetComponent<enemy_healthbar>().set_health_text(health.ToString() + "/" + max_health.ToString());
            health_bar.GetComponent<enemy_healthbar>().set_health(health);

            // if((health/max_health)*100f < 25f){

            //     print("kill player");
            //     // kill_player();
            // }

            if (health <= max_health / 2 && !GetComponent<Animator>().GetBool("IsBroken"))
            {
                GetComponent<Animator>().SetBool("IsBroken", true);
            }

            if (health == 0)
            {
                Destroy(this.gameObject);
                Game_Manager.Instance.stage_clear();
                Game_Manager.Instance.nextStage = "stage 1";
            }
        }
    }

    // Using damaged shader
    IEnumerator Damaged()
    {
        damaged = true;
        GetComponent<SpriteRenderer>().material = Game_Manager.Instance.damagedMat;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().material = Game_Manager.Instance.defaultMat;
        damaged = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        // initial_position = transform.position;
        // player = GameManager.Instance.player.transform;
        health = max_health;
        health_bar.GetComponent<enemy_healthbar>().set_max_health(max_health);
        health_bar.GetComponent<enemy_healthbar>().set_health_text( health.ToString() + "/" + max_health.ToString());
    }

    // Update is called once per frame
    void Update()
    {   
        if(!planet_destroyed){
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, initial_position, step);
            if(Mathf.Abs(transform.position.x - initial_position.x) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                timeLastProjectile = Time.time;
                StartCoroutine(Shooting());
            }

        }

        if(!in_action && planet_destroyed){
            action =  Random.Range(1, 5);
            in_action = true;
        }

        if (in_action && action == 1){  
            if(!pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, 0f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    timeLastProjectile = Time.time;
                    StartCoroutine(Shooting_straight());
                    pos_1 = true;
                }

            }
            if(pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, 4f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    timeLastProjectile = Time.time;
                    StartCoroutine(Shooting_straight());
                    pos_2 = true;
                }

            }
            if(pos_1 && pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, -4f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    timeLastProjectile = Time.time;
                    StartCoroutine(Shooting_straight());
                    pos_3 = true;
                }
            }
            if(pos_1 && pos_2 && pos_3){
                in_action = false;
                reset_actions();
            }
            
        }
        if (in_action && action == 2){  
            if(!pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, 0f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    timeLastProjectile = Time.time;
                    StartCoroutine(Shooting_three_directional());
                    pos_1 = true;
                }
            }
            if(pos_1 && !pos_2 && !pos_3){
                in_action = false;
                reset_actions();
            }
        }

        if (in_action && action == 3){  
            if(!pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, -4f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    timeLastProjectile = Time.time;
                    StartCoroutine(Shooting_rows());
                    pos_1 = true;
                }
            }
            if(pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, 4f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    pos_2 = true;
                }

            }
            if(pos_1 && pos_2 && !pos_3){
                in_action = false;
                reset_actions();
            }

        }
        if (in_action && action == 4){  
            if(!pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, 4f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    timeLastProjectile = Time.time;
                    StartCoroutine(Shooting_rows());
                    pos_1 = true;
                }
            }
            if(pos_1 && !pos_2 && !pos_3){
                Vector2 target_position = new Vector2(308f, -4f);
                float step = second_speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);                
                if(Mathf.Abs(transform.position.y - target_position.y) < 0.2f && Time.time - timeLastProjectile > shootDelay ){
                    pos_2 = true;
                }

            }
            if(pos_1 && pos_2 && !pos_3){
                in_action = false;
                reset_actions();
            }

        }

    }

    IEnumerator Shooting()
    {
        for (int i=0; i < 3; i++)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<Projectile_controller>().owner = gameObject.tag;
            laser.GetComponent<Projectile_controller>().aimDirection = new Vector2(-1f,-1f);

            yield return new WaitForSeconds(shootDelayBurst);
        }
    }
    IEnumerator Shooting_rows()
    {   second_speed = 3f;
        for (int i=0; i < 5; i++)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<Projectile_controller>().owner = gameObject.tag;
            laser.GetComponent<Projectile_controller>().aimDirection = new Vector2(-1f,0f);

            yield return new WaitForSeconds(shootDelayBurst);
        }
        second_speed= 2f;
    }
    IEnumerator Shooting_straight()
    {   
        second_speed = 0f;
        for (int i=0; i < 3; i++)
        {   
            
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<Projectile_controller>().owner = gameObject.tag;
            laser.GetComponent<Projectile_controller>().aimDirection = new Vector2(-1f,0f);

            yield return new WaitForSeconds(shootDelayBurst);
        }
        second_speed = 2f;
    }
    IEnumerator Shooting_three_directional()
    {   
        second_speed = 0f;
        for (int i=0; i < 3; i++)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            GameObject laser1 = Instantiate(projectilePrefab, transform.position, transform.rotation);
            GameObject laser2 = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<Projectile_controller>().owner = gameObject.tag;
            laser.GetComponent<Projectile_controller>().aimDirection = new Vector2(-1f,0f);
            laser1.GetComponent<Projectile_controller>().owner = gameObject.tag;
            laser1.GetComponent<Projectile_controller>().aimDirection = new Vector2(-1f,0.2f);
            laser2.GetComponent<Projectile_controller>().owner = gameObject.tag;
            laser2.GetComponent<Projectile_controller>().aimDirection = new Vector2(-1f,-0.2f);

            yield return new WaitForSeconds(shootDelayBurst);
        }
        second_speed = 2f;
    }
    IEnumerator wait_for(float t_ime)
    {   
        
        yield return new WaitForSeconds(t_ime);
    }
    void reset_actions(){
        pos_1 = false;
        pos_2 = false;
        pos_3 = false;
    }
}
