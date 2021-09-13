using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_collision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "Coin")
        {   
            float temp = GameManager.Instance.get_coin();
            temp+=1;
            GameManager.Instance.set_coin(temp);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "Special_obj")
        {   
            float temp = GameManager.Instance.get_special();
            temp+=1;
            GameManager.Instance.set_special(temp);
            Destroy(this.gameObject);
        }
    }

}
