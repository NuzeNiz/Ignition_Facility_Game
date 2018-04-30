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

        private void OnEnable()
        {
            //StartCoroutine(base.TrackingPlayer());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// 20180418 SangBin : Being Hit By Player
        /// </summary>
        new private void OnHit()
        {
            base.OnHit();
        }

        /// <summary>
        /// 20180430 SangBin : Item Cinnamon function
        /// </summary>
        override public IEnumerator ItemFunction()
        {
            transform.SetPositionAndRotation(PlayerCtrl.PlayerInstance.transform.position + (Vector3.forward * 0.5f), PlayerCtrl.PlayerInstance.transform.rotation);
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Rigidbody>().AddForce((PlayerCtrl.PlayerInstance.transform.forward + PlayerCtrl.PlayerInstance.transform.up) * 10.0f, ForceMode.Force);
            GetComponent<Rigidbody>().useGravity = true;

            yield return new WaitForSeconds(2.0f);

            GameLogicManagement.GLM_Instance.SoundEffect(transform.position, ItemSoundFile);
            GameObject itemEffect = (GameObject)Instantiate(ItemEffectPrefab, transform.position, Quaternion.identity);
            Destroy(itemEffect, 2.0f);

            Collider[] colls = Physics.OverlapSphere(transform.position, 5.0f);
            foreach (Collider coll in colls)
            {
                if (coll.gameObject.tag == "ENEMY_BEE")
                {
                    Rigidbody rbody = coll.GetComponent<Rigidbody>();
                    if (rbody != null)
                    {
                        coll.SendMessage("EnemyKilled", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<SphereCollider>().enabled = true;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 20180427 SangBin : Tracking Player (Called by GLM)
        /// </summary>
        new public void StartTrackingPlayer()
        {
            base.StartTrackingPlayer();
        }
    }
}
