using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
public class Compas : MonoBehaviour {
    public float[] myPosition = { 0, 0 };

    private void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            myPosition[0] = Input.location.lastData.latitude;
            myPosition[1] = Input.location.lastData.longitude;
        }
    }
}
