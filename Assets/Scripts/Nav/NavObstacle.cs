using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavObstacle : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NavPunto>())
        {
            NavPunto p = collision.GetComponent<NavPunto>();
            p.blocked = true;
        }
    }
}
