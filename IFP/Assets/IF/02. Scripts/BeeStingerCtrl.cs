using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class BeeStingerCtrl : MonoBehaviour
    {
        /// <summary>
        /// 20180501 SangBin : Stinger Damage
        /// </summary>
        [HideInInspector]
        public double stingerDamage = 5.0d;

        /// <summary>
        /// 20180403 SangBin : Stinger Speed
        /// </summary>
        private float stingerSpeed = 50.0f;

        //-----------------------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(this.CheckBulletState());
        }

        private void OnDisable()
        {
            //총알 파괴 이펙트 작업 (추후)

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        /// <summary>
        /// 20180403 SangBin : Add force to this object
        /// </summary>
        void AddForceToBullet(Vector3 directionVetor)
        {
            this.gameObject.GetComponent<TrailRenderer>().enabled = true;
            /// <summary>
            /// 20180403 SangBin : Add force to this object on the world space 
            /// </summary>
            GetComponent<Rigidbody>().AddForce(directionVetor * stingerSpeed, ForceMode.Force);

            /// <summary>
            /// 20180403 SangBin : Add force to this object on the local space
            /// </summary>
            //GetComponent<Rigidbody>().AddRelativeForce(transform.forward * bulletSpeed);
        }

        /// <summary>
        /// 20180403 SangBin : Check bullet state
        /// </summary>
        IEnumerator CheckBulletState()
        {
            yield return new WaitForSeconds(3.0f);

            if (this.gameObject.activeSelf)
            {
                this.gameObject.GetComponent<TrailRenderer>().enabled = false;
                this.gameObject.SetActive(false);
            }
        }



        /// <summary>
        /// 20180403 SangBin : Bullet collision handler
        /// </summary>
        //void OnCollisionEnter(Collision collision)
        //{
        //    //Destroy(this.gameObject);
        //    GameObject flare = (GameObject)Instantiate(FlareEffect, collision.transform.position, Quaternion.identity);
        //    Destroy(flare, flare.GetComponent<ParticleSystem>().main.duration + 0.2f);
        //    //Destroy(collision.gameObject);
        //}

    }
}