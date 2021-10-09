using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_collision : MonoBehaviour
{
    public GameObject player;
    public GameObject mainPlanetObj;
    public float speed = 8;
    public bool magnet = false;

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
                float temp_score = GameManager.Instance.getScore();
                temp_score += 100;
                GameManager.Instance.setScore(temp_score);
                this.GetComponent<SpecificObject>().DestroySaveable();
                Destroy(gameObject);
            }
            if (gameObject.tag == "Special_obj")
            {//
                float temp = GameManager.Instance.get_special();
                temp += 1;
                GameManager.Instance.set_special(temp);
                this.GetComponent<SpecificObject>().DestroySaveable();
                Destroy(gameObject);
            }
            if (gameObject.tag == "Star")
            {   
                int temp = GameManager.Instance.get_star();
                temp += 1;
                GameManager.Instance.set_star(temp);
                this.GetComponent<SpecificObject>().DestroySaveable();
                Destroy(gameObject);
            }
            if (gameObject.tag == "Bread")
            {   
                float temp = GameManager.Instance.get_bread();
                temp += 1f;
                GameManager.Instance.set_bread(temp);
                this.GetComponent<SpecificObject>().DestroySaveable();
                Destroy(gameObject);
            }
        }
    }
        
    IEnumerator go_in_zone()
    {   
        yield return new WaitForSeconds(1.0f);
        // float d_world_border = mainPlanetObj.GetComponent<PlanetScript>().fullKillBorderDistance;
        float half_d_world_border = mainPlanetObj.GetComponent<PlanetScript>().fullKillBorderDistance * 0.5f;
        float d_world_thisObj = Vector2.Distance(mainPlanetObj.transform.position , this.transform.position);
        // Debug.Log("world distance " + half_d_world_border + " name " + this.name + " dword : "+ d_world_thisObj);
        if(d_world_thisObj >= half_d_world_border){
            Vector2 target_position = player.transform.position;
            while (Vector2.Distance(target_position, this.transform.position) > 0)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target_position, step);
                //Wait for a frame to give Unity and other scripts chance to run
                yield return null;
            }   
        }
    }

    IEnumerator magnetized()
    {   

        while (magnet == true)
        {
            Vector2 target_position = player.transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target_position, step);
            //Wait for a frame to give Unity and other scripts chance to run
            yield return null;
        }       
    }

    public void magnet_activate(){
        StartCoroutine(magnetized());
    }

    


    void Start(){
        if (!mainPlanetObj)
        {
            GameObject closestPlanet = null;
            float smallestDistance = Mathf.Infinity;

            foreach (GameObject planet in GameManager.Instance.planets)
            {
                float distance = planet.GetComponent<PlanetScript>().calcDistance(gameObject, false);
                if (distance < smallestDistance)
                {
                    closestPlanet = planet;
                    smallestDistance = distance;
                }
            }

            mainPlanetObj = closestPlanet;
        }
        if (!player){
            player = GameManager.Instance.player;
        }

        if (this.gameObject.tag != "Star"){
            StartCoroutine(go_in_zone());
        }


    }

}
