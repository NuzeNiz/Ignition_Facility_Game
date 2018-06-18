using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IFP {
    public class Logo : MonoBehaviour
    {

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        [SerializeField]
        private string nextScene;

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        // Use this for initialization
        void Start()
        {
            StartCoroutine(NextScene());
        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        IEnumerator NextScene()
        {
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(nextScene);
        }

    }

}
