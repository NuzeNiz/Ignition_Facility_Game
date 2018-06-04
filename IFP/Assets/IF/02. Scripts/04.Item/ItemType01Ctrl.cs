using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class ItemType01Ctrl : ItemBaseClass
    {
        /// <summary>
        /// 20180418 SangBin : Item Type
        /// </summary>
        [HideInInspector]
        override public ItemTypeEnum ItemType { get { return ItemTypeEnum.cinnamon; } }

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
        /// <summary>
        /// 20180418 SangBin : Being Hit By Player
        /// </summary>
        new private void OnHit()
        {
            base.OnHit();
        }

        /// <summary>
        /// 20180430 SangBin : Item Cinnamon function
        /// 20180516 SeongJun : If item used notify
        /// </summary>
        override public IEnumerator ItemFunction()
        {
            while (gameObject.activeSelf)
            {
                GameManagement.instance.SoundEffect(transform.position, ItemSoundFile);
                transform.SetPositionAndRotation(PlayerCtrl.instance.PlayerTr.position + (PlayerCtrl.instance.PlayerTr.forward), PlayerCtrl.instance.PlayerTr.rotation);
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<Rigidbody>().useGravity = true;
                Vector3 directionVec = PlayerCtrl.instance.PlayerTr.forward + (PlayerCtrl.instance.PlayerTr.up * 0.5f);
                GetComponent<Rigidbody>().AddForce(directionVec * 300.0f, ForceMode.Force);


                yield return new WaitForSeconds(1.0f);


                GameObject itemEffect = (GameObject)Instantiate(ItemEffectPrefab, transform.position, Quaternion.identity);
                Destroy(itemEffect, 2.0f);

                Collider[] colls = Physics.OverlapSphere(transform.position, 10.0f);
                foreach (Collider coll in colls)
                {
                    if (coll.gameObject.layer == 8)
                    {
                        Rigidbody rbody = coll.GetComponent<Rigidbody>();
                        if (rbody != null)
                        {
                            coll.SendMessage("EnemyKilled", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }

                GetComponent<Rigidbody>().useGravity = false;
                //GetComponent<SphereCollider>().enabled = true;
                gameObject.SetActive(false);
                //ItemWindow.Instance.ItemSubject.CheakConsumedItem();
            }
            yield break;
        }
    }
}
