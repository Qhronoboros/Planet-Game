using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private PlanetScript planetScript;
    public CameraStates cameraState = CameraStates.PlanetView;
    public Cinemachine.CinemachineVirtualCamera VCamPlanet;
    public Cinemachine.CinemachineVirtualCamera VCamPlayer;
    public Cinemachine.CinemachineVirtualCamera VCamBorder;
    public float planetCamDistance;
    public float playerCamDistance;

    // Enums for the 2 camera states
    public enum CameraStates
    {
        PlanetView,
        PlayerView,
        BorderView
    }

    // Update camera settings according to planet
    public void UpdateCameraSettings(GameObject planetObj)
    {
        planetScript = planetObj.GetComponent<PlanetScript>();

        planetCamDistance = planetScript.planetRadius;
        playerCamDistance = planetScript.planetRadius + 1.0f;
        Debug.Log(planetScript.planetRadius);
        Debug.Log(playerCamDistance);
    }

    private void Update()
    {
        foreach (GameObject collectable in GameManager.Instance.collectablesOnScreen)
        {
            collectable.transform.eulerAngles = transform.rotation.eulerAngles;
        }


        if (cameraState == CameraStates.PlanetView)
        {
            VCamPlayer.transform.rotation = transform.rotation;
        }

        if (planetScript.calcDistance(GameManager.Instance.player, true) >= playerCamDistance && !GameManager.playerDead && cameraState != CameraStates.PlayerView)
        {
            //Debug.Log(planetScript.calcDistance(GameManager.Instance.player, true));
            //Debug.Log(playerCamDistance);
            //Debug.Log("Transitioning player");
            TransitionPlayer();
        }
        else if (planetScript.calcDistance(GameManager.Instance.player, true) < planetCamDistance && !GameManager.playerDead && cameraState != CameraStates.PlanetView)
        {
            TransitionPlanet();
        }
        else if (GameManager.playerDeaths == GameManager.PlayerDeaths.Border && GameManager.playerDead && cameraState != CameraStates.BorderView)
        {
            VCamBorder.Follow = null;
            VCamBorder.LookAt = null;
            TransitionBorder();
        }
    }

    public void TransitionPlanet()
    {
        VCamPlanet.gameObject.GetComponent<CinemachineCameraOffset>().m_Offset = new Vector3(0, planetScript.planetRadius + 5, -10);
        VCamPlanet.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets[0].target = planetScript.gameObject.transform;

        VCamPlanet.m_Lens.OrthographicSize = planetScript.planetRadius + 5;

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
