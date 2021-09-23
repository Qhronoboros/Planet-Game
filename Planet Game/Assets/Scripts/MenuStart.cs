using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string firstStage;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SceneManager.LoadSceneAsync(firstStage, LoadSceneMode.Single);
    }
}
