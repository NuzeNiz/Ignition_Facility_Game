using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IF
{
    public class HPDisplayer : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rotationAxis;

        [SerializeField]
        private Image HPImage;

        //public double SetHP
        //{
        //    private get { return SetHP; }
        //    set
        //    {
        //        HPImage.fillAmount = (float)(value / 100.0d);
        //        SetHP = value;

        //        if (SetHP <= 0.0d)
        //        {
        //            gameObject.SetActive(false);
        //        }
        //        else
        //        {
        //            gameObject.SetActive(true);
        //        }
        //    }
        //}

        /// <summary>
        /// 20180516 SungJun :
        /// 20180530 SangBin : add max hp & refactoring
        /// </summary>
        public void SetHP(double currentHP, double MaxHP)
        {
            if (currentHP >= 0.0d)
            {
                HPImage.fillAmount = (float)(currentHP / MaxHP);
            }
            else
            {
                HPImage.fillAmount = 0.0f;
            }
        }

        private void Awake()
        {
            //HPImage.fillAmount = 1.0f;

            //gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            HPImage.fillAmount = 0.0f;
        }

        private void Update()
        {
            rotationAxis.LookAt(GameObject.Find("ARCore Device").transform);
        }
    }
}