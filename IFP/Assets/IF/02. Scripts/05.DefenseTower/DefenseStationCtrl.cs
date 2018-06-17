using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class DefenseStationCtrl : MonoBehaviour
    {


        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static DefenseStationCtrl instance = null;

        /// <summary>
        /// 20180530 SangBin : 
        /// </summary>
        public delegate void DS_EventHandler();
        public static event DS_EventHandler AbsorbExp;
        public static event DS_EventHandler WearEnergy;
        public static event DS_EventHandler DS_Damaged;
        //public static event DS_EventHandler DS_FallDown;

        /// <summary>
        /// 20180607 SangBin : 
        /// </summary>
        [SerializeField]
        private GameObject levelupEffect;

        #region Fields : DS Statistic
        /// <summary>
        /// 20180403 SangBin : Defense Station's Transform
        /// </summary>
        private Transform defenseStationTR;

        /// <summary>
        /// 20180403 SangBin : Defense Station's Transform Property
        /// </summary>
        public Transform DefenseStationTR { get { return defenseStationTR; } }

        /// <summary>
        /// 20180501 SangBin : Defense Station's Max Health Power
        /// </summary>
        private double defenseStation_MAXHP;

        /// <summary>
        /// 20180501 SangBin : Defense Station's Max Health Power Property
        /// </summary>
        public double DefenseStation_MAXHP { get { return defenseStation_MAXHP; } }

        /// <summary>
        /// 20180501 SangBin : Defense Station's Current Health Power
        /// </summary>
        private double defenseStation_HP = 100.0d;

        /// <summary>
        /// 20180501 SangBin : Defense Station's Current Health Power Property
        /// </summary>
        public double DefenseStation_HP { get { return defenseStation_HP; } }

        private double defenseStation_exp = 0.0d;
        public double DefenseStation_exp
        {
            get { return defenseStation_exp; }
        }

        private int treeState = 0;

        private double defenseStation_Fire_Energy = 50.0d;
        public double DefenseStation_Fire_Energy
        {
            get { return defenseStation_Fire_Energy; }
        }

        private double defenseStation_Water_Energy = 50.0d;
        public double DefenseStation_Water_Energy
        {
            get { return defenseStation_Water_Energy; }
        }

        private double defenseStation_Leaf_Energy = 50.0d;
        public double DefenseStation_Leaf_Energy
        {
            get { return defenseStation_Leaf_Energy; }
        }
        #endregion
            //-----------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            //if (instance == null)
            //{
            //    instance = this;
            //    DontDestroyOnLoad(gameObject);
            //}
            //else
            //{
            //    DestroyImmediate(this);
            //}
            instance = this;
            defenseStationTR = this.gameObject.transform;
            defenseStation_MAXHP = defenseStation_HP;
        }

        private void Start()
        {
            StartCoroutine(OnWearEnergy());
        }

        /// <summary>
        /// 20180501 SangBin : Collision event between Defense Station and Enemy Bee Stinger
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "ENEMY_TYPE01_PROJECTILE")
            {
                StartCoroutine(TreeShake());
                //defenseStation_HP -= collision.gameObject.GetComponent<EnemyProjectileType01Ctrl>().projectileDamage;
                defenseStation_HP -= BalanceManagement.instance.EnemyProjectile01damage;
                //collision.gameObject.GetComponent<TrailRenderer>().enabled = false;
                collision.gameObject.SetActive(false);
                DS_Damaged();

                if (defenseStation_HP<=0.0d)
                {
                    //DS_FallDown();
                    GameManagement.instance.GameOver();
                }
            }

        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        public DefenseStationCtrl Instance()
        {
            return instance;
        }

        /// <summary>
        /// 20180607 SangBin : 
        /// </summary>
        private IEnumerator TreeShake()
        {
            Vector3 originPos = defenseStationTR.position;

            float elapsedTime = 0.0f;

            while (elapsedTime < 1.0f)
            {
                Vector3 randomPoint = originPos + Random.insideUnitSphere * 0.1f;

                defenseStationTR.position = Vector3.Lerp(defenseStationTR.position, randomPoint, Time.deltaTime * 4.0f);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            defenseStationTR.position = originPos;
        }

        /// <summary>
        /// 20180501 SangBin : Trigger event between Defense Station and Enemy Moth
        /// </summary>
        //private void OnTriggerStay(Collider collider)
        //{
        //    if (collider.gameObject.tag == "ENEMY_MOTH_SCALE")
        //    {
        //            defenseStation_HP -= 0.01d; //damage per frame
        //    }
        //}

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public void OnAbsorbEnergy(string enemyTag)
        {
            //double sp = BalanceManagement.instance.DefenseStation_Spirit_Energy;
            double expValue = BalanceManagement.instance.DefenseStation_Spirit_Energy;

            switch (enemyTag)
            {
                case "ENEMY_TYPE01":
                    defenseStation_Fire_Energy += BalanceManagement.instance.DefenseStation_Spirit_Energy;
                    break;

                case "ENEMY_TYPE01_BOSS":
                    //if (지금 스테이지가 무한 모드인지 확인)
                    //{ //무한모드에서는 얘가 흡수되야함 보스 추가 경험치도
                    //    defenseStation_exp += (BalanceManagement.instance.DefenseStation_exp * 2);
                    //    defenseStation_Fire_Energy += (BalanceManagement.instance.DefenseStation_Spirit_Energy * 2);
                    //}
                    break;

                case "ENEMY_TYPE02":
                    defenseStation_Water_Energy += BalanceManagement.instance.DefenseStation_Spirit_Energy;
                    break;

                case "ENEMY_TYPE02_BOSS":
                    //if (지금 스테이지가 무한 모드인지 확인)
                    //{ //무한모드에서는 얘가 흡수되야함
                    //    defenseStation_exp += (BalanceManagement.instance.DefenseStation_exp * 2);
                    //    defenseStation_Fire_Energy += (BalanceManagement.instance.DefenseStation_Spirit_Energy * 2);
                    //}
                    break;

                case "ENEMY_TYPE03":
                    defenseStation_Leaf_Energy += BalanceManagement.instance.DefenseStation_Spirit_Energy;
                    break;

                case "ENEMY_TYPE03_BOSS":
                    //if (지금 스테이지가 무한 모드인지 확인)
                    //{ //무한모드에서는 얘가 흡수되야함
                    //    defenseStation_exp += (BalanceManagement.instance.DefenseStation_exp * 2);
                    //    defenseStation_Fire_Energy += (BalanceManagement.instance.DefenseStation_Spirit_Energy * 2);
                    //}
                    break;
            }


            if(defenseStation_Fire_Energy>10.0d && defenseStation_Water_Energy>10.0d && defenseStation_Leaf_Energy>10.0d)
                //기본 경험치
                defenseStation_exp += expValue;


            if (defenseStation_exp >= 100.0d)
            {
                GameObject le = Instantiate(levelupEffect, transform.parent.position, Quaternion.identity);
                defenseStation_exp = 0.0d;
                transform.parent.transform.localScale *= 2.0f;
                treeState++;
                Destroy(le, 6.0f);

                //일단 4단계
                if (treeState == 3)
                {
                    GameManagement.instance.GameClear();
                }
            }

            AbsorbExp();
        }


        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private IEnumerator OnWearEnergy()
        {
            //시작 핸디캡 5초
            yield return new WaitForSeconds(5.0f);
            double wValue = BalanceManagement.instance.DefenseStation_wValue;

            //while(스테이지 상태 필요) //반복문 빠져나오도록
            while (true)
            {
                if(defenseStation_Fire_Energy>= wValue)
                    defenseStation_Fire_Energy -= wValue;

                if (defenseStation_Water_Energy >= wValue)
                    defenseStation_Water_Energy -= wValue;

                if (defenseStation_Leaf_Energy >= wValue)
                    defenseStation_Leaf_Energy -= wValue;

                WearEnergy();
                yield return new WaitForSeconds(0.5f);
            }

        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == "ENEMY_TYPE03_BOX")
            {
                StartCoroutine(TreeShake());

                defenseStation_HP -= BalanceManagement.instance.EnemyProjectile01damage;
                //defenseStation_HP -= 10.0d;
                DS_Damaged();

                if (defenseStation_HP <= 0.0d)
                {
                    GameManagement.instance.GameOver();
                }
            }
        }
    }
}