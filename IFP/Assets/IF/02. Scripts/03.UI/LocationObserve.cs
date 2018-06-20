using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
public class LocationObserve : MonoBehaviour {

    enum Chapter
    {
        chapter01,
        chapter02,
        chapter03
    }

    private Button myBotton;
    private Image myImage;
    private Color originColor;

    [SerializeField]
    private MyLocationChecker MyLocation;

    [SerializeField]
    private Chapter MyChapter;

    private void Awake()
    {
        myBotton = gameObject.GetComponent<Button>();
        myImage = gameObject.GetComponent<Image>();
        originColor = myImage.color;
    }
    private void Update()
    {
        if (MyLocation.isInLocation)
        {
            var isMe = false;

            #region Contents Cheack
            switch (MyChapter)
            {
                case Chapter.chapter01:
                    if (MyLocation.contents == "first")
                        isMe = true;
                    break;
                case Chapter.chapter02:
                    if (MyLocation.contents == "second")
                        isMe = true;
                    break;
                case Chapter.chapter03:
                    if (MyLocation.contents == "third")
                        isMe = true;
                    break;
                default:
                    isMe = false;
                    break;
            }
            #endregion

            if (isMe)
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
