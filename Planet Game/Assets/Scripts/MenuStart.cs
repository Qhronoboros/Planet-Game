using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuStart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");

    }
}
