using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    private GameObject planetObj;
    private PlanetScript planetScript;
    public List<GameObject> planetsOrbiting = new List<GameObject>();
    public bool gravity = true;
    float G;

    public void GravityToggle(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            gravity = !gravity;
            Debug.Log("gravity:" + gravity.ToString());
        }
    }

    public void assignPlanet(GameObject planet)
    {
        planetObj = planet;
        planetScript = planet.GetComponent<PlanetScript>();
    }

    // Reset Static Variables
    private void Awake()
    {
        gravity = true;
    }

    void FixedUpdate()
    {
        if (gravity)
        {
            float highestG = 0;
            GameObject planetHighestG = planetObj;
            foreach (GameObject planet in planetsOrbiting)
            {
                // Calc Gravitational Constant
                G = (planet.GetComponent<PlanetScript>().mass * GetComponent<Rigidbody2D>().mass / Mathf.Pow(Vector2.Distance(transform.position, planet.transform.position), 2)) * 6.67384e-11f;
                //Debug.Log("Gravitational Constant: " + G.ToString());

                // Find the planet with the highest G
                if (G > highestG)
                {
                    highestG = G;
                    planetHighestG = planet;
                }

                // Applies gravity
                GetComponent<Rigidbody2D>().AddForce((planet.transform.position - transform.position) * G * Time.deltaTime, ForceMode2D.Force);
            }

            // Assign planet with highest G as mainPlanetObj
            if (planetHighestG != planetObj && tag == "Player")
            {
                GameManager.Instance.player.GetComponent<PlayerController>().ChangePlanet(planetHighestG);
            }

        }
        else
        {
            //float acceleration = (planetScript.fullKillBorderDistance - planetScript.calcDistance(gameObject, false));
            //// Pull object to planet
            //Vector3 pos = Vector2.MoveTowards(transform.position, planetObj.transform.position, Time.deltaTime) * acceleration * Time.deltaTime;

            //// Applies reverse gravity
            //GetComponent<Rigidbody2D>().AddForce(pos, ForceMode2D.Force);
        }
    }
}
