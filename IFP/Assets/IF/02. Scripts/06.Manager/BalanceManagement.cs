using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class BalanceManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public static BalanceManagement instance = null;

        #region Fields : Damage Value Player to Enemy
        /// <summary>
        /// 20180521 SangBin : Damage Value WeaponType01 to Item Monster
        /// </summary>
        private double wToIM = 100.0d;

        /// W1 - Standard   E1 - Fire
        /// W2 - Fire       E2 - Water
        /// W3 - Water      E3 - Leaf
        /// W4 - Leaf       E4 - Witchs

        /// <summary>
        /// 20180521 SangBin : Damage Values WeaponType01 to Enemies
        /// </summary>
        #region Fields : WeaponType01 to Enemy
        private double w1ToE = 25.0d;
        //private double w1ToE2 = 25.0d;
        //private double w1ToE3 = 25.0d;
        //private double w1ToE4 = 25.0d;
        #endregion

        /// <summary>
        /// 20180521 SangBin : Damage Values WeaponType02 to Enemies
        /// </summary>
        #region Fields : WeaponType02 to Enemy
        private double w2ToE1 = 0.6;
        private double w2ToE2 = 0.3; //per frame
        private double w2ToE3 = 1.2;
        private double w2ToE4 = 0.15d;
        #endregion

        /// <summary>
        /// 20180521 SangBin : Damage Values WeaponType03 to Enemies
        /// </summary>
        #region Fields : WeaponType03 to Enemy
        private double w3ToE1 = 50.0d;
        private double w3ToE2 = 25.0d;
        private double w3ToE3 = 12.5d;
        private double w3ToE4 = 6.25d;
        #endregion

        /// <summary>
        /// 20180521 SangBin : Damage Values WeaponType04 to Enemies
        /// </summary>
        #region Fields : WeaponType04 to Enemy
        private double w4ToE1 = 12.5d;
        private double w4ToE2 = 50.0d;
        private double w4ToE3 = 25.0d;
        private double w4ToE4 = 6.25d;
        #endregion
        #endregion

        #region Fields : Enemy Projectile Statistic
        /// <summary>
        /// 20180529 SangBin :
        /// </summary>
        public float EnemyProjectile01Speed { get{ return 50.0f; } }

        /// <summary>
        /// 20180529 SangBin :
        /// </summary>
        public double EnemyProjectile01damage { get { return 0.1d; } }
        #endregion

        #region Fields : Defense Station Energy
        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private double defenseStation_exp = 5.0d;
        public double DefenseStation_exp { get { return defenseStation_exp; } set { defenseStation_exp = value; } }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private double defenseStation_Spirit_Energy = 10.0d;
        public double DefenseStation_Spirit_Energy { get { return defenseStation_Spirit_Energy; } set { defenseStation_Spirit_Energy = value; } }

        ///// <summary>
        ///// 20180529 SangBin : 
        ///// </summary>
        //private double defenseStation_Fire_Energy = 10.0d;
        //public double DefenseStation_Fire_Energy { get { return defenseStation_Fire_Energy; } set { defenseStation_Fire_Energy = value; } }

        ///// <summary>
        ///// 20180529 SangBin : 
        ///// </summary>
        //private double defenseStation_Water_Energy = 10.0d;
        //public double DefenseStation_Water_Energy { get { return defenseStation_Water_Energy; } set { defenseStation_Water_Energy = value; } }

        ///// <summary>
        ///// 20180529 SangBin : 
        ///// </summary>
        //private double defenseStation_Leaf_Energy = 10.0d;
        //public double DefenseStation_Leaf_Energy { get { return defenseStation_Leaf_Energy; } set { defenseStation_Leaf_Energy = value; } }

        ///// <summary>
        ///// 20180529 SangBin : 
        ///// </summary>
        private double defenseStation_wValue = 0.5d;
        public double DefenseStation_wValue { get { return defenseStation_wValue; } set { defenseStation_wValue = value; } } 

        #endregion 

        //---------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
        }


        /// <summary>
        /// 20180515 SangBin : Calculation Player Striking Power
        /// 20180529 SangBin : Add Boss
        /// </summary>
        public double CalcPlayerStrkingPower(string enemyTag, WeaponCtrl.WeaponTypeEnum currentWeaponType)
        {
            double strikingPower = 0.0d;

            switch (currentWeaponType)
            {
                case WeaponCtrl.WeaponTypeEnum.weaponType01:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                        case "ENEMY_TYPE01_BOSS":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_TYPE02":
                        case "ENEMY_TYPE02_BOSS":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_TYPE03":
                        case "ENEMY_TYPE03_BOSS":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_TYPE04_BOSS":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                    }
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType02:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                        case "ENEMY_TYPE01_BOSS":
                            strikingPower = w2ToE1;
                            break;
                        case "ENEMY_TYPE02":
                        case "ENEMY_TYPE02_BOSS":
                            strikingPower = w2ToE2;
                            break;
                        case "ENEMY_TYPE03":
                        case "ENEMY_TYPE03_BOSS":
                            strikingPower = w2ToE3;
                            break;
                        case "ENEMY_TYPE04_BOSS":
                            strikingPower = w2ToE4;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                    }
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType03:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                        case "ENEMY_TYPE01_BOSS":
                            strikingPower = w3ToE1;
                            break;
                        case "ENEMY_TYPE02":
                        case "ENEMY_TYPE02_BOSS":
                            strikingPower = w3ToE2;
                            break;
                        case "ENEMY_TYPE03":
                        case "ENEMY_TYPE03_BOSS":
                            strikingPower = w3ToE3;
                            break;
                        case "ENEMY_TYPE04_BOSS":
                            strikingPower = w3ToE4;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                    }
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType04:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                        case "ENEMY_TYPE01_BOSS":
                            strikingPower = w4ToE1;
                            break;
                        case "ENEMY_TYPE02":
                        case "ENEMY_TYPE02_BOSS":
                            strikingPower = w4ToE2;
                            break;
                        case "ENEMY_TYPE03":
                        case "ENEMY_TYPE03_BOSS":
                            strikingPower = w4ToE3;
                            break;
                        case "ENEMY_TYPE04_BOSS":
                            strikingPower = w4ToE4;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                    }
                    break;
            }

            return strikingPower;
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public double CalcPlayerStrkingPower_Splash(string enemyTag)
        {
            double strikingPower_Splash = 0.0d;

                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                            strikingPower_Splash = 10.0d;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower_Splash = 10.0d;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower_Splash = 10.0d;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower_Splash = 10.0d;
                            break;
                        case "ENEMY_MOTH":
                            strikingPower_Splash = 10.0d;
                            break;
                    }

            return strikingPower_Splash;
        }
    }
}