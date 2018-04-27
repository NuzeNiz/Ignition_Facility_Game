using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    public class CirclingEnemyCtrl : MonoBehaviour
    {
        private void Update()
        {
            transform.RotateAround(IF.DefenseStationCtrl.DS_Instance.DefenseStationTR.position, Vector3.up, 40.0f * Time.deltaTime);
        }

        void OnHit()
        {
            GameLogicManagement.GLM_Instance.GenerateItem(transform);
            gameObject.SetActive(false);
        }
    }
}