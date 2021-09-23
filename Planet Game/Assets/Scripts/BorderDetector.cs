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
        //Vignette tmp;
        //if (warning.GetComponent<Volume>().profile.TryGet(out tmp))
        //{
        //    vignette = tmp;
        //}
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

        GameManager.Instance.vignetteMat.SetColor("_VColor", new Color(1, Mathf.Max(1-intensity, 0.0f), Mathf.Max(1-intensity, 0.0f), 1));
        GameManager.Instance.vignetteMat.SetFloat("_VRadius", Mathf.Max(1.0f - intensity*0.8f, 0.0f));
        GameManager.Instance.vignetteMat.SetFloat("_VSoft", Mathf.Min(intensity * 2, 1.0f));
    }

    public float calcIntensityPlanet()
    {
        return Mathf.Min(Mathf.Max((parentInstance.calcDistance(GameManager.Instance.player, false) - (parentInstance.warningBorderDistance / 2)) 
            / (parentInstance.killBorderDistance / 2) * GameManager.Instance.maxIntensity, 0.1f), GameManager.Instance.maxIntensity);
    }
}
