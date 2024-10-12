using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnAnimation : MonoBehaviour
{
    public SpaceShip SpaceShip;

    public void CallSpaceShip()
    {
        SpaceShip.MoveAnim();
    }

    public void EndOfWinAnimation()
    {
        SpaceShip.Win();
    }
}
