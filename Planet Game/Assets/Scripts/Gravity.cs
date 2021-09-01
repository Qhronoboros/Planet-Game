using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    public GameObject planet;
    float G;
    static bool gravity = true;

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
        G = (planet.GetComponent<TempPlanetScript>().mass * GetComponent<Rigidbody>().mass / Mathf.Pow(Vector3.Distance(transform.position, planet.transform.position), 2)) * 6.67384e-11f;
        //Debug.Log("Gravitational Constant: " + G.ToString());

        // Pull object to planet
        Vector3 pos = Vector3.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * G * Time.deltaTime;
        
        if (gravity)
        {
            Debug.Log("on");
            GetComponent<Rigidbody>().AddForce(-pos, ForceMode.Acceleration);
        }
        else
        {
            Debug.Log("off");
            //GetComponent<Rigidbody>().AddForce(pos, ForceMode.Acceleration);
        }
    }
}
