using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackController : Photon.PunBehaviour {

    public enum ATTACKTYPE { Ranged, Mele }

    public ATTACKTYPE attackType;
    public float damage;
    public float maxDistance = 3f;
    public float debounceTime = 1f;
    public GameObject attackPrefab;
    public Transform basicAttackPoint;

    private bool debouncing = false;
    private bool attacking = false;
    private Collider2D target = null;
    private SyncAnimator syncAnimator;
    private NavChar navChar;
    private SyncSFX sfx;


    private void Start()
    {
        syncAnimator = GetComponent<SyncAnimator>();
        navChar = GetComponent<NavChar>();
        sfx = GetComponent<SyncSFX>();
    }

    private void Update ()
    {
        if (photonView.isMine)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                GetTargetOrWalk();
            }
            direccionar();
        }
    }

    private void GetTargetOrWalk()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = transform.position.z;
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(clickPos, 0.5F, LayerMask.GetMask("Character"));

        target = null;
        for (int i = 0; i < hitCollider.Length; i++)
        {
            if (hitCollider[i] && hitCollider[i].gameObject.CompareTag("Enemy"))
            {
                target = hitCollider[i];
                break;
            }
        }

        if (!target)
        {
            if (navChar.enabled)
            {
                attacking = false;
                debouncing = false;
                StopAllCoroutines();
                navChar.irHaciaClick();
            }
        }
        else
        {
            if (Vector2.Distance(target.transform.position, transform.position) <= maxDistance)
            {
                Attack();
            }
            else
            {
                attacking = false;
                debouncing = false;
                StopAllCoroutines();
                navChar.irHaciaClick(Attack);
            }
        }
    }

    private void Attack()
    {
        if (!debouncing && Vector2.Distance(target.transform.position, transform.position) <= maxDistance)
        {
            syncAnimator.SetTrigger("attack");
            sfx.PlaySound("basic", false);

            if (attackType == ATTACKTYPE.Ranged)
            {
                if (attackPrefab)
                {
                    GameObject instancedAttack = PhotonNetwork.Instantiate("Skills/" + attackPrefab.name, basicAttackPoint.position, Quaternion.identity, 0);
                    instancedAttack.GetComponent<FlyingBasicAttack>().target = target.transform;
                    instancedAttack.GetComponent<FlyingBasicAttack>().owner = gameObject;
                    instancedAttack.GetComponent<FlyingBasicAttack>().damage = damage;
                    instancedAttack.GetComponent<FlyingBasicAttack>().enabled = true;
                }
            }
            else
            {
                target.GetComponent<HealthController>().TakeDamage(damage);
            }
            attacking = true;
            StartCoroutine(DebounceAttack());
        }
    }

    private IEnumerator DebounceAttack()
    {
        debouncing = true;
        yield return new WaitForSeconds(debounceTime);
        debouncing = false;
        if (attacking)
        {
            if (target.GetComponent<HealthController>().isAlive)
            {
                Attack();
            }
            else
            {
                attacking = false;
                target = null;
            }
        }
    }

    private void direccionar()
    {
        if (!attacking)
        {
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= transform.position.x)
            {
                direccionarDerecha();
            }
            else
            {
                direccionarIzquierda();
            }
        }
    }

    public void direccionarDerecha()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z));
    }

    public void direccionarIzquierda()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z));
    }

    public void CancelAttack()
    {
        attacking = false;
    }

}
