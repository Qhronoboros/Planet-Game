using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public string owner;
    public string type;
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
            temp_score += 250;
            GameManager.Instance.setScore(temp_score);

            other.GetComponent<Asteroid>().OnHit();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Planet")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy" && owner != "Enemy")
        {   
            if (other.GetComponent<enemy_fox>() != null){
                other.GetComponent<enemy_fox>().OnHit();
            }
            else if (other.GetComponent<enemy_dog>() != null){
                other.GetComponent<enemy_dog>().OnHit();
            }else if (other.GetComponent<enemy_cat>() != null){
                other.GetComponent<enemy_cat>().OnHit();
            }

            Destroy(this.gameObject);
        }
        else if (other.tag == "Player" && owner != "Player" && !GameManager.Instance.player.GetComponent<PlayerController>().invincibility)
        {
            other.GetComponent<PlayerController>().OnHit("projectile");
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        transform.Translate(aimDirection * speed * Time.deltaTime);
    }
}
