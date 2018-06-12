using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyLocationChecker2 : MonoBehaviour
{

    public bool isInLocation { get; private set; }
    public bool isInGroup { get; private set; }

    private readonly float groupDistance = 0.000872629430946f;
    private readonly float locationDistance = 0.00016814009039972156f;

    private float[] myPosition = new float[2];

    private void Awake()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start();
        }
    }

    private void Update()
    {
        myPosition[0] = Input.location.lastData.latitude;
        myPosition[1] = Input.location.lastData.longitude;
    }

    float DistanceCalc(float[] a, float[] b)
    {
        float[] c = { a[0] - b[0], a[1] - b[1] };
        var cDis = Math.Sqrt(c[0] * c[0] + c[1] * c[1]);

        return (float)cDis;
    }

    private void OnDisable()
    {
        Input.location.Stop();
    }
}
