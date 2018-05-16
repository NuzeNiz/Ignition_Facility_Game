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

    public Transform itemslot1;
    public Transform itemslot2;

    public GameObject slotPrefeb;

    private List<GameObject> weapons;

    private void Awake()
    {
        Instance = this;
        WeaponSubject = new WeaponSubject();
        ItemSubject = new ItemSubject();
        weapons = new List<GameObject>();

        var content = gameObject.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            weapons.Add(content.GetChild(i).gameObject);
        }
        if (weapons.Count > 0)
        {
            WeaponSubject.SetCurruntWeapon(weapons[0]);
        }

        gameObject.SetActive(false);
    }

    public void AddItem(ItemBaseClass item)
    {
       ItemSubject.ItemAcquisition(item);
    }
}
