using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IFP {
    public class StartMenuManagement : MonoBehaviour
    {
        [SerializeField]
        private Curtain curtain;
        /// <summary>
        /// 20180403 SangBin : Start button event handler
        /// </summary>
        public void OnclickInfinityModeBtn()
        {
            TempStageManagement.instance.CurrentStageLevel = 10;
            SceneManager.LoadScene("GameScene A");
        }

        /// <summary>
        /// 20180614 SangBin :
        /// </summary>
        public void OnclickQuitBtn()
        {
            Application.Quit();
        }

        /// <summary>
        /// 20180617 SangBin :
        /// </summary>
        public void OnClickChpater1()
        {
            TempStageManagement.instance.CurrentStageLevel = 1;
        }

        /// <summary>
        /// 20180617 SangBin :
        /// </summary>
        public void OnClickChpater2_1()
        {
            TempStageManagement.instance.CurrentStageLevel = 2;
            TempStageManagement.instance.talkSceneMeta.nextSceneName = "GameScene A";
            TempStageManagement.instance.talkSceneMeta.scriptNumber = 1;
            //SceneManager.LoadScene("GameScene A");
            curtain.PlayAnimationWithCallBack(() => SceneManager.LoadScene("IntoScene"));
        }

        /// <summary>
        /// 20180617 SangBin :
        /// </summary>
        public void OnClickChpater2_2()
        {
            TempStageManagement.instance.CurrentStageLevel = 3;
            SceneManager.LoadScene("GameScene A");
        }

        /// <summary>
        /// 20180617 SangBin :
        /// </summary>
        public void OnClickChpater2_3()
        {
            TempStageManagement.instance.CurrentStageLevel = 4;
            SceneManager.LoadScene("GameScene A");
        }

        /// <summary>
        /// 20180617 SangBin :
        /// </summary>
        public void OnClickChapter3()
        {
            TempStageManagement.instance.CurrentStageLevel = 5;
            SceneManager.LoadScene("GameScene E");
            //SceneManager.LoadScene("GameScene A");
        }
    }
}