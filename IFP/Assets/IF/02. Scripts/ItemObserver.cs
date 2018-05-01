using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObserver : Observer
{
    public Action<ItemSubject> ObserverDelegate;

    public ItemObserver(Action<ItemSubject> action)
    {
        ObserverDelegate += action;
    }

    public override void Reactive(Subject sub)
    {
        if (sub is ItemSubject)
        {
            ObserverDelegate(sub as ItemSubject);
        }
    }
}
