using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace IF
{
    public class BeeCtrl : EnemyBaseClass
    {
        #region Fields : Prefabs
        /// <summary>
        /// 20180502 SangBin : Enemy Bee Death Effect Prefab
        /// </summary>
        [SerializeField]
        private GameObject deathEffectPrefab;

        /// <summary>
        /// 20180502 SangBin : Enemy Bee Death Effect
        /// </summary>
        override protected GameObject DeathEffect
        {
            get
            {
                return deathEffectPrefab;
            }
        }

        /// <summary>
        /// 20180403 SangBin : Enemy Stinger Prefab
        /// </summary>
        [SerializeField]
        private GameObject stingerPrefab;

        #endregion

        #region Fields : Enemy Statistics
        /// <summary>
        /// 20180502 SangBin : Bee Current Health Power
        /// </summary>
        private double currentBeeHP = 100.0d;

        /// <summary>
        /// 20180403 SangBin : Enemy Current Health Power
        /// </summary>
        override protected double CurrentHealthPower
        {
            get
            {
                return currentBeeHP;
            }

            set
            {
                currentBeeHP = value;
            }
        }

        /// <summary>
        /// 20180502 SangBin : Bee Moving Speed
        /// </summary>
        private float beeMovingSpeed = 5.0f;

        /// <summary>
        /// 20180418 SangBin : Enemy Moving Speed
        /// </summary>
        protected override float MovingSpeed
        {
            get
            {
                return beeMovingSpeed;
            }
        }

        protected override string TagName
        {
            get
            {
                return "ENEMY_BEE";
            }
        }
        #endregion

        #region Fields : Bee Stinger
        /// <summary>
        /// 20180403 SangBin : Contraints of the number of Enemy Bullet
        /// </summary>
        private int maxStinger = 5;

        /// <summary>
        /// 20180403 SangBin : Stinger Object Pool List
        /// </summary>
        private List<GameObject> stingerObjectPool = new List<GameObject>();
        #endregion

        #region Fields : Sound Files
        /// <summary>
        /// 20180430 SangBin : stinger shoot Sound File
        /// </summary>
        [SerializeField]
        private AudioClip stingerSoundFile;
        #endregion

        //------------------------------------------------------------------------------------------------------------------------

        override protected void Awake()
        {
            base.Awake();
            CreateBulletObjectPool();
        }

        /// <summary>
        /// 20180403 SangBin : Damage to enemy (Message Driven Method)
        /// </summary>
        new private void OnDamaged(object[] parameters)
        {
            base.isDamaged = true;
            base.OnDamaged(parameters);
        }

        /// <summary>
        /// 20180430 SangBin : stinger Shooting
        /// </summary>
        IEnumerator StingerShooting(Vector3 directionVector_Normalized)
        {
            foreach (GameObject bulletObj in stingerObjectPool)
            {
                if (!bulletObj.activeSelf)
                {
                    bulletObj.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                    yield return null;
                    GameLogicManagement.GLM_Instance.SoundEffect(transform.position, stingerSoundFile);
                    bulletObj.SetActive(true);
                    bulletObj.SendMessage("AddForceToBullet", directionVector_Normalized, SendMessageOptions.DontRequireReceiver);
                    break;
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Create bullet OP
        /// </summary>
        void CreateBulletObjectPool()
        {
            for (int i = 0; i < maxStinger; i++)
            {
                GameObject bulletObj = Instantiate(stingerPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr);
                bulletObj.name = this.gameObject.name + "Bee_Stinger_" + i.ToString();
                bulletObj.SetActive(false);
                stingerObjectPool.Add(bulletObj);
            }
        }

        protected override void ActionC()
        {
            base.ActionC();
            StartCoroutine(StingerShooting(base.directionVector_NormalizedEtoP));
        }

        protected override void ActionD()
        {
            transform.LookAt(DefenseStationCtrl.DS_Instance.DefenseStationTR);
            base.ActionD();
        }

        protected override void ActionE()
        {
            transform.LookAt(DefenseStationCtrl.DS_Instance.DefenseStationTR);
            base.ActionE();
            StartCoroutine(StingerShooting(base.directionVector_NormalizedEtoDS));
        }
    }
}