using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radar : MonoBehaviour
{   
    public bool parent_follow = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parent_follow = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parent_follow = false;
        }
    }

}
