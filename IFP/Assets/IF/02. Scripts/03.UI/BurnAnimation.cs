using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BurnAnimation : MonoBehaviour {

    public List<Sprite> animateSprites;
    public float speed = 0.0f;
    public string nextSceneName = "none";

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
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(nextSceneName);
    }
}
