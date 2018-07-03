using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IFP
{
    public class LoadingManagement : MonoBehaviour
    {
        private float loadingGauge;
        public static LoadingManagement instance = null;
        //public delegate void Loading_EventHandler();
        //public static event Loading_EventHandler Display_LoadingGauge;
        public Image loadingGauge_Bar;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //StartCoroutine(Loading());
            loadingGauge = 0.0f;
        }

        public void ShowLoadingBar()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        //private IEnumerator Loading()
        //{
        //    transform.GetChild(0).gameObject.SetActive(true);
        //    yield return new WaitForSeconds(3.0f);
        //    transform.GetChild(0).gameObject.SetActive(false);
        //}

        public void FillLoadingGauge(float value)
        {
            loadingGauge += value;

            if (loadingGauge >= 100.0f)
            {
                loadingGauge = 0.0f;
                StartCoroutine(HideLoading());
            }

            Display_LoadingGauge();
        }

        private IEnumerator HideLoading()
        {
            yield return new WaitForSeconds(1.0f);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void Display_LoadingGauge()
        {
            loadingGauge_Bar.fillAmount = loadingGauge / 100.0f;
        }

    }
}