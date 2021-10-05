using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarFlare : MonoBehaviour
{
    public float lifeTime = 3.0f;

    void Start()
    {
        StartCoroutine(DestroyObject());
    }
    
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
