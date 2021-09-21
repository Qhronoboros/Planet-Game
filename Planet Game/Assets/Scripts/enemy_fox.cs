using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_fox : MonoBehaviour
{   
    public Transform player;
    public GameObject item_prefab;
    private float speed = 8;
    private Vector2 target;
    private Vector2 position;
    public bool follow = false;
    private Vector2 initial_position;
    public int life = 6;
    public int item_id = 1;


    // Shooting
    public GameObject projectilePrefab;
    public float timeLastProjectile = 0.0f;
    public float shootDelay = 3.0f;
    public float shootDelayBurst = 0.4f;

    // On hit bullet
    public void OnHit()
    {
        life-=1;
        if(life == 0){
            destroy_self();
        }
    }

    public void destroy_self(){
        float temp_score = GameManager.Instance.getScore();
        temp_score += 500;
        GameManager.Instance.setScore(temp_score);

        if(item_id == 1){
            Instantiate(item_prefab,this.transform.position,this.transform.rotation);
        }
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        initial_position = transform.position;
        player = GameManager.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!GameManager.playerDead)
        {
            transform.up = -(player.position - transform.position);
            if (follow)
            {

                float degree_to_radians = player.rotation.eulerAngles.z * (Mathf.PI / 180);
                float y_distance = Mathf.Cos(degree_to_radians) * 10;
                float x_distance = Mathf.Sin(degree_to_radians) * 10;
                // Debug.Log("rot " + player.rotation.eulerAngles.z + " xdistance :" + x_distance + " ydistance :" + y_distance);

                target = new Vector2((player.position.x - x_distance), (player.position.y + y_distance));
                float step = speed * Time.deltaTime;

                // move sprite towards the target location
                transform.position = Vector2.MoveTowards(transform.position, target, step);
                if (player.transform.rotation.z - transform.rotation.z > -0.03 && player.transform.rotation.z - transform.rotation.z < 0.03)
                {
                    if (!GameManager.playerDead && Time.time - timeLastProjectile > shootDelay)
                    {
                        timeLastProjectile = Time.time;
                        StartCoroutine(Shooting());
                    }
                    // Debug.Log("shooooooooooooooooooot");
                    int x = 1;
                }
                else
                {
                    // Debug.Log("not shooooooting");
                    int x = 1;
                }
            }
            else
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, initial_position, step);
            }
        }
    }


    IEnumerator Shooting()
    {
        for (int i=0; i < 3; i++)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position + transform.up, transform.rotation);
            laser.GetComponent<ProjectileController>().owner = gameObject.tag;
            laser.GetComponent<ProjectileController>().aimDirection = Vector2.down;

            yield return new WaitForSeconds(shootDelayBurst);
        }
    }
}
