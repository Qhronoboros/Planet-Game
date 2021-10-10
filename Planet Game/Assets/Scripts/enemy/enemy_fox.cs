using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy_fox : MonoBehaviour
{   
    //healthbar
    public GameObject health_bar;
    public Transform player;
    public GameObject[] item_prefab;
    private float speed = 8;
    public float distance_above = 15;
    private Vector2 target;
    private Vector2 position;
    public bool follow = false;
    private Vector2 initial_position;
    public int health ;
    public int max_health = 6;
    public int item_id = 1;

    // Shooting
    public GameObject projectilePrefab;
    public float timeLastProjectile = 0.0f;
    public float shootDelay = 3.0f;
    public float shootDelayBurst = 0.4f;

    public bool damaged = false;
    public Coroutine lastCoroutineDamage;
    public Coroutine lastCoroutineShooting;
    public bool shooting = false;
    public bool dead = false;

    // On hit bullet
    public void OnHit()
    {
        if (health != 0)
        {
            if (damaged)
            {
                StopCoroutine(lastCoroutineDamage);
                damaged = false;
            }

            health -= 1;
            health_bar.GetComponent<enemy_healthbar>().set_health_text(health.ToString() + "/" + max_health.ToString());
            health_bar.GetComponent<enemy_healthbar>().set_health(health);

            if (health <= max_health/2 && !GetComponent<Animator>().GetBool("IsBroken"))
            {
                GetComponent<Animator>().SetBool("IsBroken", true);
            }

            if (health == 0)
            {
                dead = true;
                if (shooting)
                {
                    StopCoroutine(lastCoroutineShooting);
                }
                StartCoroutine(DestroySelf());
            }
            else
            {
                lastCoroutineDamage = StartCoroutine(Damaged());
            }
        }
    }

    // Using damaged shader
    IEnumerator Damaged()
    {
        damaged = true;
        GetComponent<SpriteRenderer>().material = GameManager.Instance.damagedMat;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().material = GameManager.Instance.defaultMat;
        damaged = false;
    }

    IEnumerator DestroySelf()
    {
        GetComponentInParent<AudioSource>().Play();

        //Animate
        GetComponent<SpriteRenderer>().material = GameManager.Instance.whiteFadeMat;
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer renderer = GetComponent<Renderer>();

        renderer.GetPropertyBlock(propBlock);

        for (int i=0; i <= 30; i++)
        {
            float colorValue = i / 30.0f;
            Color color = new Color(colorValue, colorValue, colorValue, colorValue);
            propBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(propBlock);

            yield return new WaitForSeconds(0.03f);
        }

        float temp_score = GameManager.Instance.getScore();
        temp_score += 500;
        GameManager.Instance.setScore(temp_score);

        GameObject item = Instantiate(item_prefab[item_id], this.transform.position, this.transform.rotation);
        if (item.tag == "Special_obj")
        {
            item.transform.parent = GameManager.Instance.special_child.transform;
        }
        else if (item.tag == "Bread")
        {
            item.transform.parent = GameManager.Instance.bread_parent.transform;
        }
        else if (item.tag == "Coin")
        {
            item.transform.parent = GameManager.Instance.coin_parent.transform;
        }
        this.GetComponent<SpecificObject>().DestroySaveable();
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "stage 1":
                item_prefab[1] = GameManager.Instance.specialPlutoPref;
                break;
            case "stage 2":
                item_prefab[1] = GameManager.Instance.specialSaturnPref;
                break;
            case "stage 3":
                item_prefab[1] = GameManager.Instance.specialSunPref;
                break;
            default:
                item_prefab[1] = GameManager.Instance.specialPlutoPref;
                break;
        }

        initial_position = transform.position;
        player = GameManager.Instance.player.transform;
        health = max_health;
        health_bar.GetComponent<enemy_healthbar>().set_max_health(max_health);
        health_bar.GetComponent<enemy_healthbar>().set_health_text( health.ToString() + "/" + max_health.ToString());
    }

    // Update is called once per frame
    void Update()
    {   
        transform.up = -(player.position - transform.position);
        if (GetComponentInChildren<radar>().parent_follow){
            
            float degree_to_radians = player.rotation.eulerAngles.z * (Mathf.PI / 180);
            float y_distance = Mathf.Cos(degree_to_radians)* distance_above ;
            float x_distance = Mathf.Sin(degree_to_radians)* distance_above ;
            // Debug.Log("rot " + player.rotation.eulerAngles.z + " xdistance :" + x_distance + " ydistance :" + y_distance);

            target = new Vector2((player.position.x - x_distance), (player.position.y + y_distance));
            float step = speed * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, target, step);
            if(player.transform.rotation.z - transform.rotation.z > -0.03 && player.transform.rotation.z - transform.rotation.z < 0.03)
            {
                if (!GameManager.playerDead && !GameManager.stageClear && Time.time - timeLastProjectile > shootDelay && !dead)
                {
                    timeLastProjectile = Time.time;
                    lastCoroutineShooting = StartCoroutine(Shooting());
                }
            }
        }else{
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, initial_position, step);
        }
    }

    IEnumerator Shooting()
    {
        shooting = true;
        for (int i=0; i < 3; i++)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position + transform.up, transform.rotation);
            laser.GetComponent<ProjectileController>().owner = gameObject.tag;
            laser.GetComponent<ProjectileController>().aimDirection = Vector2.down;

            yield return new WaitForSeconds(shootDelayBurst);
        }
        shooting = false;
    }
}
