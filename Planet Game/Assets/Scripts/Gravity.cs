using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    public GameObject planet;
    float G;
    public bool gravity = true;

    public void GravityToggle(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            gravity = !gravity;
            Debug.Log("gravity:" + gravity.ToString());
        }
    }

    void Start()
    {
    }

    void Update()
    {
        // Calc Gravitational Constant
        G = (planet.GetComponent<TempPlanetScript>().mass * GetComponent<Rigidbody2D>().mass / Mathf.Pow(Vector2.Distance(transform.position, planet.transform.position), 2)) * 6.67384e-11f;
        //Debug.Log("Gravitational Constant: " + G.ToString());

        // Pull object to planet
        Vector3 pos = Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * G * Time.deltaTime;
        
        if (gravity)
        {
            GetComponent<Rigidbody2D>().AddForce(-pos, ForceMode2D.Force);
        }
        else
        {
            //GetComponent<Rigidbody>().AddForce(pos, ForceMode.Acceleration);
        }
    }
}
