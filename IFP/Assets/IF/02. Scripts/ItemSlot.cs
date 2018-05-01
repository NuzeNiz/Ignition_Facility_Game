using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
    private void Start()
    {
        var subject = GameObject.Find("Item_Window").GetComponent<ItemWindow>().itemSubject;

        subject.Attach(new ItemObserver(a => {
            var backgroundImage = gameObject.GetComponent<Image>();
            if (a.selectedItem == gameObject)
            {
                backgroundImage.color = new Color(1.0f, 0, 0);
            }
            else
            {
                backgroundImage.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }));

        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { subject.SetCurruntItem(gameObject); });
    }
}
