using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartingHelper : MonoBehaviour
{
    public GameObject ToDisable;
    public GameObject ToEnable;
    public GameObject EnabledButton;
    public EventSystem eventSystem;


    // Start is called before the first frame update
    void Start()
    {
        if (ChangeScene.startFrom == StartFrom.end)
        {
            ToDisable.SetActive(false);
            ToEnable.SetActive(true);
            eventSystem.firstSelectedGameObject = EnabledButton;
        }
    }
}
