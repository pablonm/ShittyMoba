using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesDanceProjectile : Photon.PunBehaviour
{
    public int hits;
    public float interval;
    public float damagePerHit;
    public float slowSeconds;
    public float slowMultiplier;

    private PolygonCollider2D coll;
    private AudioSource audioSource;

    void Start()
    {
        coll = GetComponent<PolygonCollider2D>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Dance());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damagePerHit);
            collision.gameObject.GetComponent<StatusController>().ApplySlow(slowMultiplier, slowSeconds);
        }
    }

    private IEnumerator Dance()
    {
        for (int i = 0; i < hits; i++)
        {
            coll.enabled = true;
            audioSource.Play();
            yield return new WaitForSeconds(0.1f);
            coll.enabled = false;
            yield return new WaitForSeconds(interval - 0.1f);
        }
        Destroy(gameObject);
    }

}

