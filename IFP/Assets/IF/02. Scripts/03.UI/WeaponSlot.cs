using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IF;

public class WeaponSlot : MonoBehaviour
{
    private void Awake()
    {
        var subject = ItemWindow.Instance.WeaponSubject;

        subject.Attach(new WeaponObserver(a =>
        {
            var backgroundImage = gameObject.GetComponent<Image>();
            if (a.SelectedItem == gameObject)
            {
                backgroundImage.color = new Color(1.0f, 0, 0);
            }
            else
            {
                backgroundImage.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }));

        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { subject.SetCurruntWeapon(gameObject); });

        subject.Notify();
    }
}
