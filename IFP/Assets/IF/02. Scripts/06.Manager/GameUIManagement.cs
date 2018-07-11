using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace IFP
{
    public class GameUIManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static GameUIManagement instance = null;

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

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public Image DefenseStation_Exp_Bar;

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public Image DefenseStation_HP_Bar;

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public Image DefenseStation_Fire_Energy_Bar;

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public Image DefenseStation_Water_Energy_Bar;

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public Image DefenseStation_Leaf_Energy_Bar;

        /// <summary>
        /// 20180702 SangBin : 
        /// </summary>sss
        public Text textAmmu;

        public Text textWaveQuest;

        public Text textTimer;

        private float timer;

        [HideInInspector]
        public bool timerState = true;
        //-----------------------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            //if (instance == null)
            //{
            //    instance = this;
            //    DontDestroyOnLoad(gameObject);
            //}
            //else
            //{
            //    DestroyImmediate(this);
            //}
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            DisplayScore(0);
        }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private void OnEnable()
        {
            PlayerCtrl.PlayerDamaged += this.PlayerDamaged;
            DefenseStationCtrl.AbsorbExp += this.AbsorbExp;
            DefenseStationCtrl.WearEnergy += this.WearEnergy;
            DefenseStationCtrl.DS_Damaged += this.DS_Damaged;
            WeaponCtrl.Display_Ammu += this.Display_Ammu;
            GameManagement.Display_enemyDeathCount += this.Display_enemyDeathCount;
        }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private void OnDisable()
        {
            PlayerCtrl.PlayerDamaged -= this.PlayerDamaged;
            DefenseStationCtrl.AbsorbExp -= this.AbsorbExp;
            DefenseStationCtrl.WearEnergy -= this.WearEnergy;
            DefenseStationCtrl.DS_Damaged -= this.DS_Damaged;
            WeaponCtrl.Display_Ammu -= this.Display_Ammu;
            GameManagement.Display_enemyDeathCount -= this.Display_enemyDeathCount;
        }

        // Update is called once per frame
        void Update()
        {
            //DisplayDebug();

            if (timerState)
            {
                timer += Time.deltaTime;
            }

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
            PlayerPos.text = "X : <color=#FF000064>" + PlayerCtrl.instance.PlayerTr.position.x.ToString() + "</color>"
                + "  Y : <color=#FF990064>" + PlayerCtrl.instance.PlayerTr.position.y.ToString() + "</color>"
                + "  Z : <color=#00FF1764>" + PlayerCtrl.instance.PlayerTr.position.z.ToString() + "</color>";
        }

        /// <summary>
        /// 20180503 SeongJun : Add Item 
        /// </summary>
        public void AddItem()
        {

        }

        /// <summary>
        /// 20180503 SeongJun : Add Weapon
        /// </summary>
        public void AddWeapon()
        {

        }

        private void Display_Ammu()
        {
            switch (WeaponCtrl.instance.CurrentWeaponType)
            {
                case WeaponCtrl.WeaponTypeEnum.weaponType01:
                    textAmmu.text = "Infinity";
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType02:
                    textAmmu.text = WeaponCtrl.instance.amm_wt02.ToString();
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType03:
                    textAmmu.text = WeaponCtrl.instance.amm_wt03.ToString();
                    break;

                case WeaponCtrl.WeaponTypeEnum.weaponType04:
                    textAmmu.text = WeaponCtrl.instance.amm_wt04.ToString();
                    break;
            }
        }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public void PlayerDamaged()
        {
            playerHP_Bar.fillAmount = (float)(PlayerCtrl.instance.PlayerHP / PlayerCtrl.instance.PlayerMaxHP);
        }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private void AbsorbExp()
        {
            DefenseStation_Exp_Bar.fillAmount = (float)(DefenseStationCtrl.instance.DefenseStation_exp / 100.0d);
        }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        private void WearEnergy()
        {
            DefenseStation_Fire_Energy_Bar.fillAmount = (float)(DefenseStationCtrl.instance.DefenseStation_Fire_Energy / 100.0d);
            DefenseStation_Water_Energy_Bar.fillAmount = (float)(DefenseStationCtrl.instance.DefenseStation_Water_Energy / 100.0d);
            DefenseStation_Leaf_Energy_Bar.fillAmount = (float)(DefenseStationCtrl.instance.DefenseStation_Leaf_Energy / 100.0d);
        }

        /// <summary>
        /// 20180529 SangBin : 
        /// </summary>
        public void DS_Damaged()
        {
            DefenseStation_HP_Bar.fillAmount = (float)(DefenseStationCtrl.instance.DefenseStation_HP / (DefenseStationCtrl.instance.DefenseStation_MAXHP));

        }

        private void Display_enemyDeathCount()
        {
            textWaveQuest.text = GameManagement.instance.currentWaveDeathEnemCount.ToString() + " / " + GameManagement.instance.GetTotalEnemyCountConst().ToString() + " 처치 !";
        }

        private void OnGUI()
        {
            int minutes = Mathf.FloorToInt(timer / 60.0f);
            int seconds = Mathf.FloorToInt(timer - minutes * 60.0f);

            string strTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            textTimer.text = strTime;

        }

    }
}