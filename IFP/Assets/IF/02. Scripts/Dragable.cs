using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform parent;
    private Vector2 initialPosition;
    private bool doMoveUp = true;
    private Rigidbody2D rigid2D;
    public float speed = 1.0f;

    private Vector2 startParentPosition;
    private Vector2 defaultMyPosition;

    private void Awake()
    {
        parent = gameObject.transform.parent.GetComponent<RectTransform>();
        initialPosition = parent.anchoredPosition;
        rigid2D = parent.GetComponent<Rigidbody2D>();
        defaultMyPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var gap = eventData.pressPosition - eventData.position;

        var currentParentPosition = new Vector3(startParentPosition.x, startParentPosition.y - gap.y, 0);

        if ((initialPosition.y > currentParentPosition.y) && (-initialPosition.y < currentParentPosition.y))
        {
            parent.anchoredPosition = currentParentPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startParentPosition = parent.anchoredPosition;
        
        rigid2D.velocity = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MoveMagnetic();
    }

    private void SetMoveUp(bool set)
    {
        doMoveUp = set;
    }

    private void MoveMagnetic()
    {
        if (doMoveUp)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = defaultMyPosition;
            if (initialPosition.y > parent.anchoredPosition.y)
            {
                rigid2D.velocity = Vector2.up * speed;
            }
        }
        else
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultMyPosition.x, -defaultMyPosition.y);
            if (-initialPosition.y < parent.anchoredPosition.y)
            {
                rigid2D.velocity = -(Vector2.up * speed);
            }
        }
    }

    private void Break()
    {
        rigid2D.velocity = Vector2.zero;
    }
}
