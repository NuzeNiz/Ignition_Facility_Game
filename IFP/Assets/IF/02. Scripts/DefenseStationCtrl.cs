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
        public static DefenseStationCtrl DS_Instance = null;

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

        //-----------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            DS_Instance = this;
            defenseStationTR = this.gameObject.transform;
            defenseStation_MAXHP = defenseStation_HP;
        }

        /// <summary>
        /// 20180501 SangBin : Collision event between Defense Station and enemy bullet
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "ENEMY_BEE_STINGER")
            {
                defenseStation_HP -= collision.gameObject.GetComponent<BeeStingerCtrl>().stingerDamage;
                collision.gameObject.GetComponent<TrailRenderer>().enabled = false;
                collision.gameObject.SetActive(false);
            }
        }
    }
}