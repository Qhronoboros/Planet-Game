using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_fox : MonoBehaviour
{   
    public Transform player;
    private float speed = 6;
    private Vector2 target;
    private Vector2 position;
    public bool follow = false;
    private Vector2 initial_position;
    public int life = 6;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            life-=1;
            if(life == 0){
                Destroy(this.gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        initial_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.up = -(player.position - transform.position);
        if (follow){
            
            float degree_to_radians = player.rotation.eulerAngles.z * (Mathf.PI / 180);
            float y_distance = Mathf.Cos(degree_to_radians)* 10 ;
            float x_distance = Mathf.Sin(degree_to_radians)* 10 ;
            // Debug.Log("rot " + player.rotation.eulerAngles.z + " xdistance :" + x_distance + " ydistance :" + y_distance);

            target = new Vector2((player.position.x - x_distance), (player.position.y + y_distance));
            float step = speed * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, target, step);
            if(player.transform.rotation.z - transform.rotation.z > -0.03 && player.transform.rotation.z - transform.rotation.z < 0.03){
                // ####todo = shoot 
                // Debug.Log("shooooooooooooooooooot");
                int x = 1;
            }else{
                // Debug.Log("not shooooooting");
                int x = 1;
            }
        }else{
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, initial_position, step);
        }
    }
}
