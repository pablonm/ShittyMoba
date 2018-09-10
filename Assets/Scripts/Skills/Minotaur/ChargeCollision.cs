using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeCollision : Photon.PunBehaviour
{
    public float stunSeconds;
    public float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            collision.gameObject.GetComponent<StatusController>().ApplyStun(stunSeconds);
        }
    }
}
