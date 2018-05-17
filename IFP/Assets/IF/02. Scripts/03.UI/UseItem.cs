using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using IF;

public class UseItem : MonoBehaviour{
    public Sprite setImage;

    public int slotNum = 0;

    private ItemSubject subject;

    private void Awake()
    {
        subject = ItemWindow.Instance.ItemSubject;
        var button = gameObject.GetComponent<Button>();

        subject.Attach(new ItemObserver(a => {
            var childImage = gameObject.transform.GetChild(0).GetComponent<Image>();
            if (a.SelectedItem[slotNum] != null)
            {
                childImage.sprite = setImage;
                childImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                a.itemCount--;
                childImage.sprite = null;
                childImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }));

        button.onClick.AddListener(() => {
            if (subject.SelectedItem[slotNum] == null)
            {

            }
            else
            {
                StartCoroutine(subject.SelectedItem[slotNum].ItemFunction());
                subject.SelectedItem[slotNum] = null;
                subject.Notify();
            }
        });
    }
}
