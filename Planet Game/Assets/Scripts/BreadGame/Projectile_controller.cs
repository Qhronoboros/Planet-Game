using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_controller : MonoBehaviour
{
    public string owner;
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

        //bullet from player hit asteroid
        if (other.tag == "Asteroid" && owner == "Player")
        {
            float temp_score = Game_Manager.Instance.getScore();
            temp_score += 250;
            Game_Manager.Instance.setScore(temp_score);

            other.GetComponent<Breadship_asteroid>().OnHit();
            Destroy(this.gameObject);
        }
        //bullet from player hit enemy
        else if (other.tag == "Enemy" && owner == "Player")
        {
            other.GetComponent<Enemy_fox_intro>().OnHit();
            Destroy(this.gameObject);
        }
        //bullet from
        else if (other.tag == "Player" && owner == "Enemy" && !Game_Manager.Instance.player.GetComponent<BreadshipController>().invincibility)
        {
            other.GetComponent<BreadshipController>().OnHit();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Planet" && owner == "Enemy"){
            other.GetComponent<planet_breadgame>().OnHit();
            Destroy(this.gameObject);
        }

    }
    private void Update()
    {
        transform.Translate(aimDirection * speed * Time.deltaTime);
    }
}
