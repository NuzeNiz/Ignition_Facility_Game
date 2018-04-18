using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    [RequireComponent(typeof(AudioSource))]
    abstract public class ItemCtrl : MonoBehaviour
    {
        /// <summary>
        /// 20180418 SangBin : Item Type
        /// </summary>
        protected int itemType = 0;

        ///// <summary>
        ///// 20180418 SangBin : Item Type Property
        ///// </summary>
        //public int ItemType { get { return itemType; } set { itemType = value; } }

        /// <summary>
        /// 20180418 SangBin : Item Hit Effect Prefab
        /// </summary>
        protected GameObject itemHitEffectPrefab;


        /// <summary>
        /// 20180403 SangBin : Item Hit Sound File
        /// </summary>
        protected AudioClip itemHitSoundFile;

        //-----------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 20180418 SangBin : Item Hit By Player
        /// </summary>
        protected void OnItemHit()
        {   
            gameObject.tag = "Untagged";
            GameLogicManagement.GM_Instance.SoundEffect(gameObject.transform.position, itemHitSoundFile);
            GameObject itemHitEffect = (GameObject)Instantiate(itemHitEffectPrefab, gameObject.transform.position, Quaternion.identity);
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;


            Destroy(itemHitEffect, 2.0f);
            //ItemFunction();
        }

        /// <summary>
        /// 20180418 SangBin : Item function
        /// </summary>
        abstract public void ItemFunction();
    }
}
