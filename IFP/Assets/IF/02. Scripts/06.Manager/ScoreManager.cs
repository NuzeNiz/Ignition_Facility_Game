using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private string path;
    private void Awake()
    {
        path = Application.persistentDataPath;
    }
}
