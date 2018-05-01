using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {
    private void Awake()
    {
        var myImage = gameObject.GetComponent<Image>();
        var subject = GameObject.Find("Item_Window").GetComponent<ItemWindow>().itemSubject;
        subject.Attach(new ItemObserver(a =>
        {
            myImage.sprite = a.itemImage.sprite;
            myImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }));
    }
}
