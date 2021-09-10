using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CameraStates cameraState = CameraStates.PlanetView;
    public Cinemachine.CinemachineVirtualCamera VCamPlanet;
    public Cinemachine.CinemachineVirtualCamera VCamPlayer;
    public Cinemachine.CinemachineVirtualCamera VCamBorder;
    public float planetCamDistance = 20.0f;
    public float playerCamDistance = 22.5f;

    // Enums for the 2 camera states
    public enum CameraStates
    {
        PlanetView,
        PlayerView,
        BorderView
    }

    private void Update()
    {
        if (PlayerController.distance >= playerCamDistance && !GameManager.playerDead && cameraState != CameraStates.PlayerView)
        {
            TransitionPlayer();
        }
        else if (PlayerController.distance < planetCamDistance && !GameManager.playerDead && cameraState != CameraStates.PlanetView)
        {
            TransitionPlanet();
        }
        else if (GameManager.playerDead && cameraState != CameraStates.BorderView)
        {
            VCamBorder.Follow = null;
            VCamBorder.LookAt = null;
            TransitionBorder();
        }
    }

    public void TransitionPlanet()
    {
        VCamPlanet.Priority = 1;
        VCamPlayer.Priority = 0;
        VCamBorder.Priority = 0;
        cameraState = CameraStates.PlanetView;
    }

    public void TransitionPlayer()
    {
        VCamPlanet.Priority = 0;
        VCamPlayer.Priority = 1;
        VCamBorder.Priority = 0;
        cameraState = CameraStates.PlayerView;
    }

    public void TransitionBorder()
    {
        VCamPlanet.Priority = 0;
        VCamPlayer.Priority = 0;
        VCamBorder.Priority = 1;
        cameraState = CameraStates.BorderView;
    }
}
