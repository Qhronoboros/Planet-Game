using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_fox : MonoBehaviour
{   
    public Transform player;
    private float speed = 8;
    bool finished = false;
    Vector3 offset;
    private Vector2 target;
    private Vector2 position;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        transform.up = -(player.position - transform.position);
        float x_distance = Mathf.Tan(player.rotation.eulerAngles.z) * 10 ;
        Debug.Log(player.rotation.eulerAngles.z);
        if( player.rotation.eulerAngles.z >= 180 && player.rotation.eulerAngles.z < 360){
            target = new Vector2((player.position.x + x_distance), player.position.y + 10);
        }else if( player.rotation.eulerAngles.z >= 0 && player.rotation.eulerAngles.z < 180 ){
            // float x_distance = Mathf.Tan(half) * 10 ;
            // Debug.Log(x_distance+"saddasdasdasdasdasdasd");
            target = new Vector2((player.position.x - x_distance), player.position.y + 10);
        }


        // if (Vector2.Distance(player.position,transform.position)< 10){
        //     Vector3 up = new Vector3(player.position.x,1.0f,player.position.z);
        //     this.transform.Translate( up * speed * Time.deltaTime);
        // }else if (Vector2.Distance(player.position,transform.position)> 10){
        //     Vector3 down = new Vector3(player.position.x,-1.0f,player.position.z);
        //     this.transform.Translate(down * speed * Time.deltaTime);
        // }
        float step = speed * Time.deltaTime;

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        

        // float d = Vector2.Distance(player.position,transform.position);
        // transform.RotateAround(player.position, Vector3.back, speed / (d * 0.2f) * Time.deltaTime);

    }
}
