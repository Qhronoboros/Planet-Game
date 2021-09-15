using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class BorderDetector : MonoBehaviour
{
    public PlanetScript parentInstance;

    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject warning;
    public GameObject tempGameOver;
    Vignette vignette;
    public static float intensity = 0.0f;
    public static List<GameObject> borders = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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

    // Reset Static Variables
    private void Awake()
    {
        warning = GameManager.Instance.warning;

        Vignette tmp;
        if (warning.GetComponent<Volume>().profile.TryGet(out tmp))
        {
            vignette = tmp;
        }
    }

    // Gives the vignette effect
    private void Update()
    {
        if (borders.Count == 0 && !GameManager.playerDead && parentInstance.gameObject == GameManager.Instance.getPlayerPlanet())
        {
            intensity = VignetteWarning.calcIntensity(GameManager.Instance.player.GetComponent<PlayerController>().gravity.planetsOrbiting);
        }
        else if (borders.Count != 0 && !GameManager.playerDead && parentInstance.gameObject == GameManager.Instance.getPlayerPlanet())
        {
            intensity = 0;
        }
        vignette.intensity.value = intensity;
    }

    public float calcIntensityPlanet()
    {
        return Mathf.Min(Mathf.Max((parentInstance.calcDistance(GameManager.Instance.player, false) - (parentInstance.warningBorderDistance / 2)) 
            / (parentInstance.killBorderDistance / 2) * GameManager.Instance.maxIntensity, 0.1f), GameManager.Instance.maxIntensity);
    }
}
