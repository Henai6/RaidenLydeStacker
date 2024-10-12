using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    public UnityEvent WhenTriggered;


    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        WhenTriggered?.Invoke();
    }
}
