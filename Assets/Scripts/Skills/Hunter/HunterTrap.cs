using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterTrap : Photon.PunBehaviour
{
    public float trapTime;
    public float trapTimeoutTime;

    private CircleCollider2D collider;
    private Coroutine timeoutCoroutine;
    
	void Start ()
    {
        collider = GetComponent<CircleCollider2D>();
        if (photonView.isMine)
        {
            timeoutCoroutine = StartCoroutine(DestroyAfter(trapTimeoutTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            StopCoroutine(timeoutCoroutine);
            collider.enabled = false;
            collision.gameObject.GetComponent<StatusController>().ApplyTrap(transform.position, trapTime);
            ChangeColor();
            StartCoroutine(DestroyAfter(trapTime));
        }
    }

    private void ChangeColor()
    {
        photonView.RPC("ChangeColorRPC", PhotonTargets.All, null);
    }

    [PunRPC]
    private void ChangeColorRPC()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    private IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }

}
