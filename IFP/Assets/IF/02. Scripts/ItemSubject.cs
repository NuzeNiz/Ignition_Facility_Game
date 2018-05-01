using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSubject : Subject {
    public GameObject selectedItem { get; private set; }
    public Image itemImage { get { return selectedItem.transform.GetChild(0).GetComponent<Image>(); } }

    public void SetCurruntItem(GameObject item)
    {
        selectedItem = item;
        base.Notify();
    }
}
