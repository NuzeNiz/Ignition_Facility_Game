using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("Break");
    }
}
