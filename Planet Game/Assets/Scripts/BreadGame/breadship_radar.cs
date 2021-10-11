using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breadship_radar : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Coin")
            {
                collision.GetComponent<Coin>().magnet = true;
                collision.GetComponent<Coin>().magnet_activate();
            }
        }

    void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Coin")
            {
                collision.GetComponent<Coin>().magnet = false;
                
            }
        }
}
