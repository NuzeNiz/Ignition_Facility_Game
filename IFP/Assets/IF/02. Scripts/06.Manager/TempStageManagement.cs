using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class TalkSceneMetaData
    {
        public string nextSceneName = "none";
        public int scriptNumber = 0;
    }

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
        /// 20180711 SeongJun :
        /// </summary>
        public TalkSceneMetaData talkSceneMeta = new TalkSceneMetaData();

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
