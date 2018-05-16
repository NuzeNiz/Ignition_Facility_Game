using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Weap : MonoBehaviour
{
    public GameObject effectShot;
    public GameObject shot;
    public Transform spotShot;

    public int bullets = 6;
    int maxBullets;

    public Material Mweapon;

    public Texture Emissive1;
    public Texture Emissive2;
    public Texture Emissive3;
    public Texture Emissive4;


    void Start ()
    {
        maxBullets = bullets;
	}

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(bullets >=1)
            {
                Instantiate(shot);
                Instantiate(effectShot, spotShot);

                shot.transform.position = spotShot.transform.position;
                effectShot.transform.position = spotShot.transform.position;

                bullets -= 1;
            }
            else{
                bullets = maxBullets;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            bullets = maxBullets;
        }

        if (bullets >= 6)
        {
            Mweapon.SetTexture("_EmissionMap", Emissive1);
        }
        if (bullets == 4)
        {
            Mweapon.SetTexture("_EmissionMap", Emissive2);
        }
        if (bullets == 2)
        {
            Mweapon.SetTexture("_EmissionMap", Emissive3);
        }
        if (bullets == 0)
        {
            Mweapon.SetTexture("_EmissionMap", Emissive4);
        }
    }
}
