using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormOfSwords : Photon.PunBehaviour {

    public int hits = 5;
    public float interval = 1;
    public float damagePerHit = 10;

    [HideInInspector]
    public GameObject owner;

    private BoxCollider2D coll;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        StartCoroutine(Storm());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damagePerHit);
        }
    }

    private IEnumerator Storm()
    {
        for (int i = 0; i < hits; i++)
        {
            coll.enabled = true;
            yield return new WaitForSeconds(0.1f);
            coll.enabled = false;
            yield return new WaitForSeconds(interval - 0.1f);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (photonView.isMine && owner)
        {
            transform.position = owner.transform.position;
        }
    }
}
