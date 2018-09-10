using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claws : Photon.PunBehaviour {

    public float damage;

    public void Start()
    {
        StartCoroutine(DestroyAfterSeconds(0.25f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(transform.parent.gameObject);
    }
}
