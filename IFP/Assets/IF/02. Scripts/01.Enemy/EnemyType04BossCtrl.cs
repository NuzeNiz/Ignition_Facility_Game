using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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
        private double currentHP = 200.0d;

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

        protected override float EnemyAttackInterval
        {
            get
            {
                return 2.5f;
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
        private enum EnemySkillState { available, disavailable, cooling };


        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private bool isWitchSkillUsed = false;

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private EnemySkillState skill_01_State = EnemySkillState.available;
        private EnemySkillState skill_02_State = EnemySkillState.available;
        private EnemySkillState skill_03_State = EnemySkillState.available;
        private EnemySkillState skill_04_State = EnemySkillState.available;
        #endregion

        #region Fields : Summon
        /// <summary>
        /// 20180403 SangBin : Enemy  Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type01;

        /// <summary>
        /// 20180501 SangBin : Enemy  Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type02;

        /// <summary>
        /// 20180501 SangBin : Enemy  Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type03;

        private int maxSummon = 5;
        private int summonCount = 0;

        private List<GameObject> enemyPrefabList = new List<GameObject>();

        #endregion
        //--------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.animator = GetComponent<Animator>();
            base.Awake();
            tagName = gameObject.tag;
            //CreateProjectileObjectPool();
            //transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;

            CreateEnemyObjectPool();
            //enemyPrefabList.Add(enemyPrefab_type01);
            //enemyPrefabList.Add(enemyPrefab_type02);
            //enemyPrefabList.Add(enemyPrefab_type03);
        }

        override protected void OnEnable()
        {
            base.OnEnable();
            base.isDamaged = true;
            ETS_LongRange.ETS_Killed += this.ETS_Killed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ETS_LongRange.ETS_Killed -= this.ETS_Killed;
            enemyPrefabList.Clear();
        }

        private void ETS_Killed()
        {
            summonCount--;
        }

        /// <summary>
        /// 20180607 SangBin : 
        /// </summary>
        protected override void EnemyKilled()
        {
            base.EnemyKilled();

            StartCoroutine(GameClear());
        }

        /// <summary>
        /// 20180607 SangBin : 
        /// </summary>
        private IEnumerator GameClear()
        {
            yield return new WaitForSeconds(4.0f);
            GameManagement.instance.GameClear();
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

            if (!isWitchSkillUsed)
            {
                animator.SetBool("IsBasicAttack", true);
                StartCoroutine(ProjectileShot(basicAttack_shot_Effect));
                AvailableSkillCheck();
            }





            //if (!isWitchSkillUsed)
            //{
            //    animator.SetBool("IsBasicAttack", true);
            //    StartCoroutine(ProjectileShot(basicAttack_shot_Effect));

            //    AvailableSkillCheck();
            //}

            //if (!isWitchSkillUsed)
            //{
            //    //StartCoroutine(ProjectileShooting(base.directionVector_NormalizedEtoP));
            //    StartCoroutine(ProjectileShot(basicAttack_shot_Effect));
            //    animator.SetBool("IsBasicAttack", true);
            //}

        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        void AvailableSkillCheck()
        {
            double percentageofHP = (currentHP / base.maxHealthPower) * 100.0d ;

            if(percentageofHP<=80.0d)
            {
                if (percentageofHP <= 20.0d)
                {
                    if (skill_04_State == EnemySkillState.available)
                        //UseSkill(ref skill_04_State, EnemySkill.skill_04);
                        UseSkill(EnemySkill.skill_04);

                    else if (skill_03_State == EnemySkillState.available)
                        //UseSkill(ref skill_03_State, EnemySkill.skill_03);
                        UseSkill(EnemySkill.skill_03);

                    else if (skill_02_State == EnemySkillState.available)
                        //UseSkill(ref skill_02_State, EnemySkill.skill_02);
                        UseSkill(EnemySkill.skill_02);

                    else if (skill_01_State == EnemySkillState.available)
                        //UseSkill(ref skill_01_State, EnemySkill.skill_01);
                        UseSkill(EnemySkill.skill_01);
                }
                else if (percentageofHP <= 40.0d)
                {
                    if (skill_03_State == EnemySkillState.available)
                        //UseSkill(ref skill_03_State, EnemySkill.skill_03);
                        UseSkill(EnemySkill.skill_03);

                    else if (skill_02_State == EnemySkillState.available)
                        //UseSkill(ref skill_02_State, EnemySkill.skill_02);
                        UseSkill(EnemySkill.skill_02);

                    else if (skill_01_State == EnemySkillState.available)
                        //UseSkill(ref skill_01_State, EnemySkill.skill_01);
                        UseSkill(EnemySkill.skill_01);
                }
                else if (percentageofHP <= 60.0d)
                {
                    if (skill_02_State == EnemySkillState.available)
                        //UseSkill(ref skill_02_State, EnemySkill.skill_02);
                        UseSkill(EnemySkill.skill_02);

                    else if (skill_01_State == EnemySkillState.available)
                        //UseSkill(ref skill_01_State, EnemySkill.skill_01);
                        UseSkill(EnemySkill.skill_01);
                }
                else
                {
                    if (skill_01_State == EnemySkillState.available)
                        //UseSkill(ref skill_01_State, EnemySkill.skill_01);
                        UseSkill(EnemySkill.skill_01);
                }
            }
        }



        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        //private IEnumerator UseSkill_01(EnemySkill ek)
        //{
        //    StopCoroutine(base.EnemyAction());
        //    animator.SetTrigger("IsSkill01");
        //    animator.SetBool("IsBasicAttack", false);
        //    animator.SetBool("IsSkill01", true);
        //    Thread.Sleep(1200);
        //    yield return new WaitForSeconds(1.1f);
        //    isWitchSkillUsed = false;
        //    Invoke("TempDelay", 1.1f);
        //    animator.SetBool("IsSkill01", false);

        //    StartCoroutine(base.EnemyAction());
        //    animator.SetBool("IsBasicAttack", true);
        //    StartCoroutine(SkillCoolDown(ek));
        //    StartCoroutine(base.EnemyAction());

        //    yield break;
        //}

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_01(EnemySkill ek)
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill01");

            yield return new WaitForSeconds(1.1f);
            StartCoroutine(WitchSkill01());
            isWitchSkillUsed = false;


            StartCoroutine(base.EnemyAction());

            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_02(EnemySkill ek)
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill02");

            yield return new WaitForSeconds(1.1f);

            isWitchSkillUsed = false;

            StartCoroutine(base.EnemyAction());


            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_03(EnemySkill ek)
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill03");

            yield return new WaitForSeconds(4.1f);

            isWitchSkillUsed = false;

            StartCoroutine(base.EnemyAction());

            yield break;
        }

        /// <summary>
        /// 20180530 SangBin :
        /// </summary>
        private IEnumerator UseSkill_04(EnemySkill ek)
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill04");

            yield return new WaitForSeconds(1.1f);

            isWitchSkillUsed = false;

            StartCoroutine(base.EnemyAction());

            yield break;
        }

        /// <summary>s
        /// 20180530 SangBin : test
        /// </summary>
        private void ThreadBody01()
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill01");
            Thread.Sleep(1100);
            isWitchSkillUsed = false;
            StartCoroutine(base.EnemyAction());
        }

        /// <summary>s
        /// 20180530 SangBin : test
        /// </summary>
        private void ThreadBody02()
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill02");
            Thread.Sleep(1100);
            isWitchSkillUsed = false;
            StartCoroutine(base.EnemyAction());
        }

        /// <summary>s
        /// 20180530 SangBin : test
        /// </summary>
        private void ThreadBody03()
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill03");
            Thread.Sleep(4100);
            isWitchSkillUsed = false;
            StartCoroutine(base.EnemyAction());
        }

        /// <summary>s
        /// 20180530 SangBin : test
        /// </summary>
        private void ThreadBody04()
        {
            StopCoroutine(base.EnemyAction());
            animator.SetTrigger("IsSkill04");
            Thread.Sleep(1100);
            isWitchSkillUsed = false;
            StartCoroutine(base.EnemyAction());
        }

        private void TempDelay() { }

        /// <summary>s
        /// 20180530 SangBin :
        /// </summary>
        private void UseSkill(EnemySkill ek)
        //private void UseSkill(ref EnemySkillState skill_State, EnemySkill ek)
        {

            isWitchSkillUsed = true;
            //skill_State = EnemySkillState.cooling;

            switch (ek)
            {
                case EnemySkill.skill_01:
                    //skill_01_State = EnemySkillState.cooling;
                    skill_01_State = EnemySkillState.disavailable;
                    //Thread thr1 = new Thread(new ThreadStart(ThreadBody01));
                   // thr1.Start();
                    StartCoroutine(UseSkill_01(ek));
                    break;

                case EnemySkill.skill_02:
                    //skill_02_State = EnemySkillState.cooling;
                    skill_02_State = EnemySkillState.disavailable;
                    //Thread thr2 = new Thread(new ThreadStart(ThreadBody02));
                    //thr2.Start();
                    StartCoroutine(UseSkill_02(ek));
                    break;

                case EnemySkill.skill_03:
                    //skill_03_State = EnemySkillState.cooling;
                    skill_03_State = EnemySkillState.disavailable;
                    //Thread thr3 = new Thread(new ThreadStart(ThreadBody03));
                    //thr3.Start();
                    StartCoroutine(UseSkill_03(ek));
                    break;

                case EnemySkill.skill_04:
                    //skill_04_State = EnemySkillState.cooling;
                    skill_04_State = EnemySkillState.disavailable;
                    //Thread thr4 = new Thread(new ThreadStart(ThreadBody04));
                    //thr4.Start();
                    StartCoroutine(UseSkill_04(ek));
                    break;
            }

            //skill_State = EnemySkillState.cooling;
            //StartCoroutine(SkillCoolDown(ek));
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
                    coolTime = 14.0f;
                    break;

                case EnemySkill.skill_02:
                    coolTime = 16.0f;
                    break;

                case EnemySkill.skill_03:
                    coolTime = 18.0f;
                    break;

                case EnemySkill.skill_04:
                    coolTime = 20.0f;
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
                    skill_01_State = EnemySkillState.available;
                    break;

                case EnemySkill.skill_02:
                    skill_02_State = EnemySkillState.available;
                    break;

                case EnemySkill.skill_03:
                    skill_03_State = EnemySkillState.available;
                    break;

                case EnemySkill.skill_04:
                    skill_04_State = EnemySkillState.available;
                    break;
            }
        }

        void CreateEnemyObjectPool()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject enemy_type01 = (GameObject)Instantiate(enemyPrefab_type01);
                enemy_type01.name = "Enemy_type01_" + i.ToString();
                enemy_type01.SetActive(false);

                GameObject enemy_type02 = (GameObject)Instantiate(enemyPrefab_type02);
                enemy_type02.name = "Enemy_type01_" + i.ToString();
                enemy_type02.SetActive(false);

                GameObject enemy_type03 = (GameObject)Instantiate(enemyPrefab_type03);
                enemy_type03.name = "Enemy_type01_" + i.ToString();
                enemy_type03.SetActive(false);

                //GameObject Enemy_Moth = (GameObject)Instantiate(enemyPrefab_type02);
                //Enemy_Moth.name = "Enemy_Moth_" + i.ToString();
                //Enemy_Moth.transform.GetChild(0).gameObject.SetActive(false);
                //Enemy_Moth.SetActive(false);

                enemyPrefabList.Add(enemy_type01);
                enemyPrefabList.Add(enemy_type02);
                enemyPrefabList.Add(enemy_type03);
            }
        }

        private IEnumerator WitchSkill01()
        {
            summonCount = 0;
            while (summonCount < maxSummon)
            {
                foreach (GameObject enemy in enemyPrefabList)
                {
                    //GameObject enemy = Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count-1)], tempGate.transform.position, Quaternion.identity);
                    //GameObject enemy = Instantiate(enemyPrefab_type01, tempGate.transform.position, Quaternion.identity);
                    //enemy.SetActive(true);

                    if (!enemy.activeSelf)
                    {
                        Transform tempGatTR = transform.GetChild(0).GetChild(Random.Range(0, 9)).transform;

                        tempGatTR.LookAt(PlayerCtrl.instance.PlayerTr);

                        if (!tempGatTR.GetChild(0).gameObject.activeSelf)
                        {
                            tempGatTR.GetChild(0).gameObject.SetActive(true);
                        }

                        enemy.transform.position = tempGatTR.position;
                        //enemy.transform.parent = transform;

                        enemy.SetActive(true);

                        StartCoroutine(CloseGate(tempGatTR.GetChild(0).gameObject));
                        summonCount++;
                        break;
                    }
                }

                yield return new WaitForSeconds(0.6f);
            }
            yield break;
        }

        private IEnumerator CloseGate(GameObject tempGate)
        {
            yield return new WaitForSeconds(3.0f);
            if(tempGate.activeSelf)
                tempGate.SetActive(false);
        }

        private void WitchSkill02()
        {

        }

        private void WitchSkill03()
        {

        }

        private void WitchSkill04()
        {

        }
    }
}