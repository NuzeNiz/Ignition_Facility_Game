using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManagement : MonoBehaviour {
    /// <summary>
    /// 20180403 SangBin : ReStart button event handler
    /// </summary>
    public void OnclickReStartBtn()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
