using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class BorderDetector : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject warning;
    public GameObject tempGameOver;
    Vignette vignette;
    public float maxIntensity = 0.45f;
    public float intensity = 0.0f;
    public float killBorderDistance = 10.0f;
    public static float fullKillBorderDistance;
    public static bool playerInBorder = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInBorder = true;
            Debug.Log("Player in border");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInBorder = false;
            Debug.Log("Player outside border");
        }
    }

    // Reset Static Variables
    private void Awake()
    {
        fullKillBorderDistance = transform.localScale.x + killBorderDistance;
        playerInBorder = true;
    }

    private void Start()
    {
        Vignette tmp;
        if (warning.GetComponent<Volume>().profile.TryGet(out tmp))
        {
            vignette = tmp;
        }
    }

    private void Update()
    {
        if (!playerInBorder && !GameManager.playerDead)
        {
            intensity = Mathf.Max((PlayerController.distance - transform.localScale.x/2) / killBorderDistance * maxIntensity, 0.1f);
            vignette.intensity.value = intensity;
            if (PlayerController.distance > transform.localScale.x/2 + killBorderDistance)
            {
                //Kill player
                gameManager.set_life(0);
            }
        }
    }
}