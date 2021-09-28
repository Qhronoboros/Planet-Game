using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    public string firstStage;
    public string stage;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SceneManager.LoadSceneAsync(firstStage, LoadSceneMode.Single);
    }
    public void ButtonClicked()
    {
    	stage = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadSceneAsync(stage, LoadSceneMode.Single);
    }
}
