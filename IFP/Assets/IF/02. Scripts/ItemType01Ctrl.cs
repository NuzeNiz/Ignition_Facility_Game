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
        /// 20180418 SangBin : Item Hit Sound File Type 01
        /// </summary>
        public AudioClip itemHitSoundFile_type01;

        /// <summary>
        /// 20180418 SangBin : Normalized Vector From This Item To Player 
        /// </summary>
        private Vector3 directionVector_Normalized;

        /// <summary>
        /// 20180418 SangBin : Normalized Direction Vector Property
        /// </summary>
        public Vector3 DirectionVector_Normalized { get { return directionVector_Normalized; } set { directionVector_Normalized = value; } }

        /// <summary>
        /// 20180418 SangBin : Item Moving Speed
        /// </summary>
        private float MovingSpeed = 50.0f;

        /// <summary>
        /// 20180418 SangBin : Item Collision Effect Prefab Type 01
        /// </summary>
        public GameObject itemCollEffectPrefabe_type01;

        //---------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            base.itemType = 1;
            base.itemHitEffectPrefab = this.itemHitEffectPrefab_type01;
            base.itemHitSoundFile = this.itemHitSoundFile_type01;
            base.itemCollEffectPrefab = this.itemCollEffectPrefabe_type01;
        }

        private void Start()
        {
            StartCoroutine(TrackingPlayer());
        }


        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            StopCoroutine(TrackingPlayer());
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

        IEnumerator TrackingPlayer()
        {
            directionVector_Normalized = Vector3.Normalize(PlayerCtrl.PlayerInstance.PlayerTr.position - transform.position);
            transform.LookAt(PlayerCtrl.PlayerInstance.PlayerTr);
            GetComponent<Rigidbody>().AddForce(directionVector_Normalized * MovingSpeed, ForceMode.Force);
            yield return new WaitForSeconds(0.1f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameLogicManagement.GM_Instance.SoundEffect(transform.position, itemHitSoundFile);
                GameObject itemCollEffect = (GameObject)Instantiate(itemCollEffectPrefabe_type01, gameObject.transform.position, Quaternion.identity);
                Destroy(itemCollEffect, 2.0f);

                // Add some codes this item should be enter in UI slot 

                // Add some initialization in objectpool with deactive

                gameObject.SetActive(false);
            }
        }
    }
}
