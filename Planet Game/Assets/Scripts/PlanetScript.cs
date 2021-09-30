using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetScript : MonoBehaviour
{
    public float mass = 7e+14f;

    public GameObject warningBorder;
    public GameObject killBorder;

    public float warningBorderDistance = 40.0f;
    public float killBorderDistance = 20.0f;
    public float fullKillBorderDistance;

    public float planetRadius;

    public float ringLaunch = 40.0f;

    // Setup warningBorder and killBorder

    private void Awake()
    {
        planetRadius = transform.localScale.x * 3;

        warningBorder = transform.GetChild(0).gameObject;
        killBorder = transform.GetChild(1).gameObject;

        warningBorder.GetComponent<BorderDetector>().parentInstance = this;
        killBorder.GetComponent<KillBorder>().parentInstance = this;

        warningBorder.SetActive(true);
        killBorder.SetActive(true);

        fullKillBorderDistance = warningBorderDistance + killBorderDistance;

        warningBorder.transform.localScale = new Vector3(warningBorderDistance, warningBorderDistance, 0) / transform.localScale.x;
        killBorder.transform.localScale = new Vector3(fullKillBorderDistance, fullKillBorderDistance, 0) / transform.localScale.x;
    }

    // Calculate distance between planet and obj
    public float calcDistance(GameObject obj, bool surface)
    {
        if (surface)
        {
            return Vector2.Distance(obj.transform.position, transform.position) - planetRadius;
        }
        else
        {
            return Vector2.Distance(obj.transform.position, transform.position);
        }
    }

    public void OnHit()
    {
        if (gameObject == GameManager.Instance.getPlayerPlanet() && GameManager.Instance.cameraController.cameraState == CameraController.CameraStates.PlanetView)
        {
            // Camera Shake
            StartCoroutine(GameManager.Instance.cameraController.CameraShake());
        }
    }
}
