using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyCancelButton : MonoBehaviour
{
    private Action<GameObject> callBack;
    private bool iDid = false;

    private void OnEnable()
    {
        if (callBack == null)
        {
            return;
        }
        callBack(gameObject);
    }

    private void OnDisable()
    {
        iDid = false;
    }

    public void Set(Action<GameObject> action)
    {
        if (!iDid)
        {
            callBack = action;
            iDid = true;
        }
    }
}
