using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationObserve : MonoBehaviour {
    private Button myBotton;
    private Image myImage;
    private Color originColor;

    [SerializeField]
    private MyLocationChecker MyLocation;

    private void Awake()
    {
        myBotton = gameObject.GetComponent<Button>();
        myImage = gameObject.GetComponent<Image>();
        originColor = myImage.color;
    }
    private void Update()
    {
        if (MyLocation.isInGroup)
        {
            if (MyLocation.isInLocation)
            {
                myImage.color = Color.white;
            }
            else
            {
                myImage.color = originColor;
            }
        }
        else
        {
            myImage.color = originColor;
        }
    }
}
