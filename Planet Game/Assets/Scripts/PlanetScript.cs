using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class PlanetScript : MonoBehaviour
{
    public float mass = 7e+14f;

    public GameManager gameManager;
    public PlayerInput playerInput;
    public GameObject gameControls;
    public GameObject warning;
    public GameObject tempGameOver;

    Vignette vignette;
    public float maxIntensity = 0.45f;
    public float intensity = 0.0f;
    public float warningBorderDistance = 40.0f;
    public float killBorderDistance = 20.0f;
    public static float fullKillBorderDistance;
    public static bool playerInBorder = true;
}
