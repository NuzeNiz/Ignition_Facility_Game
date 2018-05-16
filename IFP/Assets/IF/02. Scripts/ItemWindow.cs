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
    private List<ItemBaseClass> itemSlots;

    private void Awake()
    {
        Instance = this;
        WeaponSubject = new WeaponSubject();
        ItemSubject = new ItemSubject();
        weaponSlots = new List<GameObject>();
        itemSlots = new List<ItemBaseClass>();

        var content = gameObject.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            weaponSlots.Add(content.GetChild(i).gameObject);
        }
        if (weaponSlots.Count > 0)
        {
            WeaponSubject.SetCurruntWeapon(weaponSlots[0]);
        }

        ItemSubject.AttachOnCheckItem(new ItemObserver(a =>
        {
            itemSlots.Remove(a.SelectedItem);
        }));

        gameObject.SetActive(false);
    }

    public void AddItem(ItemBaseClass item)
    {
        var n = 0;
        var maxChild = gameObject.transform.GetChild(1).childCount;
        var addSuccess = false;

        while ((!addSuccess) && (n < maxChild))
        {
            addSuccess = gameObject.transform.GetChild(1).GetChild(n).GetComponent<ItemSlot>().SetItemInfo(item);
            n++;
        }

        if (addSuccess)
        {
            itemSlots.Add(item);
            if (ItemSubject.SelectedItem == null)
            {
                ItemSubject.SetCurruntItem(item);
            }
        }
    }
}
