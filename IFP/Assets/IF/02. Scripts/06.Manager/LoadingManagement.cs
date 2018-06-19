using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class LoadingManagement : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Loading());
        }

        private IEnumerator Loading()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}