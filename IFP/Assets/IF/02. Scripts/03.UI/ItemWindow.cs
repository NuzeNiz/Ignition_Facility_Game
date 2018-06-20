using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IFP;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
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
                var newSlot = WeaponSlot.weaponSlots.Where(slot=> { return slot.gameObject.activeSelf == false; }).First();
                if (newSlot != null)
                {
                    newSlots.Add(newSlot.gameObject);
                    newSlot.Init(a.NewItem);
                    a.isNewItemNotify = false;
                }
            }
        }));

        //WeaponSubject.Notify();

        gameObject.SetActive(false);
    }

    //private void Start()
    //{
    //    //WeaponSubject.NotifyNewItem(WeaponCtrl.WeaponTypeEnum.weaponType01);
    //    //WeaponSubject.NotifyNewItem(WeaponCtrl.WeaponTypeEnum.weaponType02);
    //    //WeaponSubject.NotifyNewItem(WeaponCtrl.WeaponTypeEnum.weaponType03);
    //    //WeaponSubject.NotifyNewItem(WeaponCtrl.WeaponTypeEnum.weaponType04);

    //    WeaponSubject.Notify();

    //    gameObject.SetActive(false);
    //}

    public void AddItem(ItemBaseClass item)
    {
       ItemSubject.ItemAcquisition(item);
    }
}
