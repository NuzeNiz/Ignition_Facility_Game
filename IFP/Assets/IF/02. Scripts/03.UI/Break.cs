using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 20180620 SeongJun :
/// </summary>
public class Break : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("Break");
    }
}
