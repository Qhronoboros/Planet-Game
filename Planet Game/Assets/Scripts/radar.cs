using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radar : MonoBehaviour
{   
    public GameObject radar_user;
    bool parent_follow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            radar_user.GetComponent<enemy_fox>().follow = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            radar_user.GetComponent<enemy_fox>().follow = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // parent_follow = radar_user.GetComponent<enemy_fox>().follow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
