using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class ItemType02Ctrl : ItemBaseClass
    {


        /// <summary>
        /// 20180418 SangBin : Item Type
        /// </summary>
        [HideInInspector]
        override public ItemTypeEnum ItemType { get { return ItemTypeEnum.playerPotion; } }

        /// <summary>
        /// 20180418 SangBin : Sound File when item is collided
        /// </summary>
        [SerializeField]
        private AudioClip collsionSF;

        /// <summary>
        /// 20180418 SangBin : Sound File when item is collided
        /// </summary>
        override protected AudioClip CollsionSF
        {
            get
            {
                return collsionSF;
            }
        }

        /// <summary>
        /// 20180418 SangBin : Effect Prefab when item is collided
        /// </summary>
        [SerializeField]
        private GameObject collEffectPrefab;

        /// <summary>
        /// 20180418 SangBin : Effect Prefab when item is collided
        /// </summary>
        override protected GameObject CollEffectPrefab
        {
            get
            {
                return collEffectPrefab;
            }
        }

        /// <summary>
        /// 20180430 SangBin : Sound File when item is used
        /// </summary>
        [SerializeField]
        private AudioClip itemSoundFile;

        /// <summary>
        /// 20180430 SangBin : Sound File when item is used
        /// </summary>
        override protected AudioClip ItemSoundFile
        {
            get
            {
                return itemSoundFile;
            }
        }

        /// <summary>
        /// 20180430 SangBin : Effect Prefab when item is used
        /// </summary>
        [SerializeField]
        private GameObject itemEffectPrefab;

        /// <summary>
        /// 20180430 SangBin : Effect Prefab when item is used
        /// </summary>
        override protected GameObject ItemEffectPrefab
        {
            get
            {
                return itemEffectPrefab;
            }
        }

        //---------------------------------------------------------------------------------------------------------

        protected override void OnDisable()
        {
            base.OnDisable();

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }

        /// <summary>
        /// 20180418 SangBin : Being Hit By Player
        /// </summary>
        new private void OnHit()
        {
            base.OnHit();
        }

        /// <summary>
        /// 20180618 SangBin : Item potion function
        /// </summary>
        override public IEnumerator ItemFunction()
        {
            while (gameObject.activeSelf)
            {
                GameManagement.instance.SoundEffect(transform.position, ItemSoundFile);

                if (PlayerCtrl.instance.PlayerHP >= (PlayerCtrl.instance.PlayerMaxHP - 200.0d ))
                {
                    PlayerCtrl.instance.PlayerHP = PlayerCtrl.instance.PlayerMaxHP;
                }
                else
                {
                    PlayerCtrl.instance.PlayerHP += 200.0d;
                }

                GameUIManagement.instance.PlayerDamaged();

                gameObject.SetActive(false);
                //ItemWindow.Instance.ItemSubject.CheakConsumedItem();
            }
            yield break;
        }
    }
}
