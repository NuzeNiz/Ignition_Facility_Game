using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace IF
{
    public class GameUIManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static GameUIManagement GameUIManagerInstance = null;

        /// <summary>
        /// 20180403 SangBin : Player Total Score
        /// </summary>
        private int totalScore = 0;
        
        /// <summary>
        /// 20180403 SangBin : Player Text Score Link
        /// </summary>sss
        public Text textScore;

        /// <summary>
        /// 20180403 SangBin : Player Text Position Link
        /// </summary>
        public Text PlayerPos;

        /// <summary>
        /// 20180403 SangBin : Player's Health Power Bar
        /// </summary>
        public Image playerHP_Bar;

        //-----------------------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            GameUIManagerInstance = this;
        }

        // Use this for initialization
        void Start()
        {
            DisplayScore(0);

        }

        // Update is called once per frame
        void Update()
        {
            playerHP_Bar.fillAmount = (float)(PlayerCtrl.PlayerInstance.PlayerHP / PlayerCtrl.PlayerInstance.PlayerMaxHP);

            DisplayDebug();
        }

        /// <summary>
        /// 20180403 SangBin : Add Score
        /// </summary>
        public void DisplayScore(int addScore)
        {
            totalScore += addScore;
            textScore.text = "Score <color=#FFA400FF>" + totalScore.ToString() + "</color>";

        }

        /// <summary>
        /// 20180403 SangBin : Display some system information for making sure of them
        /// </summary>
        public void DisplayDebug()
        {
            PlayerPos.text = "X : <color=#FF000064>" + PlayerCtrl.PlayerInstance.PlayerTr.position.x.ToString() + "</color>"
                + "  Y : <color=#FF990064>" + PlayerCtrl.PlayerInstance.PlayerTr.position.y.ToString() + "</color>"
                + "  Z : <color=#00FF1764>" + PlayerCtrl.PlayerInstance.PlayerTr.position.z.ToString() + "</color>";
        }
    }
}