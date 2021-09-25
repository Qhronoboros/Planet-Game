using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{   
    public string state = "next";
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player")
        {   

            state = "current";

        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Player")
        {   
            transform.parent.GetComponent<Background_rotation>().rotate_bg();
            state = "last";

            
        }
    }
}
