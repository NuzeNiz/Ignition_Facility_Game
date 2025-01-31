﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    [RequireComponent(typeof(AudioSource))]
    abstract public class ItemBaseClass : MonoBehaviour
    {
        #region Fields : Item Type
        /// <summary>
        /// 20180418 SangBin : Item Type Enum
        /// </summary>
        [HideInInspector]
        public enum ItemTypeEnum { boom, playerPotion, treePotion, itemType04 };

        /// <summary>
        /// 20180418 SangBin : Item Type
        /// </summary>
        abstract public ItemTypeEnum ItemType { get; }
        #endregion

        #region Fields : Sound Files
        /// <summary>
        /// 20180403 SangBin : Sound File when item is collided
        /// </summary>
        abstract protected AudioClip CollsionSF { get; }

        /// <summary>
        /// 20180430 SangBin : Sound File when item is collided
        /// </summary>
        abstract protected AudioClip ItemSoundFile { get; }
        #endregion

        #region Fields : Prefabs
        /// <summary>
        /// 20180418 SangBin : Effect Prefab when item is collided
        /// </summary>
        abstract protected GameObject CollEffectPrefab { get; }

        /// <summary>
        /// 20180430 SangBin : Effect Prefab when item is collided
        /// </summary>
        abstract protected GameObject ItemEffectPrefab { get; }
        #endregion

        #region Fields : Tracking
        /// <summary>
        /// 20180418 SangBin : Normalized Vector From This Item To Player 
        /// </summary>
        private Vector3 directionVector_Normalized;

        /// <summary>
        /// 20180418 SangBin : Item Moving Speed
        /// </summary>
        private float movingSpeed = 50.0f;
        #endregion

        /// <summary>
        /// 20180418 SangBin : Item Tag
        /// </summary>
        string itemTag;

        private bool isColl = false;
        //-----------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 20180418 SangBin :
        /// </summary>
        protected void Awake()
        {
            itemTag = gameObject.tag;
        }

        /// <summary>
        /// 20180418 SangBin :
        /// </summary>
        protected void OnEnable()
        {
            StartCoroutine(TrackingPlayer());
        }

        /// <summary>
        /// 20180418 SangBin :
        /// </summary>
        virtual protected void OnDisable()
        {
            StopAllCoroutines();
            tag = itemTag;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<SphereCollider>().enabled = true;
        }

        /// <summary>
        /// 20180418 SangBin : Item function
        /// </summary>
        abstract public IEnumerator ItemFunction();

        /// <summary>
        /// 20180418 SangBin : Being Hit By Player
        /// </summary>
        protected void OnHit()
        {
            tag = "Untagged";
            GameManagement.instance.SoundEffect(transform.position, CollsionSF);
            GameObject itemHitEffect = (GameObject)Instantiate(CollEffectPrefab, transform.position, Quaternion.identity);
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            Destroy(itemHitEffect, 2.0f);

            ObtainingItem();
        }

        /// <summary>
        /// 20180427 SangBin : Collision & Obtaining Item
        /// 20180508 SeongJun : Notify to ItemWindow
        /// </summary>
        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isColl = true;
                StopCoroutine(TrackingPlayer());
                GameManagement.instance.SoundEffect(transform.position, CollsionSF);
                GameObject itemCollEffect = (GameObject)Instantiate(CollEffectPrefab, transform.position, Quaternion.identity);
                Destroy(itemCollEffect, 2.0f);

                ObtainingItem();

                ItemWindow.Instance.AddItem(this);
            }
        }

        /// <summary>
        /// 20180427 SangBin : Tracking Player
        /// </summary>
        protected IEnumerator TrackingPlayer()
        {
            while (!isColl)
            {
                directionVector_Normalized = Vector3.Normalize(PlayerCtrl.instance.PlayerTr.position - transform.position);
                transform.LookAt(PlayerCtrl.instance.PlayerTr);

                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                GetComponent<Rigidbody>().AddForce(directionVector_Normalized * movingSpeed, ForceMode.Force);
                yield return new WaitForSeconds(0.5f);
            }
        }

        /// <summary>
        /// 20180427 SangBin : Obtaining Item
        /// </summary>
        virtual protected void ObtainingItem()
        {
            tag = "Untagged";
            StopCoroutine(TrackingPlayer());
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            transform.position = PlayerCtrl.instance.PlayerTr.up * 5.0f;
            //transform.position = (Vector3.back * 5.0f);
        }
    }
}
