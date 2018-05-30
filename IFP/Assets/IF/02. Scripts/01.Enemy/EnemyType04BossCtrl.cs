using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class EnemyType04BossCtrl : EnemyBaseClass
    {
        #region Fields : Prefabs
        /// <summary>
        /// 20180502 SangBin : Enemy Type04 Death Effect Prefab
        /// </summary>
        [SerializeField]
        private GameObject deathEffectPrefab;

        /// <summary>
        /// 20180502 SangBin : Enemy Type04 Death Effect
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
        //[SerializeField]
        //private GameObject projectilePrefab;

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private GameObject basicAttack_shot_Effect;
        #endregion

        #region Fields : Enemy Statistic
        /// <summary>
        /// 20180530 SangBin :  Enemy Type04 Current Health Power
        /// </summary>
        private double currentHP = 500.0d;

        /// <summary>
        /// 20180530 SangBin : Enemy Current Health Power
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
        /// 20180530 SangBin :  Enemy Type04 Moving Speed
        /// </summary>
        private float movingSpeed = 2.0f;

        /// <summary>
        /// 20180530 SangBin :  Enemy Type04 Moving Speed
        /// </summary>
        protected override float MovingSpeed
        {
            get
            {
                return movingSpeed;
            }
        }

        /// <summary>
        /// 20180530 SangBin : Enemy Type04 Tag
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

        #region Fields : Enemy Projectile
        /// <summary>
        /// 20180530 SangBin : Contraints of the number of Enemy Projectile
        /// </summary>
        private int maxProjectile = 5;

        /// <summary>
        /// 20180530 SangBin : Projectile Object Pool List
        /// </summary>
        private List<GameObject> projectileObjectPool = new List<GameObject>();
        #endregion

        #region Fields : Tracking(Enemy To Player)
        /// <summary>
        /// 20180530 SangBin : 
        /// </summary>
        protected override float TraceDistEtoP
        {
            get
            {
                return 20.0f;
            }
        }

        /// <summary>
        /// 20180530 SangBin : 
        /// </summary>
        protected override float AttackDistEtoP
        {
            get
            {
                return 5.0f;
            }
        }
        #endregion

        #region Fields : Sound Files
        /// <summary>
        /// 20180530 SangBin : Projectile shoot Sound File
        /// </summary>
        [SerializeField]
        private AudioClip shootingSoundFile;
        #endregion

        #region  Fields : Legacy Animation
        ///// <summary>
        ///// 20180530 SangBin : Legacy Animation
        ///// </summary>
        //Animation anim;

        ///// <summary>
        ///// 20180530 SangBin : Legacy Animation
        ///// </summary>
        //public const string IDLE = "free";
        //public const string RUN = "walk";
        //public const string BASIC_ATTACK = "attack2";
        ////public const string DAMAGED = "Anim_Damage";
        //public const string DEATH = "death";

        //public const string SKILL01 = "skill2"; // summon
        //public const string SKILL02 = "attack"; //teleport
        //public const string SKILL03 = "idle"; //shield
        //public const string SKILL04 = "skill"; // 
        #endregion

        #region Fields : Enemy Skill
        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private enum EnemySkill { skill_01, skill_02, skill_03, skill_04 };
        //스킬 중요도 1<2<3<4(most)

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private bool isWitchSkillUsed = false;

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private bool skill_01_State = false;
        private bool skill_02_State = false;
        private bool skill_03_State = false;
        private bool skill_04_State = false;
        #endregion
        //--------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.animator = GetComponent<Animator>();
            base.Awake();
            tagName = gameObject.tag;
            //CreateProjectileObjectPool();
            transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
        }

        override protected void OnEnable()
        {
            base.OnEnable();
            base.isDamaged = true;
        }

        ///// <summary>
        ///// 20180530 SangBin : Create bullet OP
        ///// </summary>
        //void CreateProjectileObjectPool()
        //{
        //    for (int i = 0; i < maxProjectile; i++)
        //    {
        //        GameObject projectileObj = Instantiate(projectilePrefab, gameObject.transform.position + (gameObject.transform.up * 1.0f), gameObject.transform.rotation, GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr);
        //        projectileObj.name = this.gameObject.name + projectileObj.tag + "_" + i.ToString();
        //        projectileObj.SetActive(false);
        //        projectileObjectPool.Add(projectileObj);
        //    }
        //}

        ///// <summary>
        ///// 20180530 SangBin : Projectile Shooting
        ///// </summary>
        //IEnumerator ProjectileShooting(Vector3 directionVector_Normalized)
        //{
        //    foreach (GameObject projectileObj in projectileObjectPool)
        //    {
        //        if (!projectileObj.activeSelf)
        //        {
        //            projectileObj.transform.SetPositionAndRotation(this.gameObject.transform.position + (gameObject.transform.up * 1.0f), this.gameObject.transform.rotation);
        //            yield return null;
        //            GameManagement.instance.SoundEffect(transform.position, shootingSoundFile);
        //            projectileObj.SetActive(true);
        //            //projectileObj.SendMessage("AddForceToProjectile", directionVector_Normalized, SendMessageOptions.DontRequireReceiver);
        //            projectileObj.GetComponent<EnemyProjectileType01Ctrl>().AddForceToProjectile(directionVector_Normalized);
        //            break;
        //        }
        //    }
        //}

        /// <summary>
        /// 20180530 SangBin : 
        /// </summary>
        private IEnumerator ProjectileShot(GameObject eff)
        {
            GameObject effect = Instantiate(eff, transform.position + (transform.up * 1.0f), transform.rotation);
            effect.transform.LookAt(PlayerCtrl.instance.PlayerTr);

            yield return new WaitForSeconds(3.0f);

            if (effect.activeSelf)
                Destroy(effect, 1.0f);

        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        protected override void ActionB()
        {
            base.animator.SetBool("IsBasicAttack", false);
            base.animator.SetBool("IsSkill01", false);
            base.animator.SetBool("IsSkill02", false);
            base.animator.SetBool("IsSkill03", false);
            base.animator.SetBool("IsSkill04", false);

            Vector3 tempV = Vector3.zero;
            tempV.x = PlayerCtrl.instance.PlayerTr.position.x;
            tempV.z = PlayerCtrl.instance.PlayerTr.position.z;
            tempV.y = transform.position.y;

            transform.LookAt(tempV);

            //transform.LookAt(PlayerCtrl.instance.PlayerTr);
            GetComponent<Rigidbody>().AddForce(base.directionVector_NormalizedEtoP * MovingSpeed, ForceMode.Force);
            base.animator.SetBool("IsTrace", true); 
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        protected override void ActionC()
        {
            Vector3 tempV = Vector3.zero;
            tempV.x = PlayerCtrl.instance.PlayerTr.position.x;
            tempV.z = PlayerCtrl.instance.PlayerTr.position.z;
            tempV.y = transform.position.y;

            transform.LookAt(tempV);

            //transform.LookAt(PlayerCtrl.instance.PlayerTr);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            AvailableSkillCheck();

            if (!isWitchSkillUsed)
            {
                //StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoP));
                StartCoroutine(ProjectileShot(basicAttack_shot_Effect));
                animator.SetBool("IsBasicAttack", true);
            }

        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        void AvailableSkillCheck()
        {
            double percentageofHP = currentHP / base.maxHealthPower;

            isWitchSkillUsed = false;

            if(percentageofHP<=80.0d)
            {
                if (percentageofHP <= 20.0d)
                {
                    if (skill_04_State)
                        UseSkill(ref skill_04_State, EnemySkill.skill_04);

                    else if (skill_03_State)
                        UseSkill(ref skill_03_State, EnemySkill.skill_03);

                    else if (skill_02_State)
                        UseSkill(ref skill_02_State, EnemySkill.skill_02);

                    else if (skill_01_State)
                        UseSkill(ref skill_01_State, EnemySkill.skill_01);
                }
                else if (percentageofHP <= 40.0d)
                {
                    if (skill_03_State)
                        UseSkill(ref skill_03_State, EnemySkill.skill_03);

                    else if (skill_02_State)
                        UseSkill(ref skill_02_State, EnemySkill.skill_02);

                    else if (skill_01_State)
                        UseSkill(ref skill_01_State, EnemySkill.skill_01);
                }
                else if (percentageofHP <= 60.0d)
                {
                    if (skill_02_State)
                        UseSkill(ref skill_02_State, EnemySkill.skill_02);

                    else if (skill_01_State)
                        UseSkill(ref skill_01_State, EnemySkill.skill_01);
                }
                else
                {
                    if (skill_01_State)
                        UseSkill(ref skill_01_State, EnemySkill.skill_01);
                }
            }
        }


        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_01()
        {
            animator.SetBool("IsSkill01", true);
            isWitchSkillUsed = true;


            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_02()
        {
            animator.SetBool("IsSkill02", true);
            isWitchSkillUsed = true;


            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_03()
        {
            animator.SetBool("IsSkill03", true);
            isWitchSkillUsed = true;


            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_04()
        {
            animator.SetBool("IsSkill04", true);
            isWitchSkillUsed = true;


            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private void UseSkill(ref bool skill_State, EnemySkill ek)
        {
            switch (ek)
            {
                case EnemySkill.skill_01:
                    StartCoroutine(UseSkill_01());
                    break;

                case EnemySkill.skill_02:
                    StartCoroutine(UseSkill_02());
                    break;

                case EnemySkill.skill_03:
                    StartCoroutine(UseSkill_03());
                    break;

                case EnemySkill.skill_04:
                    StartCoroutine(UseSkill_04());
                    break;
            }

            skill_State = false;
            StartCoroutine(SkillCoolDown(ek));
        }


        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator SkillCoolDown(EnemySkill ek)
        {
            float elapsedTime = 0.0f, coolTime = 0.0f;

            switch (ek)
            {
                case EnemySkill.skill_01:
                    coolTime = 4.0f;
                    break;

                case EnemySkill.skill_02:
                    coolTime = 6.0f;
                    break;

                case EnemySkill.skill_03:
                    coolTime = 8.0f;
                    break;

                case EnemySkill.skill_04:
                    coolTime = 10.0f;
                    break;
            }



            while (elapsedTime < coolTime)
            {
                elapsedTime += Time.deltaTime;
            }

            EnableSkill(ek);

            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private void EnableSkill(EnemySkill ek)
        {
            switch (ek)
            {
                case EnemySkill.skill_01:
                    skill_01_State = true;
                    break;

                case EnemySkill.skill_02:
                    skill_02_State = true;
                    break;

                case EnemySkill.skill_03:
                    skill_03_State = true;
                    break;

                case EnemySkill.skill_04:
                    skill_04_State = true;
                    break;
            }
        }
    }
}