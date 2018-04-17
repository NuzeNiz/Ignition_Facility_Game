using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 20180412 SeongJun : Display Enemies
/// </summary>
public class Ladar : MonoBehaviour {
    public GameObject redDot;
    public float distance = 1.0f;

    RectTransform map;
    Transform myTransform;
    List<GameObject> dots;

    public int dotsCount { get { return dots.Count; } }

    void Start () {
        map = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        myTransform = GameObject.Find("ARCore Device").transform;
        dots = new List<GameObject>();
	}


    void Update()
    {
        var enemies = GameObject.FindGameObjectsWithTag("ENEMY_TYPE01").Select(a => { return a.transform; }).ToList();
        var gap = dots.Count - enemies.Count;
        if (gap < 0)
        {
            for (int i = 0; i < (-gap); i++)
            {
                var newDot = Instantiate(redDot, map);
                dots.Add(newDot);
            }
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            dots[i].SetActive(true);
            var localPosition = distance * (enemies[i].position - myTransform.position);
            localPosition.y = localPosition.z;
            localPosition.z = 0;
            dots[i].GetComponent<RectTransform>().localPosition = localPosition;
        }
        for(int i = enemies.Count; i < dots.Count; i++)
        {
            dots[i].SetActive(false);
        }
        //var localAngle = new Vector3(0, 0, myTransform.eulerAngles.y);
        var localAngle = new Vector3(0, 0, myTransform.transform.GetChild(0).eulerAngles.y);
        map.localEulerAngles = localAngle;
    }
}
