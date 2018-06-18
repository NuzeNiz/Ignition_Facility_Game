using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyLocationChecker : MonoBehaviour
{

    public bool isInLocation { get; private set; }
    public bool isInGroup { get; private set; }

    public string contents { get; private set; }

    private readonly float groupDistance = 0.000872629430946f;
    private readonly float locationDistance = 0.00016814009039972156f;

    private float[] myPosition = { 0.0f, 0.0f };

    private Thread locationChecker;
    private CancellationTokenSource cts;


    private void Awake()
    {
        var tAsset = Resources.Load("data") as TextAsset;
        var xDoc = XElement.Parse(tAsset.text);

        Input.location.Start();

        cts = new CancellationTokenSource();

        locationChecker = new Thread(() =>
          {
              IEnumerable<XElement> xElement = xDoc.Elements("pin");

              try
              {
                  while (true)
                  {
                      cts.Token.ThrowIfCancellationRequested();
                      var selected = xElement.Where(xe =>
                      {
                          float[] pos = { float.Parse(xe.Element("latitude").Value), float.Parse(xe.Element("longitude").Value) };
                          var dis = DistanceCalc(myPosition, pos);
                          var type = xe.Element("type").Value;
                          if (type == "group")
                          {
                              return dis <= groupDistance;
                          }
                          else
                          {
                              return dis <= locationDistance;
                          }
                      }).FirstOrDefault();

                      if (selected != null)
                      {
                          var type = selected.Element("type").Value;
                          if (type == "group")
                          {
                              xElement = selected.Elements("childs");
                              isInGroup = true;
                          }
                          else if (type == "location")
                          {
                              isInLocation = true;
                          }
                          contents = selected.Element("contents").Value;
                      }
                      else
                      {
                          xElement = xDoc.Elements("pin");
                          isInGroup = false;
                          isInLocation = false;
                          contents = "none";
                      }
                  }
              }
              catch
              {
                  Debug.Log("stop!!");
              }
          });
        locationChecker.Start();

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
        cts.Cancel();
        cts.Dispose();
        locationChecker.Join();
    }
}
