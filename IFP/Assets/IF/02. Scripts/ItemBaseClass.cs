using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    [RequireComponent(typeof(AudioSource))]
    abstract public class ItemBaseClass : MonoBehaviour
    {
        /// <summary>
        /// 20180418 SangBin : Item Type Enum
        /// </summary>
        [HideInInspector]
        public enum ItemTypeEnum { cinnamon, itemType02, itemType03, itemType04 };

        /// <summary>
        /// 20180418 SangBin : Item Type
        /// </summary>
        abstract public ItemTypeEnum ItemType { get; }

        /// <summary>
        /// 20180403 SangBin : Item Collision Sound File
        /// </summary>
        abstract protected AudioClip ItemCollSoundFile { get; }

        /// <summary>
        /// 20180418 SangBin : Item Collision Effect Prefab
        /// </summary>
        abstract protected GameObject ItemCollEffectPrefab { get; }

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

        //-----------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 20180418 SangBin : Item function
        /// </summary>
        abstract public void ItemFunction();

        /// <summary>
        /// 20180418 SangBin : Item Hit By Player
        /// </summary>
        protected void OnHit()
        {
            tag = "Untagged";
            GameLogicManagement.GLM_Instance.SoundEffect(transform.position, ItemCollSoundFile);
            GameObject itemHitEffect = (GameObject)Instantiate(ItemCollEffectPrefab, transform.position, Quaternion.identity);
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            Destroy(itemHitEffect, 2.0f);
            //gameObject.SetActive(false);

            ObtainingItem();
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameLogicManagement.GLM_Instance.SoundEffect(transform.position, ItemCollSoundFile);
                GameObject itemCollEffect = (GameObject)Instantiate(ItemCollEffectPrefab, transform.position, Quaternion.identity);
                Destroy(itemCollEffect, 2.0f);

                ObtainingItem();

                // Add some initialization in objectpool with deactive

                //gameObject.SetActive(false);
            }
        }

        protected IEnumerator TrackingPlayer()
        {
            directionVector_Normalized = Vector3.Normalize(PlayerCtrl.PlayerInstance.PlayerTr.position - transform.position);
            transform.LookAt(PlayerCtrl.PlayerInstance.PlayerTr);
            GetComponent<Rigidbody>().AddForce(directionVector_Normalized * MovingSpeed, ForceMode.Force);
            yield return new WaitForSeconds(0.1f);
        }

        protected void StartTrackingPlayer()
        {
            StartCoroutine(TrackingPlayer());
        }

        void ObtainingItem()
        {
            //tag = "Untagged";
            StopCoroutine(TrackingPlayer());
            //GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            transform.parent = PlayerCtrl.PlayerInstance.PlayerTr;
            transform.position = Vector3.zero + Vector3.forward;

            // Add some codes this item should be enter in UI slot 
        }
    }
}
