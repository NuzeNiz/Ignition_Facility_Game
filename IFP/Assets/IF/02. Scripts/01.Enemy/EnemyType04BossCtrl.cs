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
        [SerializeField]
        private GameObject projectilePrefab;
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
        private float movingSpeed = 5.0f;

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
                return 4.0f;
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
        #endregion
        //--------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            tagName = gameObject.tag;
            CreateProjectileObjectPool();
        }

        override protected void OnEnable()
        {
            base.isDamaged = true;
            base.OnEnable();
        }

        /// <summary>
        /// 20180530 SangBin : Create bullet OP
        /// </summary>
        void CreateProjectileObjectPool()
        {
            for (int i = 0; i < maxProjectile; i++)
            {
                GameObject projectileObj = Instantiate(projectilePrefab, gameObject.transform.position + (gameObject.transform.up * 0.1f), gameObject.transform.rotation, GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr);
                projectileObj.name = this.gameObject.name + projectileObj.tag + "_" + i.ToString();
                projectileObj.SetActive(false);
                projectileObjectPool.Add(projectileObj);
            }
        }

        /// <summary>
        /// 20180530 SangBin : Projectile Shooting
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
        /// 20180530 SangBin :
        /// </summary>
        protected override void ActionB()
        {
            base.animator.SetBool("IsAttack", false);
            base.animator.SetBool("IsSkill01", false);
            base.animator.SetBool("IsSkill02", false);
            base.animator.SetBool("IsSkill03", false);
            base.animator.SetBool("IsSkill04", false);

            transform.LookAt(PlayerCtrl.instance.PlayerTr);
            GetComponent<Rigidbody>().AddForce(base.directionVector_NormalizedEtoP * MovingSpeed, ForceMode.Force);
            base.animator.SetBool("IsTrace", true); 
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        protected override void ActionC()
        {
            transform.LookAt(PlayerCtrl.instance.PlayerTr);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            AvailableSkillCheck();

            if (!isWitchSkillUsed)
                StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoP));

        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        void AvailableSkillCheck()
        {
            double percentageofHP = currentHP / base.maxHealthPower;


            //if (currentHP <= 100.0d && currentHP > 70.0d)
            //{

            //}

            isWitchSkillUsed = false;

            //bool skill04_State = false;


            if(percentageofHP<=70.0d)
            {
                if (percentageofHP <= 20.0d)
                {
                    if (skill_03_State)
                        StartCoroutine(UseSkill_03());
                    //else if(skill_02_State)

                }
                else if (percentageofHP <= 50.0d)
                {

                }
            }
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator SkillCoolDown(bool skill)
        {

            //float elapsedTime = 0.0f;

            ////while (elapsedTime < )
            //{
            //   // yield return null;
            //}

            yield return null;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_01()
        {


            skill_01_State = false;
            StartCoroutine(SkillCoolDown(skill_01_State));
            yield return null;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_02()
        {


            skill_02_State = false;
            StartCoroutine(SkillCoolDown(skill_02_State));
            yield return null;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_03()
        {


            skill_03_State = false;
            StartCoroutine(SkillCoolDown(skill_03_State));
            yield return null;
        }

        private void UseSkill(ref bool skill_State, EnemySkill ek)
        {

            skill_State = false;
            StartCoroutine(SkillCoolDown(skill_State));
        }
    }
}