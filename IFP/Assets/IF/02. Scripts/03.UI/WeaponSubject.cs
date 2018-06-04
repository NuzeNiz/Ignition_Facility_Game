using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IF;

public class WeaponSubject : Subject {
    private WeaponCtrl weaponCtrl = WeaponCtrl.instance;

    public WeaponCtrl.WeaponTypeEnum SelectedItem { get { return weaponCtrl.CurrentWeaponType; } }
    public WeaponCtrl.WeaponTypeEnum NewItem { get; private set; }
    public bool isNewItemNotify = false;
    public Image WeaponImage { get { return null; } }

    public void SetCurruntWeapon(WeaponCtrl.WeaponTypeEnum item)
    {
        weaponCtrl.SwitchWeapon(item);
        //Notify();
    }

    public void NotifyNewItem(WeaponCtrl.WeaponTypeEnum item)
    {
        isNewItemNotify = true;
        NewItem = item;
        Notify();
    }
}
