using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet_breadgame : MonoBehaviour
{   
    public GameObject Enemy_parent;
    int bullet_counter = 0;

    // OnHit by enemy projectile
    public void OnHit()
    {
        bullet_counter += 1;
        if (bullet_counter == 6)
        {
            GetComponent<Animator>().enabled = true;
            // Enemy_parent = transform.parent.parent.GetComponent<Stage_controller>().Planet;
            // Enemy_parent.transform.getChild(0).GetComponent<Enemy_fox_intro>().planet_destroyed = true;
            transform.parent.parent.Find("Enemy").transform.GetChild(0).GetComponent<Enemy_fox_intro>().planet_destroyed = true;
            transform.parent.parent.Find("Enemy").transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void Start(){
        GetComponent<Animator>().enabled = false;
    }
}
