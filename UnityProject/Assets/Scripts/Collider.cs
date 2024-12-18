using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy")
        {
            Destroy(collision.gameObject);
            SpaceShip.TookDmg();
            Destroy(this.gameObject);
        }
    }

}
