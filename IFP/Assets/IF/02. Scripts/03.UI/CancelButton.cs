using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour {

    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private List<GameObject> menuPanels = new List<GameObject>(5);

    private Button myButton;

    private Stack<GameObject> menuStack;

    private void Awake()
    {
        myButton = gameObject.GetComponent<Button>();
        menuStack = new Stack<GameObject>();

        menuPanels.ForEach(a =>
        {
            a.AddComponent<NotifyCancelButton>().Set(NotifyMovePanel);
        });

        NotifyMovePanel(mainMenuPanel);

        gameObject.SetActive(false);
    }

    public void NotifyMovePanel(GameObject activateGameObject)
    {
        //var activateGameObject = menuPanels.Where(a => a.activeSelf).FirstOrDefault();

        if ((activateGameObject == mainMenuPanel) || (menuStack.Count == 0))
        {
            menuStack.Push(mainMenuPanel);
            return;
        }
        var checkSample = menuStack.Pop();
        if (activateGameObject == checkSample)
        {
            menuStack.Push(checkSample);
            return;
        }
        else
        {
            menuStack.Push(checkSample);
        }

        menuStack.Push(activateGameObject);

        gameObject.SetActive(true);
    }

    public void MoveBack()
    {
        var currentMenu = menuStack.Pop();
        var beforeMenu = menuStack.Pop();

        currentMenu.SetActive(false);
        beforeMenu.SetActive(true);

        if (beforeMenu == mainMenuPanel)
        {
            gameObject.SetActive(false);
        }
    }
}
