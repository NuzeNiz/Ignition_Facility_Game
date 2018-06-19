using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    [RequireComponent(typeof(AudioSource))]
    public class CirclingEnemyCtrl : MonoBehaviour
    {
        /// <summary>
        /// 20180430 SangBin : Butterfly death Sound File
        /// </summary>
        [SerializeField]
        private AudioClip SoundFile;

        /// <summary>
        /// 20180430 SangBin : Butterfly death Effect Prefabs
        /// </summary>
        [SerializeField]
        private GameObject expEffectPrefab;
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            /// <summary>
            /// 20180427 SangBin :
            /// </summary>
            transform.RotateAround(DefenseStationCtrl.instance.DefenseStationTR.position, Vector3.up, 40.0f * Time.deltaTime);
        }

        /// <summary>
        /// 20180427 SangBin : When this object is Hit By Player
        /// </summary>
        void OnDamaged(object[] parameters)
        {
            GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
            GameManagement.instance.SoundEffect(transform.position, SoundFile);
            GameManagement.instance.ActivateItem(transform);
            Destroy(explosion, 2.0f);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 20180427 SangBin : test
        /// </summary>
        protected void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "WEAPON_TYPE02_FLAME")
            {
                GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
                GameManagement.instance.SoundEffect(transform.position, SoundFile);
                GameManagement.instance.ActivateItem(transform);
                Destroy(explosion, 2.0f);

                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 20180516 SangBin : When this object is Hit By Player
        /// </summary>
        private void OnParticleCollision(GameObject other)
        {
            if (other.gameObject.layer == 9)
            {
                GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
                GameManagement.instance.SoundEffect(transform.position, SoundFile);
                GameManagement.instance.ActivateItem(transform);
                Destroy(explosion, 2.0f);

                gameObject.SetActive(false);
            }
        }

    }
}