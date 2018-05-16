using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObserver : Observer
{
    public Action<WeaponSubject> ObserverDelegate;

    public WeaponObserver(Action<WeaponSubject> action)
    {
        ObserverDelegate += action;
    }

    public override void Reactive(Subject sub)
    {
        if (sub is WeaponSubject)
        {
            ObserverDelegate(sub as WeaponSubject);
        }
    }
}
