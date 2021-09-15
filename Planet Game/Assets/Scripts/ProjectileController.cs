using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject owner;
    public float lifeTime = 30.0f;
    public float speed = 0.3f;
    void Start()
    {
        StartCoroutine(selfDestruct());
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Asteroid")
        {
            float temp_score = GameManager.Instance.getScore();
            temp_score += 100;
            GameManager.Instance.setScore(temp_score);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Planet")
        {
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
