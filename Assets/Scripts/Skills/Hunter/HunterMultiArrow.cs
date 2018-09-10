using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMultiArrow : Photon.PunBehaviour {

    public float damage = 10;
    public float knockBack = 10;

    private PolygonCollider2D rb;

    void Start()
    {
        rb = GetComponent<PolygonCollider2D>();
        StartCoroutine(DisableRB());
    }

    private IEnumerator DisableRB()
    {
        yield return new WaitForSeconds(0.5f);
        rb.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            collision.gameObject.GetComponent<StatusController>().ApplyForce(((Vector2)collision.gameObject.transform.position - (Vector2)transform.position).normalized * knockBack);
        }
    }
}
