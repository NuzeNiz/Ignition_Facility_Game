using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IFP;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
public class WeaponSlot : MonoBehaviour
{
    public static List<WeaponSlot> weaponSlots = new List<WeaponSlot>();

    [SerializeField]
    private WeaponCtrl.WeaponTypeEnum myType;
    private WeaponSubject subject;

    private void Awake()
    {
        weaponSlots.Add(gameObject.GetComponent<WeaponSlot>());

        subject = ItemWindow.Instance.WeaponSubject;

        subject.Attach(new WeaponObserver(CheckSelectedWeapon));

        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { subject.SetCurruntWeapon(myType); });

        //subject.Notify();

        //gameObject.SetActive(false);
    }

    public void Init(WeaponCtrl.WeaponTypeEnum type)
    {
        //myType = type;
        gameObject.SetActive(true);
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
