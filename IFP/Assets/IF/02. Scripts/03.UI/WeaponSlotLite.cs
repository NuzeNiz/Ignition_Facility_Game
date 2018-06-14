using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IFP;

public class WeaponSlotLite : MonoBehaviour {
    [SerializeField]
    private WeaponCtrl.WeaponTypeEnum thisType;

    private void Awake()
    {
        var image = gameObject.GetComponent<Image>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => WeaponCtrl.instance.SwitchWeapon(thisType));
        button.onClick.AddListener(() => { image.color = Color.red; });
        button.onClick.AddListener(() => ItemWindow.Instance.WeaponSubject.Notify());
    }

    public void BeOrigin()
    {
        var image = gameObject.GetComponent<Image>();
        image.color = Color.white;
    }
}
