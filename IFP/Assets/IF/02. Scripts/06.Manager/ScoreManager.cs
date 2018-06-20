using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
public class ScoreManager : MonoBehaviour {
    private string path;
    private void Awake()
    {
        path = Application.persistentDataPath;
    }
}
