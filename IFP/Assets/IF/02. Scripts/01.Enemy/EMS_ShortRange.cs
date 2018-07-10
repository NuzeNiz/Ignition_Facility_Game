using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace IFP
{
    public class EMS_ShortRange : EnemyBaseClass
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


        #endregion

        #region Fields : Enemy Statistic
        /// <summary>
        /// 20180502 SangBin :  Enemy Type01 Current Health Power
        /// </summary>
        private double currentHP = 800.0d;

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
        private float movingSpeed = 2.0f;

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

        protected override float EnemyAttackInterval
        {
            get
            {
                return 0.5f;
            }
        }


        protected override float AttackDistEtoDS
        {
            get
            {
                return 2.0f;
            }
        }

        protected override float AttackDistEtoP
        {
            get
            {
                return 1.0f;
            }
        }
        #endregion

        #region Fields : Sound Files
        /// <summary>
        /// 20180430 SangBin : Projectile shoot Sound File
        /// </summary>
        [SerializeField]
        private AudioClip shootingSoundFile;
        #endregion

        #region Fields : Enemy Skill
        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private enum EnemySkill { skill_01, skill_02, skill_03, skill_04 };

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private enum EnemySkillState { available, disavailable, cooling };

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private EnemySkillState skill_01_State = EnemySkillState.available;
        private EnemySkillState skill_02_State = EnemySkillState.available;
        private EnemySkillState skill_03_State = EnemySkillState.available;
        private EnemySkillState skill_04_State = EnemySkillState.available;
        #endregion

        public delegate void EnemyMiddleBoss_EventHandler();
        public static event EnemyMiddleBoss_EventHandler AbsorbAmmu;
        public static event EnemyMiddleBoss_EventHandler WaveBossDied;
        //------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        override protected void Awake()
        {
            base.animator = GetComponent<Animator>();
            base.Awake();
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
        /// 20180617 SangBin : 
        /// </summary>
        override protected void OnEnable()
        {

            base.OnEnable();
            if (IFP.TempStageManagement.instance.CurrentStageLevel != 10)
                StartCoroutine(TurnOffCanvas());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Destroy(gameObject);
        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        private IEnumerator TurnOffCanvas()
        {
            yield return new WaitForSeconds(2.5f);
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(false);
        }

        /// <summary>
        /// 20180607 SangBin : 
        /// </summary>
        protected override void EnemyKilled()
        {
            //ETS_Killed();
            base.EnemyKilled();
            if (IFP.TempStageManagement.instance.CurrentStageLevel == 10)
                WaveBossDied();
            else
            {
                AbsorbAmmu();
            }
            StartCoroutine(base.PushObjectPool());
            //if (DefenseStationCtrl.instance != null)
            //    StartCoroutine(base.PushObjectPool());
            //else
            //{
            //    Destroy(this, 4.0f);
            //}
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
            transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = true;
            base.ActionC();
            //anim.CrossFade(ATTACK, 0.3f);
            //transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(OffColiider());

            AvailableSkillCheck();
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
            transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = true;
            //transform.LookAt(DefenseStationCtrl.instance.DefenseStationTR);
            base.ActionE();
            //anim.CrossFade(ATTACK, 0.3f);
            //transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(OffColiider());
        }

        /// <summary>
        /// 20180510 SangBin : 
        /// </summary>
        protected override void OnTriggerStay(Collider collider)
        {
            base.OnTriggerStay(collider);

            if (!base.isDamaged)
                base.isDamaged = true;
        }

        /// <summary>
        /// 20180510 SangBin : 
        /// </summary>
        protected override void OnParticleCollision(GameObject other)
        {
            base.OnParticleCollision(other);

            if (!base.isDamaged)
                base.isDamaged = true;
        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        private IEnumerator OffColiider()
        {
            yield return new WaitForSeconds(0.3f);
            transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        void AvailableSkillCheck()
        {
            //double percentageofHP = (currentHP / base.maxHealthPower) * 100.0d ;
            double percentageofHP = currentHP / base.maxHealthPower;

            if (percentageofHP <= 0.7d)
            {
                if (skill_01_State == EnemySkillState.available)
                    UseSkill(EnemySkill.skill_01);
            }
        }

        /// <summary>s
        /// 20180530 SangBin :
        /// </summary>
        private void UseSkill(EnemySkill ek)
        {

            switch (ek)
            {
                case EnemySkill.skill_01:
                    //skill_01_State = EnemySkillState.cooling;
                    skill_01_State = EnemySkillState.disavailable;
                    //Thread thr1 = new Thread(new ThreadStart(ThreadBody01));
                    // thr1.Start();
                    StartCoroutine(UseSkill_01());
                    break;
            }

            //StartCoroutine(SkillCoolDown(ek));
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_01()
        {
            StopCoroutine(base.EnemyAction());

            animator.SetTrigger("IsSkill01");

            yield return new WaitForSeconds(2.5f);
            //transform.GetChild(3).gameObject.transform.position = DefenseStationCtrl.instance.transform.position + (Vector3.up * 2.0f);
            transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(TurnOffSkill());


            StartCoroutine(base.EnemyAction());

            //ENEMY_TYPE02_BOSS_SKILL
            yield break;
        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        private IEnumerator TurnOffSkill()
        {
            yield return new WaitForSeconds(5.0f);
            transform.GetChild(3).gameObject.SetActive(false);
        }
    }
}