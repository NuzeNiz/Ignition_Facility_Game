using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class BalanceManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public static BalanceManagement instance = null;

        #region Fields : Player to Enemy
        /// <summary>
        /// 20180521 SangBin : Damage Value WeaponType01 to Item Monster
        /// </summary>
        private double wToIM = 100.0d;

        /// W1 - Standard   E1 - Water
        /// W2 - Fire       E2 - Fire
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
        private double w2ToE1 = 0.3;
        private double w2ToE2 = 0.6; //per frame
        private double w2ToE3 = 1.2;
        private double w2ToE4 = 0.0d;
        #endregion

        /// <summary>
        /// 20180521 SangBin : Damage Values WeaponType03 to Enemies
        /// </summary>
        #region Fields : WeaponType03 to Enemy
        private double w3ToE1 = 25.0d;
        private double w3ToE2 = 50.0d;
        private double w3ToE3 = 12.5d;
        private double w3ToE4 = 0.0d;
        #endregion

        /// <summary>
        /// 20180521 SangBin : Damage Values WeaponType04 to Enemies
        /// </summary>
        #region Fields : WeaponType04 to Enemy
        private double w4ToE1 = 50.0d;
        private double w4ToE2 = 12.5d;
        private double w4ToE3 = 25.0d;
        private double w4ToE4 = 0.0d;
        #endregion
        #endregion

        //---------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
        }


        /// <summary>
        /// 20180515 SangBin : Calculation Player Striking Power
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
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_TYPE04":
                            strikingPower = w1ToE;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                        case "ENEMY_MOTH":
                            strikingPower = 50.0d;
                            break;
                    }
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType02:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                            strikingPower = w2ToE1;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = w2ToE2;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = w2ToE3;
                            break;
                        case "ENEMY_TYPE04":
                            strikingPower = w2ToE4;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                        case "ENEMY_MOTH":
                            strikingPower = 10.0d;
                            break;
                    }
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType03:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                            strikingPower = w3ToE1;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = w3ToE2;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = w3ToE3;
                            break;
                        case "ENEMY_TYPE04":
                            strikingPower = w3ToE4;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                        case "ENEMY_MOTH":
                            strikingPower = 50.0d;
                            break;
                    }
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType04:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                            strikingPower = w4ToE1;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = w4ToE2;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = w4ToE3;
                            break;
                        case "ENEMY_TYPE04":
                            strikingPower = w4ToE4;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = wToIM;
                            break;
                        case "ENEMY_MOTH":
                            strikingPower = 50.0d;
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