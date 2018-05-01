using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 20180423 SeongJun : Open item window button
/// </summary>
public class ItemWindowOpen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject itemWindow;

    public void OnPointerDown(PointerEventData eventData)
    {
        itemWindow.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        itemWindow.SetActive(false);
    }
}
