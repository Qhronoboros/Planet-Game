using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{   
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
            Destroy(this.gameObject);

        }
    }
}
