using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_collision : MonoBehaviour
{
    public bool rotating = false;

    void OnBecameVisible()
    {
        GameManager.Instance.collectablesOnScreen.Add(gameObject);
    }

    void OnBecameInvisible()
    {
        GameManager.Instance.collectablesOnScreen.Remove(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInParent<AudioSource>().Play();
            if (gameObject.tag == "Coin")
            {
                float temp = GameManager.Instance.get_coin();
                temp += 1;
                GameManager.Instance.set_coin(temp);
                Destroy(gameObject);
            }
            if (gameObject.tag == "Special_obj")
            {
                float temp = GameManager.Instance.get_special();
                temp += 1;
                GameManager.Instance.set_special(temp);
                Destroy(gameObject);
            }
        }
    }

}
