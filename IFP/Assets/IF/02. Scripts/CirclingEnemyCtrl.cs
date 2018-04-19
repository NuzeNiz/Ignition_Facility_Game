using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclingEnemyCtrl : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Tr.position, Vector3.up, 20.0f * Time.deltaTime);
    }
}
