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

        //-----------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            DS_Instance = this;
            defenseStationTR = this.gameObject.transform;
        }
    }
}