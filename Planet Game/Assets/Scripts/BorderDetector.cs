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
    public float maxIntensity = 0.45f;
    public float intensity = 0.0f;
    public static List<GameObject> borders = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.player.GetComponent<PlayerController>().ChangePlanet(parentInstance.gameObject);
            borders.Add(gameObject);
            Debug.Log("Player in border " + borders.Count);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            borders.Remove(gameObject);
            Debug.Log("Player outside border " + borders.Count);
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
            Debug.Log("Here");
            intensity = Mathf.Max((parentInstance.calcDistance(GameManager.Instance.player) - transform.localScale.x * 2) / parentInstance.killBorderDistance * maxIntensity, 0.1f);
            Debug.Log((parentInstance.calcDistance(GameManager.Instance.player) - transform.localScale.x * 2) / parentInstance.killBorderDistance * maxIntensity);
        }
        else if (borders.Count != 0 && !GameManager.playerDead)
        {
            intensity = 0;
        }
        vignette.intensity.value = intensity;
    }
}
