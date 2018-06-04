using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyLocationChecker : MonoBehaviour {

    [SerializeField]
    private Button locationModBtn;
    private Image btnImage;

    private Color originColor;

    private float[] coord = new float[2];
    private float distance = 0.00016814009039972156f;

    private void Awake()
    {
        btnImage = locationModBtn.GetComponent<Image>();
        originColor = btnImage.color;

        //coord[0] = 37.7131538f;
        //coord[1] = 126.889557f;

        coord[0] = 37.71359f;
        coord[1] = 126.890091f;

        Input.location.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var latitude = Input.location.lastData.latitude;
            var longitude = Input.location.lastData.longitude;

            var a = coord[0] - latitude;
            a = a * a;
            var b = coord[1] - longitude;
            b = b * b;
            var currentDistance = Mathf.Sqrt(a + b);

            if (currentDistance < distance)
            {
                btnImage.color = new Color(1.0f, 1.0f, 1.0f);
            }
            else
            {
                btnImage.color = originColor;
            }
        }
    }

    private void OnDisable()
    {
        Input.location.Stop();
    }
}
