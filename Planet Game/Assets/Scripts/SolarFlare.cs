using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarFlare : MonoBehaviour
{
    public float lifeTime = 3.0f;
    public Coroutine moveCoroutine;
    public bool coroutineRunning = false;
    public float force = 5.0f;

    void Start()
    {
        StartCoroutine(DestroyObject());
    }
    
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(lifeTime);

        if (coroutineRunning)
        {
            StopCoroutine(moveCoroutine);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            coroutineRunning = true;
            collision.GetComponent<PlayerController>().OnHit("Flare");
            moveCoroutine = StartCoroutine(MovePlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            coroutineRunning = false;
            StopCoroutine(moveCoroutine);
        }
    }

    IEnumerator MovePlayer()
    {
        while (true)
        {
            Debug.Log("Move");
            GameManager.Instance.player.GetComponent<Rigidbody2D>().AddForce(force * transform.up, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
