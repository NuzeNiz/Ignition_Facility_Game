using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 20180711 SeongJun :
/// </summary>
public class Ladar : MonoBehaviour
{
    public bool isThereATree = true;

    [SerializeField]
    private GameObject greenDot;
    public GameObject redDot;
    public float distance = 1.0f;

    RectTransform map;
    Transform playerTransform;
    Transform treeTransform;
    List<GameObject> dots;
    GameObject onMapTreeDot;

    public int dotsCount { get { return dots.Count; } }

    void Start()
    {
        map = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        playerTransform = GameObject.Find("ARCore Device").transform;
        dots = new List<GameObject>();
    }

    private void TreeDisplay()
    {
        if (isThereATree)
        {
            if ((treeTransform == null) || (onMapTreeDot == null))
            {
                var TG = GameObject.FindGameObjectsWithTag("TREE").FirstOrDefault();
                if (TG == null)
                {
                    return;
                }
                treeTransform = TG.transform;
                onMapTreeDot = Instantiate(greenDot, map);
            }
            var localPosition = distance * (treeTransform.position - playerTransform.position);
            localPosition.y = localPosition.z;
            localPosition.z = 0;
            onMapTreeDot.GetComponent<RectTransform>().localPosition = localPosition;
        }
    }

    void Update()
    {
        var enemies = GameObject.FindGameObjectsWithTag("ENEMY_TYPE01").Select(a => { return a.transform; }).ToList();
        enemies.InsertRange(enemies.Count, GameObject.FindGameObjectsWithTag("ENEMY_TYPE02").Select(a => { return a.transform; }));
        enemies.InsertRange(enemies.Count, GameObject.FindGameObjectsWithTag("ENEMY_TYPE03").Select(a => { return a.transform; }));
        enemies.InsertRange(enemies.Count, GameObject.FindGameObjectsWithTag("ENEMY_TYPE01_BOSS").Select(a => { return a.transform; }));
        enemies.InsertRange(enemies.Count, GameObject.FindGameObjectsWithTag("ENEMY_TYPE02_BOSS").Select(a => { return a.transform; }));
        enemies.InsertRange(enemies.Count, GameObject.FindGameObjectsWithTag("ENEMY_TYPE03_BOSS").Select(a => { return a.transform; }));
        enemies.InsertRange(enemies.Count, GameObject.FindGameObjectsWithTag("ENEMY_TYPE04_BOSS").Select(a => { return a.transform; }));

        var gap = dots.Count - enemies.Count;
        if (gap < 0)
        {
            for (int i = 0; i < (-gap); i++)
            {
                var newDot = Instantiate(redDot, map);
                dots.Add(newDot);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            dots[i].SetActive(true);
            var localPosition = distance * (enemies[i].position - playerTransform.position);
            localPosition.y = localPosition.z;
            localPosition.z = 0;
            dots[i].GetComponent<RectTransform>().localPosition = localPosition;
        }
        for (int i = enemies.Count; i < dots.Count; i++)
        {
            dots[i].SetActive(false);
        }

        TreeDisplay();

        var localAngle = new Vector3(0, 0, playerTransform.transform.GetChild(0).eulerAngles.y);
        map.localEulerAngles = localAngle;
    }
}
