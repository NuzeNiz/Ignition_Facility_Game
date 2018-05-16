using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
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
        //----------------------------------------------------------

        private void Update()
        {
            transform.RotateAround(DefenseStationCtrl.instance.DefenseStationTR.position, Vector3.up, 40.0f * Time.deltaTime);
        }

        /// <summary>
        /// 20180427 SangBin : Being Hit By Player
        /// </summary>
        void OnDamaged(object[] parameters)
        {
            GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
            GameLogicManagement.instance.SoundEffect(transform.position, SoundFile);
            GameLogicManagement.instance.ActivateItem(transform);
            Destroy(explosion, 2.0f);

            gameObject.SetActive(false);
        }

        ///임시로 넣음
        protected void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "WEAPON_TYPE02_FLAME")
            {
                GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
                GameLogicManagement.instance.SoundEffect(transform.position, SoundFile);
                GameLogicManagement.instance.ActivateItem(transform);
                Destroy(explosion, 2.0f);

                gameObject.SetActive(false);
            }
        }

    }
}