using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManagement : MonoBehaviour
{
    /// <summary>
    /// 20180403 SangBin : Start button event handler
    /// </summary>
    public void OnclickStartBtn()
    {
        SceneManager.LoadScene("GameScene A");
    }

    public void OnclickQuitBtn()
    {
        Application.Quit();
    }

    public void OnclickTempBtn()
    {
        SceneManager.LoadScene("StoryMode Chapter 3");
    }
}
