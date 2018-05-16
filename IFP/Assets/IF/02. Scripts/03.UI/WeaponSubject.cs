using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IF;

public class WeaponSubject : Subject {
    public GameObject SelectedItem { get; private set; }
    public Image ItemImage { get { return SelectedItem.transform.GetChild(0).GetComponent<Image>(); } }

    public void SetCurruntWeapon(GameObject item)
    {
        SelectedItem = item;
        base.Notify();
    }
}
