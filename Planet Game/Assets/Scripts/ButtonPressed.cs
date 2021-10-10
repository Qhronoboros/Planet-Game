using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    public AudioSource canvasSource;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = defaultSprite;
        if (canvasSource != null)
        {
            canvasSource.Play();
        }
    }
}
