using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using IF;

public class ItemSubject : Subject
{
    private int itemCount = 0;
    public ItemBaseClass[] SelectedItem { get; private set; }

    public ItemSubject()
    {
        SelectedItem = new ItemBaseClass[2];
    }

    public void ItemAcquisition(ItemBaseClass item)
    {
        if (itemCount < 2)
        {
            var emptySlot = 0;
            for (; emptySlot < 2; emptySlot++)
            {
                if (SelectedItem[emptySlot] == null)
                {
                    break;
                }
            }
            SelectedItem[emptySlot] = item;
            itemCount++;
            Notify();
        }
    }

    public void CheakConsumedItem()
    {
        var marker = false;

        for (int i = 0; i < 2; i++)
        {
            if (SelectedItem[i].gameObject.activeSelf == false)
            {
                SelectedItem[i] = null;
                marker = true;
                itemCount--;
            }
        }

        if (marker)
        {
            Notify();
        }
    }
}
