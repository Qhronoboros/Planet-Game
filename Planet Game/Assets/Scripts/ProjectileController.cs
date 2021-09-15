using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject owner;
    public Vector2 aimDirection = new Vector2(0, 0);
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
        if (other.tag == "Asteroid")
        {
            float temp_score = GameManager.Instance.getScore();
            temp_score += 100;
            GameManager.Instance.setScore(temp_score);

            other.GetComponent<Asteroid>().OnHit();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Planet")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy" && owner.tag != "Enemy")
        {
            other.GetComponent<enemy_fox>().OnHit();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player" && owner.tag != "Player")
        {
            other.GetComponent<PlayerController>().OnHit();
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        transform.Translate(aimDirection * speed * Time.deltaTime);
    }
}
