using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteWarning : MonoBehaviour
{
    public static float calcIntensity(List<GameObject> planets)
    {
        // ((playerDistance - warningBorderDistance) / fullKillBorderDistance) * maxIntensity

        float intensity = Mathf.Infinity;

        foreach (GameObject planet in planets)
        {
            // Get the minimum value of all the planets
            intensity = Mathf.Min(planet.GetComponent<PlanetScript>().warningBorder.GetComponent<BorderDetector>().calcIntensityPlanet(), intensity);
        }

        return intensity;
    }
}
