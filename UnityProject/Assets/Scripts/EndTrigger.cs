using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CallSpaceShipEnd();
    }

    public void CallSpaceShipEnd()
    {
        SpaceShip.instance.StartWin();
    }
}
