using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IF;

public class ItemSubject : Subject
{
    public ItemBaseClass SelectedItem { get; private set; }

    public void SetCurruntItem(ItemBaseClass item)
    {
        SelectedItem = item;
        base.Notify();
    }

    public void CheckItem()
    {
        if (SelectedItem.gameObject.activeSelf == false)
        {
            SelectedItem = null;
        }
        base.Notify();
    }
}
