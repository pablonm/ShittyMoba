using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedField : Photon.PunBehaviour
{
    public float seconds = 5;
    public float slowSeconds = 1;
    public float slowMultiplier = 0.3f;
    public float poisonDamage = 5f;
    public float poisonSeconds = 3f;

    private BoxCollider2D coll;

	void Start ()
    {
        coll = GetComponent<BoxCollider2D>();
        StartCoroutine(DestroyAfterSeconds(seconds));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<StatusController>().ApplySlow(slowMultiplier, slowSeconds);
            collision.gameObject.GetComponent<StatusController>().ApplyPoison(poisonDamage, poisonSeconds);
        }
    }

    private IEnumerator DestroyAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
