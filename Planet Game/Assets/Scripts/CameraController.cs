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
    public Cinemachine.CinemachineVirtualCamera VCamWin;
    public NoiseSettings noiseProfile;
    public float planetCamDistance;
    public float playerCamDistance;

    // Enums for the 2 camera states
    public enum CameraStates
    {
        PlanetView,
        PlayerView,
        WinView
    }

    // Update camera settings according to planet
    public void UpdateCameraSettings(GameObject planetObj)
    {
        VCamPlayer.Follow = GameManager.Instance.player.transform;
        VCamPlayer.LookAt = GameManager.Instance.player.transform;

        Transform planet = GameManager.Instance.planets[0].transform;
        VCamWin.Follow = planet;
        VCamWin.LookAt = planet;

        planetScript = planetObj.GetComponent<PlanetScript>();

        planetCamDistance = planetScript.planetRadius;
        playerCamDistance = planetScript.planetRadius + 1.0f;
    }

    public void ResetPlanetCam()
    {
        VCamPlanet.gameObject.GetComponent<CinemachineCameraOffset>().m_Offset = new Vector3(0, planetScript.planetRadius + 5, -10);
        VCamPlanet.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets[0].target = planetScript.gameObject.transform;

        VCamPlanet.m_Lens.OrthographicSize = planetScript.planetRadius + 5;
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


        if (GameManager.stageClear && cameraState != CameraStates.WinView)
        {
            TransitionWin();
        }
        else if (!planetScript.isPlanet || planetScript.calcDistance(GameManager.Instance.player, true) >= playerCamDistance && !GameManager.playerDead && !GameManager.stageClear && cameraState != CameraStates.PlayerView)
        {
            //Debug.Log(planetScript.calcDistance(GameManager.Instance.player, true));
            //Debug.Log(playerCamDistance);
            //Debug.Log("Transitioning player");
            TransitionPlayer();
        }
        else if (planetScript.isPlanet && planetScript.calcDistance(GameManager.Instance.player, true) < planetCamDistance && !GameManager.playerDead && !GameManager.stageClear && cameraState != CameraStates.PlanetView)
        {
            TransitionPlanet();
        }
        else if (GameManager.playerDeaths == GameManager.PlayerDeaths.Border && GameManager.playerDead && !GameManager.stageClear)
        {
            VCamPlayer.Follow = null;
            VCamPlayer.LookAt = null;
        }
    }

    public void TransitionPlanet()
    {
        ResetPlanetCam();

        VCamPlanet.Priority = 1;
        VCamPlayer.Priority = 0;
        cameraState = CameraStates.PlanetView;
    }

    public void TransitionPlayer()
    {
        VCamPlanet.Priority = 0;
        VCamPlayer.Priority = 1;
        cameraState = CameraStates.PlayerView;
    }

    public void TransitionWin()
    {
        //VCamWin.m_Lens.OrthographicSize = planetScript.planetRadius + 10;

        VCamPlanet.Priority = 0;
        VCamPlayer.Priority = 0;
        VCamWin.Priority = 1;
        cameraState = CameraStates.PlayerView;
    }

    // Shake camera when planet gets hit by asteroid
    public IEnumerator CameraShake()
    {
        VCamPlanet.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        VCamPlanet.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = noiseProfile;
        yield return new WaitForSeconds(0.2f);
        VCamPlanet.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = null;
    }
}
