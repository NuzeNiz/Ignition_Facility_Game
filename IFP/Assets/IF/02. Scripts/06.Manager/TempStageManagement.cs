using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class TempStageManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180614 SangBin :
        /// </summary>
        public static TempStageManagement instance = null;

        /// <summary>
        /// 20180614 SangBin :
        /// </summary>
        private int currentStageLevel = 0;

        /// <summary>
        /// 20180614 SangBin :
        /// </summary>
        public int CurrentStageLevel { get { return currentStageLevel; } set { currentStageLevel = value; } }

        /// <summary>
        /// 20180614 SangBin :
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(this);
            }
        }

    }
}
