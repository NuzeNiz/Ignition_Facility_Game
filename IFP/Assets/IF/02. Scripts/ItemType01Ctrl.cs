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
        /// 20180418 SangBin : Item collision Sound File
        /// </summary>
        [SerializeField]
        private AudioClip collSoundFile;

        /// <summary>
        /// 20180418 SangBin : Item collision Sound File
        /// </summary>
        override protected AudioClip ItemCollSoundFile
        {
            get
            {
                return collSoundFile;
            }
        }

        /// <summary>
        /// 20180418 SangBin : Item Collision Effect Prefab
        /// </summary>
        [SerializeField]
        private GameObject collEffectPrefab;

        /// <summary>
        /// 20180418 SangBin : Item Collision Effect Prefab
        /// </summary>
        override protected GameObject ItemCollEffectPrefab
        {
            get
            {
                return collEffectPrefab;
            }
        }

        //---------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            //StartCoroutine(base.TrackingPlayer());
        }

        private void OnDisable()
        {
            //StopCoroutine(base.TrackingPlayer());
            StopAllCoroutines();
        }

        /// <summary>
        /// 20180418 SangBin : Item Hit By Player
        /// </summary>
        new private void OnHit()
        {
            base.OnHit();
        }

        /// <summary>
        /// 20180418 SangBin : Item function
        /// </summary>
        override public void ItemFunction()
        {
            //various function in here
        }

        new public void StartTrackingPlayer()
        {
            base.StartTrackingPlayer();
        }
    }
}
