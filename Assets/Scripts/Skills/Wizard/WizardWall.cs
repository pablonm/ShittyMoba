using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardWall : MonoBehaviour
{
    private List<NavPunto> puntosBloqueados;

	void Start ()
    {
        puntosBloqueados = new List<NavPunto>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NavPunto>())
        {
            NavPunto p = collision.GetComponent<NavPunto>();
            p.blocked = true;
            puntosBloqueados.Add(p);
        }
    }

    private void OnDestroy()
    {
        foreach (NavPunto p in puntosBloqueados)
        {
            p.blocked = false;
        }
    }

}
