using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
public class ChangeImage : MonoBehaviour {
    private void Awake()
    {
        var myImage = gameObject.GetComponent<Image>();
        var subject = GameObject.Find("Item_Window").GetComponent<ItemWindow>().WeaponSubject;
        subject.Attach(new WeaponObserver(a =>
        {
            myImage.sprite = a.WeaponImage.sprite;
            myImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }));
    }
}
