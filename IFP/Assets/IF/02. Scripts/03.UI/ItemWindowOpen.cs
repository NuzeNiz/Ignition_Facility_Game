using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using IF;

/// <summary>
/// 20180423 SeongJun : Open item window button
/// </summary>
public class ItemWindowOpen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Sprite[] images = new Sprite[4];

    private void Start()
    {
        var image = transform.GetChild(0).GetComponent<Image>();
        ItemWindow.Instance.WeaponSubject.Attach(new WeaponObserver(a => {
            switch (WeaponCtrl.instance.CurrentWeaponType)
            {
                case WeaponCtrl.WeaponTypeEnum.weaponType01:
                    image.sprite = images[0];
                    break;
                case WeaponCtrl.WeaponTypeEnum.weaponType02:
                    image.sprite = images[1];
                    break;
                case WeaponCtrl.WeaponTypeEnum.weaponType03:
                    image.sprite = images[2];
                    break;
                case WeaponCtrl.WeaponTypeEnum.weaponType04:
                    image.sprite = images[3];
                    break;
            }
        }));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ItemWindow.Instance.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ItemWindow.Instance.gameObject.SetActive(false);
    }
}
