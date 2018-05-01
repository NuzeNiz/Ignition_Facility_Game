using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWindow : MonoBehaviour {
    public ItemSubject itemSubject { get; private set; }

    private List<GameObject> itemSlots;

    void Awake()
    {
        itemSubject = new ItemSubject();
        itemSlots = new List<GameObject>();
    }

    void Start () {
        var content = gameObject.transform.GetChild(0).GetChild(0);
        for(int i = 0; i < content.childCount; i++)
        {
            itemSlots.Add(content.GetChild(i).gameObject);
        }
        if (itemSlots.Count > 0)
        {
            itemSubject.SetCurruntItem(itemSlots[0]);
        }
	}

    //public void AddItem(GameObject item)
    //{
    //    var slot = (GameObject)Instantiate(itemSlotPrefep);
    //    item.transform.parent = slot.transform;
    //    slot.transform.parent = gameObject.transform.GetChild(0).GetChild(0);
    //    itemSlots.Add(slot);
    //}
}
