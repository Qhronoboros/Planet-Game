using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_collision : MonoBehaviour
{
    //public bool rotating = false;

    //void OnBecameVisible()
    //{
    //    Debug.Log(gameObject.name);
    //    rotating = true;
    //    StartCoroutine(RotateToCam());

    //}

    //void OnBecameInvisible()
    //{
    //    Debug.Log(gameObject.name);
    //    rotating = false;
    //    //StopCoroutine(RotateToCam());

    //}

    //IEnumerator RotateToCam()
    //{
    //    Debug.Log("Rotating " + gameObject.name);
    //    transform.up = -(GameManager.Instance.cameraController.gameObject.transform.);
    //    yield return null;
    //}

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
