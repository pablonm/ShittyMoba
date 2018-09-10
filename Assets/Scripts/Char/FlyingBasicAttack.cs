using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBasicAttack : Photon.PunBehaviour {

    public Transform target;
    public GameObject owner;
    public float speed = 1;

    [HideInInspector]
    public float damage = 1;
    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        if (target && owner)
        {
            if (target.GetComponent<HealthController>().isAlive)
            {
                transform.right = target.position - transform.position;
                rb.velocity = (target.position - transform.position).normalized * speed;
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && owner)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage * owner.GetComponent<StatusController>().damageMultiplier);
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls") && photonView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
