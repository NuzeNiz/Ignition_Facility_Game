using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IFP {
    public class StartMenuManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Start button event handler
        /// </summary>
        public void OnclickStartBtn()
        {
            TempStageManagement.instance.CurrentStageLevel = 2;
            SceneManager.LoadScene("GameScene A");
        }

        public void OnclickQuitBtn()
        {
            Application.Quit();
        }

        public void OnclickTempBtn()
        {
            TempStageManagement.instance.CurrentStageLevel = 5;
            //SceneManager.LoadScene("StoryMode Chapter 3");
            SceneManager.LoadScene("GameScene A");
        }
    }
}