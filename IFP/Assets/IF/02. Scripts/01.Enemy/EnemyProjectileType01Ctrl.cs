using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class EnemyProjectileType01Ctrl : MonoBehaviour
    {
        /// <summary>
        /// 20180501 SangBin : Projectile Damage
        /// </summary>
        //[HideInInspector]
        //public double projectileDamage = 2.0d;

        /// <summary>
        /// 20180403 SangBin : Projectile Speed
        /// </summary>
        private float projectileSpeed = 50.0f;

        //-----------------------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(this.CheckProjectileState());
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
        public void AddForceToProjectile(Vector3 directionVetor)
        {
            this.gameObject.GetComponent<TrailRenderer>().enabled = true;
            /// <summary>
            /// 20180403 SangBin : Add force to this object on the world space 
            /// </summary>
            GetComponent<Rigidbody>().AddForce(directionVetor * projectileSpeed, ForceMode.Force);

            /// <summary>
            /// 20180403 SangBin : Add force to this object on the local space
            /// </summary>
            //GetComponent<Rigidbody>().AddRelativeForce(transform.forward * bulletSpeed);
        }

        /// <summary>
        /// 20180403 SangBin : Check Projectile state
        /// </summary>
        IEnumerator CheckProjectileState()
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