using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class ItemType01Ctrl : ItemCtrl
    {
        /// <summary>
        /// 20180418 SangBin : Item Hit Effect Prefab Type 01
        /// </summary>
        public GameObject itemHitEffectPrefab_type01;

        /// <summary>
        /// 20180403 SangBin : Item Hit Sound File Type 01
        /// </summary>
        public AudioClip itemHitSoundFile_type01;

        private void Awake()
        {
            base.itemType = 1;
            base.itemHitEffectPrefab = this.itemHitEffectPrefab_type01;
            base.itemHitSoundFile = this.itemHitSoundFile_type01;
        }

        /// <summary>
        /// 20180418 SangBin : Item Hit By Player
        /// </summary>
        private void OnHit()
        {
            base.OnItemHit();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 20180418 SangBin : Item function
        /// </summary>
        override public void ItemFunction()
        {
            //various function in here
        }
    }
}
