using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterChargedArrow : Photon.PunBehaviour
{

    public float speed = 1;
    public float damage = 10;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && collision.gameObject.CompareTag("Enemy") && photonView.isMine)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
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