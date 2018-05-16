using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using IF;

public class ItemSubject : Subject
{
    private List<ItemObserver> observeOnCheckItem = new List<ItemObserver>();

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
            observeOnCheckItem.ForEach(a => { a.Reactive(this); });
            SelectedItem = null;
        }
        base.Notify();
    }

    public void AttachOnCheckItem(ItemObserver observe)
    {
        observeOnCheckItem.Add(observe);
    }
}
