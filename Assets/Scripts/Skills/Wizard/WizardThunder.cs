using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardThunder : Photon.PunBehaviour
{
    public float damage = 10;
    public float freezeDamageMultiplier = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage((collision.gameObject.GetComponent<StatusController>().freeze) ? damage * freezeDamageMultiplier : damage);
        }
    }
}
