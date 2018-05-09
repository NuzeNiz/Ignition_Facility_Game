using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IF;

public class ItemWindow : MonoBehaviour
{
    public static ItemWindow Instance { get; private set; }

    public WeaponSubject WeaponSubject { get; private set; }
    public ItemSubject ItemSubject { get; private set; }

    public GameObject slotPrefeb;

    private List<GameObject> weaponSlots;
    private List<ItemBaseClass> itemList;

    private int ownItems = 0;

    private void Awake()
    {
        Instance = this;
        WeaponSubject = new WeaponSubject();
        ItemSubject = new ItemSubject();
        weaponSlots = new List<GameObject>();
        itemList = new List<ItemBaseClass>();

        var content = gameObject.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            weaponSlots.Add(content.GetChild(i).gameObject);
        }
        if (weaponSlots.Count > 0)
        {
            WeaponSubject.SetCurruntWeapon(weaponSlots[0]);
        }

        gameObject.SetActive(false);
    }

    public void AddItem(ItemBaseClass item)
    {
        //bool isStored = false;
        //if (itemList.Count != 0)
        //{
        //    itemList.ForEach(it => { isStored = it.ItemType == item.ItemType ? true : false; });
        //}
        gameObject.transform.GetChild(1).GetChild(ownItems).GetComponent<ItemSlot>().itemInfo = item;
    }
}
