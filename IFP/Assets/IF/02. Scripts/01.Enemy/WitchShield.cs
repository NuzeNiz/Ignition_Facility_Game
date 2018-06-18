using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class WitchShield : MonoBehaviour
    {

        private double currentHealthPower;
        private double maxHealthPower;

        /// <summary>
        /// 20180514 SeongJun : HP Display in Child
        /// </summary>
        [SerializeField]
        private HPDisplayer healthDisplay;

        // Use this for initialization
        void Start()
        {
            currentHealthPower = 400.0d;
            maxHealthPower = currentHealthPower;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 20180514 SeongJun : Use HP_Displayer
        /// 20180530 SangBin : add max hp
        /// </summary>
        private void SetHPHuD()
        {
            if (healthDisplay != null)
            {
                healthDisplay.SetHP(currentHealthPower, maxHealthPower);
            }
        }

        protected void OnDamaged(object[] parameters)
        {
            currentHealthPower -= (double)parameters[0];

            SetHPHuD();

            if (currentHealthPower <= 0.0d)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 20180516 SangBin : 
        /// </summary>
        private void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "WEAPON_TYPE02_FLAME")
            {
                currentHealthPower -= BalanceManagement.instance.CalcPlayerStrkingPower(gameObject.tag, WeaponCtrl.instance.CurrentWeaponType);
            }

            SetHPHuD();

            //임시로 넣음
            if (currentHealthPower <= 0.0d)
            {
                gameObject.SetActive(false);
            }

        }

        /// <summary>
        /// 20180516 SangBin : 
        /// </summary>
        private void OnParticleCollision(GameObject other)
        {
            //if (collision.gameObject.tag == "WEAPON_TYPE03_PROJECTILE")
            {
                currentHealthPower -= BalanceManagement.instance.CalcPlayerStrkingPower(gameObject.tag, WeaponCtrl.instance.CurrentWeaponType); //damage per frame
            }

            SetHPHuD();

            //임시로 넣음
            if (currentHealthPower <= 0.0d)
            {
                gameObject.SetActive(false);
            }
        }
    }
}