using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplayer : MonoBehaviour
{
    [SerializeField]
    private RectTransform rotationAxis;

    [SerializeField]
    private Image HPImage;

    //public double SetHP
    //{
    //    private get { return SetHP; }
    //    set
    //    {
    //        HPImage.fillAmount = (float)(value / 100.0d);
    //        SetHP = value;

    //        if (SetHP <= 0.0d)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            gameObject.SetActive(true);
    //        }
    //    }
    //}

    public void SetHP(double currentHP)
    {
        HPImage.fillAmount = (float)(currentHP / 100.0d);
        if (currentHP <= 0.0d)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        HPImage.fillAmount = 1.0f;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        rotationAxis.LookAt(GameObject.Find("ARCore Device").transform);
    }
}
