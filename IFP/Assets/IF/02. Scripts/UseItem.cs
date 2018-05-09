using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using IF;

public class UseItem : MonoBehaviour{
    public Sprite setImage;

    private ItemBaseClass itemInfo;

    private Image myImage;

    private void Awake()
    {
        myImage = gameObject.GetComponent<Image>();

        var subject = ItemWindow.Instance.ItemSubject;
        var button = gameObject.GetComponent<Button>();

        subject.Attach(new ItemObserver(a => {
            itemInfo = a.SelectedItem;
            if (a.SelectedItem != null)
            {
                var childImage = gameObject.transform.GetChild(0).GetComponent<Image>();
                childImage.sprite = setImage;
                childImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }));

        button.onClick.AddListener(() => {
            if (itemInfo == null)
            {

            }
            else
            {
                StartCoroutine(itemInfo.ItemFunction());
            }
        });
    }
}
