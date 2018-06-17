using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace IFP
{
    public class ETS_LongRange : EnemyBaseClass
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
        /// 20180403 SangBin : Enemy Projectile Prefab2
        /// </summary>
        [SerializeField]
        private GameObject projectilePrefab;

        #endregion

        #region Fields : Enemy Statistic
        /// <summary>
        /// 20180502 SangBin :  Enemy Type01 Current Health Power
        /// </summary>
        private double currentHP = 100.0d;

        /// <summary>
        /// 20180403 SangBin : Enemy Current Health Power
        /// </summary>
        override protected double CurrentHealthPower
        {
            get
            {
                return currentHP;
            }

            set
            {
                currentHP = value;
            }
        }

        /// <summary>
        /// 20180502 SangBin :  Enemy Type01 Moving Speed
        /// </summary>
        private float movingSpeed = 5.0f;

        /// <summary>
        /// 20180418 SangBin :  Enemy Type01 Moving Speed
        /// </summary>
        protected override float MovingSpeed
        {
            get
            {
                return movingSpeed;
            }
        }


        /// <summary>
        /// 20180502 SangBin : Enemy Type01 Tag
        /// </summary>
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

        /// <summary>
        /// 20180612 SangBin : 
        /// </summary>
        public delegate void ETS_EventHandler();
        public event ETS_EventHandler ETS_Killed;

        //------------------------------------------------------------------------------------------------------------------------

        override protected void Awake()
        {
            base.animator = GetComponent<Animator>();
            base.Awake();
            CreateProjectileObjectPool();
            //anim = GetComponent<Animation>();
            tagName = gameObject.tag;
        }

        /// <summary>
        /// 20180403 SangBin : Damage to enemy (Message Driven Method)
        /// </summary>
        new private void OnDamaged(object[] parameters)
        {
            base.OnDamaged(parameters);

            if (!base.isDamaged)
                base.isDamaged = true;
        }



        /// <summary>
        /// 20180607 SangBin : 
        /// </summary>
        protected override void EnemyKilled()
        {
            //ETS_Killed();
            base.EnemyKilled();
            StartCoroutine(base.PushObjectPool());
            //if (DefenseStationCtrl.instance != null)
            //    StartCoroutine(base.PushObjectPool());
            //else
            //{
            //    Destroy(this, 4.0f);
            //}
            ETS_Killed();
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
                GameObject projectileObj = Instantiate(projectilePrefab, gameObject.transform.position + (gameObject.transform.up * 0.1f), gameObject.transform.rotation);
                projectileObj.name = this.gameObject.name + projectileObj.tag + "_" + i.ToString();
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
            //anim.CrossFade(IDLE, 0.3f);
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Tracing to Player Action
        /// </summary>
        protected override void ActionB()
        {
            base.ActionB();
            //anim.CrossFade(RUN, 0.3f);
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Attack to Player Action
        /// </summary>
        protected override void ActionC()
        {
            base.ActionC();
            //anim.CrossFade(ATTACK, 0.3f);
            StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoP));
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Tracing to DS Action
        /// </summary>
        protected override void ActionD()
        {
            //transform.LookAt(DefenseStationCtrl.instance.DefenseStationTR);
            base.ActionD();
            //anim.CrossFade(RUN, 0.3f);
        }

        /// <summary>
        /// 20180510 SangBin : Enemy Attack to DS Action
        /// </summary>
        protected override void ActionE()
        {
            //transform.LookAt(DefenseStationCtrl.instance.DefenseStationTR);
            base.ActionE();
            //anim.CrossFade(ATTACK, 0.3f);
            StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoDS));

        }

        protected override void OnTriggerStay(Collider collider)
        {
            base.OnTriggerStay(collider);

            if (!base.isDamaged)
                base.isDamaged = true;
        }

        protected override void OnParticleCollision(GameObject other)
        {
            base.OnParticleCollision(other);

            if (!base.isDamaged)
                base.isDamaged = true;
        }
    }
}