using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BorderDetector : MonoBehaviour
{
    public PlanetScript parentInstance;

    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject warning;
    public GameObject tempGameOver;
    public static float intensity = 0.0f;
    public static List<GameObject> borders = new List<GameObject>();

    private void Awake()
    {
        borders = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !borders.Contains(gameObject))
        {
            borders.Add(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            borders.Remove(gameObject);
        }
    }

    public float calcIntensityPlanet()
    {
        return Mathf.Min(Mathf.Max((parentInstance.calcDistance(GameManager.Instance.player, false) - (parentInstance.warningBorderDistance / 2)) 
            / (parentInstance.killBorderDistance / 2) * GameManager.Instance.maxIntensity, 0.1f), GameManager.Instance.maxIntensity);
    }
}
