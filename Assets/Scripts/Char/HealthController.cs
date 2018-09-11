using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : Photon.PunBehaviour
{

    public int totalHealth;
    public Image healthBar;

    [HideInInspector]
    public bool isAlive = true;

    private float currentHealth;
    private SyncAnimator syncAnimator;
    private SkillController skillController;
    private BasicAttackController basicAttackController;
    private CapsuleCollider2D coll;
    private StatusController statusController;
    private BattleArena battleArena;

    private void Start()
    {
        currentHealth = totalHealth;
        syncAnimator = GetComponent<SyncAnimator>();
        skillController = GetComponent<SkillController>();
        basicAttackController = GetComponent<BasicAttackController>();
        coll = GetComponent<CapsuleCollider2D>();
        statusController = GetComponent<StatusController>();
        battleArena = FindObjectOfType<BattleArena>();
    }

    public void TakeDamage(float amount)
    {
        photonView.RPC("TakeDamageRPC", PhotonTargets.All, amount);
    }

    [PunRPC]
    public void TakeDamageRPC(float amount)
    {
        // Special conditions
        if (statusController.freeze)
        {
            statusController.CleanFreeze();
        }
        if (statusController.parry)
        {
            statusController.ParryHit();
            return;
        }
        if (statusController.invisible)
        {
            statusController.CleanInvisible();
            return;
        }

        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (currentHealth == 0 && isAlive)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        photonView.RPC("HealRPC", PhotonTargets.All, amount);
    }

    [PunRPC]
    public void HealRPC(float amount)
    {
        currentHealth += amount;
        if (currentHealth > totalHealth)
        {
            currentHealth = totalHealth;
        }
    }

    private void Die()
    {
        isAlive = false;
        statusController.CleanAll();
        syncAnimator.SetTrigger("death");
        skillController.enabled = false;
        basicAttackController.CancelAttack();
        basicAttackController.enabled = false;
        coll.enabled = false;
        if (PhotonNetwork.isMasterClient)
        {
            battleArena.CheckGame();
        }
    }

    public void Revive()
    {
        photonView.RPC("ReviveRPC", PhotonTargets.All, null);
    }

    [PunRPC]
    public void ReviveRPC()
    {
        isAlive = true;
        syncAnimator.SetTrigger("idle");
        statusController.CleanAll();
        currentHealth = totalHealth;
        skillController.enabled = true;
        basicAttackController.enabled = true;
        coll.enabled = true;
    }

    void Update ()
    {
        healthBar.fillAmount = currentHealth / totalHealth;
	}
}
