using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_fox : MonoBehaviour
{   
    private float speed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        if(transform.position.z <= -10){
            float RNG = Random.Range(-30f , 30f);
            transform.position = new Vector3(RNG , 0 , 0);
        }
    }
}
