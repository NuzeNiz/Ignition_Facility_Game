using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IF;

public class ItemSlot : MonoBehaviour {
    public ItemBaseClass itemInfo;
    private void Awake()
    {
        var subject = ItemWindow.Instance.ItemSubject;

        subject.Attach(new ItemObserver(a => {
            var image = gameObject.GetComponent<Image>();
            if (subject.SelectedItem == itemInfo)
            {
                image.color = new Color((float)36 / 255, (float)234 / 255, (float)169 / 255, 1.0f);
            }
            else
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }));

        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { subject.SetCurruntItem(itemInfo); });
    }
}
