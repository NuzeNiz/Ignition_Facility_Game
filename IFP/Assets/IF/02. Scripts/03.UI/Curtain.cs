using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curtain : MonoBehaviour {

    public bool isFadeOut = true;
    public bool withStart = true;

    [SerializeField]
    private float waitSecond = 0.0f;

    private Image myCurtain;

    private void Awake()
    {
        myCurtain = gameObject.GetComponent<Image>();
        if (withStart)
        {
            PlayAnimation();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void PlayAnimation()
    {
        if (isFadeOut)
        {
            StartCoroutine(CurtainOpen());
        }
        else
        {
            StartCoroutine(CurtaionOff());
        }
    }

    private IEnumerator CurtainOpen()
    {
        while (myCurtain.color.a > 0.0f)
        {
            myCurtain.color = new Color(myCurtain.color.r, myCurtain.color.g, myCurtain.color.b, myCurtain.color.a - 0.01f);
            yield return new WaitForSeconds(waitSecond);
        }
        gameObject.SetActive(false);
    }

    private IEnumerator CurtaionOff()
    {
        gameObject.SetActive(true);
        while (myCurtain.color.a < 1.0f)
        {
            myCurtain.color = new Color(myCurtain.color.r, myCurtain.color.g, myCurtain.color.b, myCurtain.color.a + 0.01f);
            yield return new WaitForSeconds(waitSecond);
        }
    }
}
