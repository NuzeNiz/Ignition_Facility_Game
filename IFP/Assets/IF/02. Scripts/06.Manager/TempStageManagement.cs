using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class TempStageManagement : MonoBehaviour
    {
        public static TempStageManagement instance = null;

        private int currentStageLevel = 0;

        public int CurrentStageLevel { get { return currentStageLevel; } set { currentStageLevel = value; } }

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
