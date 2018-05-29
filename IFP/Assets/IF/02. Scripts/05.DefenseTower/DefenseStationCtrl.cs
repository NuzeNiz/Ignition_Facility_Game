using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class DefenseStationCtrl : MonoBehaviour
    {


        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static DefenseStationCtrl instance = null;

        public delegate void DS_EventHandler();
        public static event DS_EventHandler AbsorbExp;
        public static event DS_EventHandler WearEnergy;

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

        private double defenseStation_Fire_Energy = 0.0d;
        public double DefenseStation_Fire_Energy
        {
            get { return defenseStation_Fire_Energy; }
        }

        private double defenseStation_Water_Energy = 0.0d;
        public double DefenseStation_Water_Energy
        {
            get { return defenseStation_Water_Energy; }
        }

        private double defenseStation_Leaf_Energy = 0.0d;
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
                //defenseStation_HP -= collision.gameObject.GetComponent<EnemyProjectileType01Ctrl>().projectileDamage;
                defenseStation_HP -= BalanceManagement.instance.EnemyProjectile01damage;
                //collision.gameObject.GetComponent<TrailRenderer>().enabled = false;
                collision.gameObject.SetActive(false);
            }

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

            //기본 경험치
            defenseStation_exp += expValue;
            AbsorbExp();
        }

        private IEnumerator OnWearEnergy()
        {
            //시작 어드벤테이지 5초
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
    }
}