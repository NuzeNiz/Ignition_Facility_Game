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
    [SerializeField]
    private Transform content;

    private List<GameObject> newSlots;

    private void Awake()
    {
        Instance = this;
        WeaponSubject = new WeaponSubject();
        ItemSubject = new ItemSubject();
        newSlots = new List<GameObject>();

        WeaponSubject.Attach(new WeaponObserver(a =>
        {
            if (a.isNewItemNotify)
            {
                var newSlot = Instantiate(slotPrefeb);
                newSlots.Add(newSlot);
                newSlot.GetComponent<WeaponSlot>().Init(a.NewItem);
                WeaponSlot.slotsCount++;
                a.isNewItemNotify = false;
            }
        }));

        WeaponSubject.Notify();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        newSlots.ForEach(a =>
        {
            a.transform.parent = content;
        });
        newSlots = new List<GameObject>();
    }

    public void AddItem(ItemBaseClass item)
    {
       ItemSubject.ItemAcquisition(item);
    }
}
