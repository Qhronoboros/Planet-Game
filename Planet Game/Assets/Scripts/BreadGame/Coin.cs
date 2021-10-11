using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{   
    public bool magnet = false;
    public float speed = 8;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            GetComponentInParent<AudioSource>().Play();
            float temp = Game_Manager.Instance.get_coin();
            temp += 1;
            Game_Manager.Instance.set_coin(temp);
            float temp_score = Game_Manager.Instance.getScore();
            temp_score += 100;
            Game_Manager.Instance.setScore(temp_score);
            this.GetComponent<SpecificObject>().DestroySaveable();
            Destroy(this.gameObject);

        }
    }

    IEnumerator magnetized()
    {   
        while (magnet == true)
        {
            Vector2 target_position = Game_Manager.Instance.player.transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target_position, step);
            //Wait for a frame to give Unity and other scripts chance to run
            yield return null;
        }       
    }

    public void magnet_activate(){
        StartCoroutine(magnetized());
    }
}
