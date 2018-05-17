using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IF;

public class WeaponSlot : MonoBehaviour
{
    public static int slotsCount = 0;

    private WeaponCtrl.WeaponTypeEnum myType;
    private WeaponSubject subject;

    private void Awake()
    {
        subject = ItemWindow.Instance.WeaponSubject;

        subject.Attach(new WeaponObserver(CheckSelectedWeapon));

        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { subject.SetCurruntWeapon(myType); });

        subject.Notify();
    }

    public void Init(WeaponCtrl.WeaponTypeEnum type)
    {
        myType = type;
        CheckSelectedWeapon(subject);
    }

    private void CheckSelectedWeapon(WeaponSubject a)
    {
        var backgroundImage = gameObject.GetComponent<Image>();
        if (a.SelectedItem == myType)
        {
            backgroundImage.color = new Color(1.0f, 0, 0);
        }
        else
        {
            backgroundImage.color = new Color(1.0f, 1.0f, 1.0f);
        }
    }
}
