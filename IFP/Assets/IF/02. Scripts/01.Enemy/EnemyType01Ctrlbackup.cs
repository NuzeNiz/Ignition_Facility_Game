using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace IF
{
    public class EnemyType01Ctrlbackup : EnemyBaseClass
    {
        #region Fields : Prefabs
        /// <summary>
        /// 20180502 SangBin : Enemy Type01 Death Effect Prefab
        /// </summary>
        [SerializeField]
        private GameObject deathEffectPrefab;

        /// <summary>
        /// 20180502 SangBin : Enemy Type01 Death Effect
        /// </summary>
        override protected GameObject DeathEffect
        {
            get
            {
                return deathEffectPrefab;
            }
        }

        /// <summary>
        /// 20180403 SangBin : Enemy Projectile Prefab
        /// </summary>
        [SerializeField]
        private GameObject projectilePrefab;

        #endregion

        #region Fields : Enemy Statistic
        /// <summary>
        /// 20180502 SangBin :  Enemy Type01 Current Health Power
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
        /// 20180502 SangBin :  Enemy Type01 Moving Speed
        /// </summary>
        private float e1MovingSpeed = 5.0f;

        /// <summary>
        /// 20180418 SangBin :  Enemy Type01 Moving Speed
        /// </summary>
        protected override float MovingSpeed
        {
            get
            {
                return e1MovingSpeed;
            }
        }

        private string tagName;

        protected override string TagName
        {
            get
            {
                return tagName;
            }
        }
        #endregion

        #region Fields : Enemy Type01 Projectile
        /// <summary>
        /// 20180403 SangBin : Contraints of the number of Enemy Projectile
        /// </summary>
        private int maxStinger = 5;

        /// <summary>
        /// 20180403 SangBin : Projectile Object Pool List
        /// </summary>
        private List<GameObject> projectileObjectPool = new List<GameObject>();
        #endregion

        #region Fields : Sound Files
        /// <summary>
        /// 20180430 SangBin : Projectile shoot Sound File
        /// </summary>
        [SerializeField]
        private AudioClip shootingSoundFile;
        #endregion

        #region  Fields : Test Animation
        /// <summary>
        /// 20180509 SangBin : Test Animation
        /// </summary>
        Animation anim;

        /// <summary>
        /// 20180509 SangBin : Test Animation
        /// </summary>
        public const string IDLE = "Anim_Idle";
        public const string RUN = "Anim_Run";
        public const string ATTACK = "Anim_Attack";
        public const string DAMAGE = "Anim_Damage";
        public const string DEATH = "Anim_Death";
        #endregion
        //------------------------------------------------------------------------------------------------------------------------

        override protected void Awake()
        {
            base.Awake();
            CreateProjectileObjectPool();
            anim = GetComponent<Animation>();
            tagName = gameObject.tag;
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
        /// 20180430 SangBin : Projectile Shooting
        /// </summary>
        IEnumerator ProjectileShooting(Vector3 directionVector_Normalized)
        {
            foreach (GameObject projectileObj in projectileObjectPool)
            {
                if (!projectileObj.activeSelf)
                {
                    projectileObj.transform.SetPositionAndRotation(this.gameObject.transform.position + (gameObject.transform.up * 0.1f), this.gameObject.transform.rotation);
                    yield return null;
                    GameManagement.instance.SoundEffect(transform.position, shootingSoundFile);
                    projectileObj.SetActive(true);
                    //projectileObj.SendMessage("AddForceToProjectile", directionVector_Normalized, SendMessageOptions.DontRequireReceiver);
                    projectileObj.GetComponent<EnemyProjectileType01Ctrl>().AddForceToProjectile(directionVector_Normalized);
                    break;
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Create bullet OP
        /// </summary>
        void CreateProjectileObjectPool()
        {
            for (int i = 0; i < maxStinger; i++)
            {
                GameObject projectileObj = Instantiate(projectilePrefab, gameObject.transform.position + (gameObject.transform.up * 0.1f), gameObject.transform.rotation, GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr);
                projectileObj.name = this.gameObject.name + gameObject.tag + "_" + i.ToString();
                projectileObj.SetActive(false);
                projectileObjectPool.Add(projectileObj);
            }
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Idle Action
        /// </summary>
        protected override void ActionA()
        {
            base.ActionA();
            anim.CrossFade(IDLE, 0.3f);
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Tracing to Player Action
        /// </summary>
        protected override void ActionB()
        {
            base.ActionB();
            anim.CrossFade(RUN, 0.3f);
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Attack to Player Action
        /// </summary>
        protected override void ActionC()
        {
            base.ActionC();
            anim.CrossFade(ATTACK, 0.3f);
            StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoP));
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Tracing to DS Action
        /// </summary>
        protected override void ActionD()
        {
            transform.LookAt(DefenseStationCtrl.instance.DefenseStationTR);
            base.ActionD();
            anim.CrossFade(RUN, 0.3f);
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Attack to DS Action
        /// </summary>
        protected override void ActionE()
        {
            transform.LookAt(DefenseStationCtrl.instance.DefenseStationTR);
            base.ActionE();
            anim.CrossFade(ATTACK, 0.3f);
            StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoDS));
        }
    }
}