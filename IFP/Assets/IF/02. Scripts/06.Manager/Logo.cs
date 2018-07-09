using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace IFP {
    public class Logo : MonoBehaviour
    {

        [SerializeField]
        private Image logo;
        [SerializeField]
        private float frameSpeed = 0.0f;

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
        /// 20180709 SeongJun :
        /// </summary>
        IEnumerator NextScene()
        {
            //yield return new WaitForSeconds(2.5f);
            //SceneManager.LoadScene(nextScene);

            if (logo != null)
            {
                while (logo.color.a < 1.0f)
                {
                    logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a + 0.1f);
                    yield return new WaitForSeconds(frameSpeed);
                }
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                yield return new WaitForSeconds(2.5f);
                SceneManager.LoadScene(nextScene);
            }
        }

    }

}
