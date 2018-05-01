using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Click_Effect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image myImage;
    private Color idleColor;
    private Color downColor;

    private void Awake()
    {
        myImage = gameObject.GetComponent<Image>();
        idleColor = myImage.color;
        downColor = idleColor;
        downColor.a = 1.0f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        myImage.color = downColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        myImage.color = idleColor;
    }
}
