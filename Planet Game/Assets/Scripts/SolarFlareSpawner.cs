using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarFlareSpawner : MonoBehaviour
{
    public GameObject solarFlarePref;
    public float offset = 3.3f;
    public float spawnDelayMax = 5.0f;
    public float spawnDelayMin = 1.0f;
    public float spawnAmountMax = 2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SolarFlareGeneration());
    }

    IEnumerator SolarFlareGeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
            for (int i=0; i <= Random.Range(0, 2); i++)
            {
                SpawnSolarFlare();
            }
        }
    }

    public void SpawnSolarFlare()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = direction * GetComponentInParent<PlanetScript>().planetRadius + direction * offset;

        float angle = Mathf.Atan2(spawnPos.y - transform.parent.position.y, spawnPos.x - transform.parent.position.x) * Mathf.Rad2Deg;
        Quaternion spawnRot = Quaternion.Euler(new Vector3(0, 0, angle-90));

        GameObject solarFlare = Instantiate(solarFlarePref, spawnPos, spawnRot);
        solarFlare.transform.parent = transform;
    }
}
