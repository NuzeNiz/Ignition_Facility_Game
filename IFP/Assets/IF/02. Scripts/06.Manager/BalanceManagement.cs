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

        //---------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
        }


        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public double PlayerStrkingPower(string enemyTag, WeaponCtrl.WeaponTypeEnum currentWeaponType)
        {
            double strikingPower = 0.0d;

            switch (currentWeaponType)
            {
                case WeaponCtrl.WeaponTypeEnum.weaponType01:
                    switch (enemyTag)
                    {
                        case "ENEMY_TYPE01":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = 50.0d;
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
                            strikingPower = 10.0d;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = 10.0d;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = 10.0d;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = 10.0d;
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
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = 50.0d;
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
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_TYPE02":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_TYPE03":
                            strikingPower = 50.0d;
                            break;
                        case "ENEMY_BUTTERFLY":
                            strikingPower = 50.0d;
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
        public double PlayerStrkingPower_Splash(string enemyTag)
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