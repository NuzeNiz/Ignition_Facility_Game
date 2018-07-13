using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptMoveUp : MonoBehaviour {
    private RectTransform rectTransform;
    [SerializeField]
    private float breack = 0.0f;
    [SerializeField]
    private float speed = 0.1f;
    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        while (rectTransform.localPosition.y < breack)
        {
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y + 1.0f, rectTransform.localPosition.z);

            yield return new WaitForSeconds(speed);
        }
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("Talking_Scene");
    }
}
