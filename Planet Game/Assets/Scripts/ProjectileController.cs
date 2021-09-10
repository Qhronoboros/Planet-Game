using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject planet;
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

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, planet.transform.position, -1 * speed * Time.deltaTime);
    }
}
