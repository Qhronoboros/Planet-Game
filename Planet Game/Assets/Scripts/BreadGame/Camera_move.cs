using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour
{   
    public float movespeed = 5.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(transform.position.x > 299.5){
            movespeed = 0.0f;
        }
        transform.Translate(new Vector2(1,0) * movespeed * Time.deltaTime );
    }
}
