using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_magnet : MonoBehaviour
{
    // Start is called before the first frame update
void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            collision.GetComponent<object_collision>().magnet = true;
            collision.GetComponent<object_collision>().magnet_activate();
        }
    }

void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            collision.GetComponent<object_collision>().magnet = false;
            
        }
    }
}
