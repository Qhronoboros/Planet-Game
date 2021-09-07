using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraStates cameraState = CameraStates.PlanetView;
    public Cinemachine.CinemachineVirtualCamera VCamPlanet;
    public Cinemachine.CinemachineVirtualCamera VCamPlayer;

    // Enums for the 2 camera states
    public enum CameraStates
    {
        PlanetView,
        PlayerView
    }
    private void Update()
    {
        if (PlayerController.distance >= 22.5f && cameraState == CameraStates.PlanetView)
        {
            TransitionPlayer();
        }
        else if (PlayerController.distance < 20.0f && cameraState == CameraStates.PlayerView)
        {
            TransitionPlanet();
        }
    }

    public void TransitionPlanet()
    {
        VCamPlayer.Priority = 0;
        VCamPlanet.Priority = 1;
        cameraState = CameraStates.PlanetView;
    }

    public void TransitionPlayer()
    {
        VCamPlanet.Priority = 0;
        VCamPlayer.Priority = 1;
        cameraState = CameraStates.PlayerView;
    }
}
