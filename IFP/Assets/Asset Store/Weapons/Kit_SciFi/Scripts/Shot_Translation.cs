using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Translation : MonoBehaviour
{
    public GameObject objShot;

    public float velShot = 2;
    public float timeDestroy = 5;

    void Start ()
    {
		
	}

	void Update ()
    {
        this.transform.Translate(Vector3.left * Time.deltaTime * velShot);
        Destroy(objShot, timeDestroy);
    }
}
