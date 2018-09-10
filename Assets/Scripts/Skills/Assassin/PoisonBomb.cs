using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBomb : Photon.PunBehaviour
{
    public float poisonSeconds = 5;
    public float poisonDamage = 0.5f;

    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(0.25f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<StatusController>().ApplyPoison(poisonDamage, poisonSeconds);
        }
    }

    private IEnumerator DestroyAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
