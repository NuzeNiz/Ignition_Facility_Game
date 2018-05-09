using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollTrigger : MonoBehaviour {
    private Collider2D col2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("SetMoveUp", false);

        Debug.Log("collision!!!");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.SendMessage("SetMoveUp", true);
    }
}
