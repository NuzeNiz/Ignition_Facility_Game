using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnAnimation : MonoBehaviour {

    public List<Sprite> animateSprites;
    public float speed = 0.0f;

    private Image myImage;

    private void Awake()
    {
        myImage = gameObject.GetComponent<Image>();
        StartCoroutine(Projector());
    }

    public IEnumerator Projector()
    {
        foreach(var s in animateSprites)
        {
            myImage.sprite = s;
            yield return new WaitForSeconds(speed);
        }
    }
}
