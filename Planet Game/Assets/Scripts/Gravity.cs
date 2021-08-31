using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public GameObject planet;
    float G;

    void Start()
    {
    }

    void Update()
    {
        // Calc Gravitational Constant
        G = (1e+14f * GetComponent<Rigidbody>().mass / Mathf.Pow(Vector3.Distance(transform.position, planet.transform.position), 2)) * 6.67384e-11f;
        Debug.Log("Gravitational Constant: " + G.ToString());

        // Move
        Vector3 pos = Vector3.MoveTowards(transform.position, planet.transform.position, 0) * G * Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(-pos);
    }
}
