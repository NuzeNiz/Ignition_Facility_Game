using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 20180423 SeongJun : Open item window button
/// </summary>
public class ItemWindowOpen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //public GameObject itemWindow;

    public void OnPointerDown(PointerEventData eventData)
    {
        ItemWindow.Instance.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ItemWindow.Instance.gameObject.SetActive(false);
    }
}
