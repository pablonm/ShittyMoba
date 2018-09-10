using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : Photon.PunBehaviour {

    public float slowMultipier;
    public float slowSeconds;

    private CircleCollider2D coll;

    public void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        StartCoroutine(DestroyAfterSeconds(0.25f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<StatusController>().ApplySlow(slowMultipier, slowSeconds);
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        coll.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
