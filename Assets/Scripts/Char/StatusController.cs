using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusController : Photon.PunBehaviour {

    public GameObject effects;
    public GameObject canvas;

    public GameObject freezeEffect;
    public GameObject stunEffect;
    public GameObject poisonEffect;
    public GameObject slowEffect;
    public GameObject speedEffect;
    public GameObject blessEffect;
    public GameObject parryEffect;
    public GameObject healEffect;

    [HideInInspector]
    public bool freeze;

    [HideInInspector]
    public bool stun;

    [HideInInspector]
    public bool poison;

    [HideInInspector]
    public bool trap;

    [HideInInspector]
    public bool slow;

    [HideInInspector]
    public bool speed;

    [HideInInspector]
    public bool basicAttackSpeed;

    [HideInInspector]
    public bool bless;

    [HideInInspector]
    public bool parry;

    [HideInInspector]
    public bool invisible;

    [HideInInspector]
    public float damageMultiplier = 1;

    private Coroutine freezeCoroutine;
    private Coroutine stunCoroutine;
    private Coroutine poisonCoroutine;
    private Coroutine poisonDamageCoroutine;
    private Coroutine trapCoroutine;
    private Coroutine slowCoroutine;
    private Coroutine speedCoroutine;
    private Coroutine basicAttackSpeedCoroutine;
    private Coroutine blessCoroutine;
    private Coroutine parryCoroutine;
    private Coroutine invisibleCoroutine;
    private Coroutine healCoroutine;

    private NavChar navChar;
    private BasicAttackController basicAttackController;
    private SkillController skillController;
    private HealthController healthController;
    private SpriteRenderer sprite;
    private SyncAnimator syncAnimator;
    private Rigidbody2D rb;
    private SyncSFX sfx;

    private float originalWalkingSpeed;
    private float originalBasicAttackSpeed;
    private int parryHitsLeft;

    private void Start()
    {
        navChar = GetComponent<NavChar>();
        basicAttackController = GetComponent<BasicAttackController>();
        skillController = GetComponent<SkillController>();
        healthController = GetComponent<HealthController>();
        sprite = GetComponent<SpriteRenderer>();
        syncAnimator = GetComponent<SyncAnimator>();
        sfx = GetComponent<SyncSFX>();
        originalWalkingSpeed = navChar.speed;
        originalBasicAttackSpeed = basicAttackController.debounceTime;
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyFreeze(float time)
    {
        photonView.RPC("ApplyFreezeRPC", PhotonTargets.All, time);
    }

    [PunRPC]
    public void ApplyFreezeRPC(float time)
    {
        sfx.PlaySoundRPC("freeze", false);
        CleanStun();
        freeze = true;
        freezeEffect.SetActive(true);
        basicAttackController.CancelAttack();
        basicAttackController.enabled = false;
        skillController.enabled = false;
        freezeCoroutine = StartCoroutine(DelayedTask(CleanFreeze, time));
    }

    public void CleanFreeze()
    {
        if (freeze)
        {
            if (stunCoroutine != null)
            {
                StopCoroutine(freezeCoroutine);
            }
            freezeCoroutine = null;
            freeze = false;
            basicAttackController.enabled = true;
            skillController.enabled = true;
            freezeEffect.SetActive(false);
        }
    }

    public void ApplyStun(float time)
    {
        photonView.RPC("ApplyStunRPC", PhotonTargets.All, time);
    }

    [PunRPC]
    public void ApplyStunRPC(float time)
    {
        sfx.PlaySoundRPC("stun", true);
        CleanFreeze();
        stun = true;
        stunEffect.SetActive(true);
        basicAttackController.CancelAttack();
        basicAttackController.enabled = false;
        skillController.enabled = false;
        stunCoroutine = StartCoroutine(DelayedTask(CleanStun, time));
    }

    public void CleanStun()
    {
        if (stun)
        {
            sfx.StopRPC("stun");
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
            }
            stunCoroutine = null;
            stun = false;
            basicAttackController.enabled = true;
            skillController.enabled = true;
            stunEffect.SetActive(false);
        }
    }

    public void ApplyPoison(float dmgPerSecond, float seconds)
    {
        photonView.RPC("ApplyPoisonRPC", PhotonTargets.All, dmgPerSecond, seconds);
    }

    [PunRPC]
    public void ApplyPoisonRPC(float dmgPerSecond, float seconds)
    {
        poison = true;
        poisonEffect.SetActive(true);
        if (photonView.isMine)
        {
            poisonDamageCoroutine = StartCoroutine(PoisonDamage(dmgPerSecond, seconds));
        }
        poisonCoroutine = StartCoroutine(DelayedTask(CleanPoison, seconds));
    }

    public void CleanPoison()
    {
        if (poison)
        {
            if (poisonCoroutine != null)
            {
                StopCoroutine(poisonCoroutine);
            }
            if (poisonDamageCoroutine != null)
            {
                StopCoroutine(poisonDamageCoroutine);
            }
            poisonCoroutine = null;
            poison = false;
            poisonEffect.SetActive(false);
        }
    }

    private IEnumerator PoisonDamage(float dmgPerSecond, float seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            healthController.TakeDamage(dmgPerSecond);
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }

    public void ApplyTrap(Vector2 position, float seconds)
    {
        photonView.RPC("ApplyTrapRPC", PhotonTargets.All, position, seconds);
    }

    [PunRPC]
    public void ApplyTrapRPC(Vector2 position, float seconds)
    {
        sfx.PlaySoundRPC("trap", false);
        trap = true;
        syncAnimator.SetTrigger("stop");
        if (navChar.enabled)
        {
            navChar.cancelarMovimiento();
        }
        navChar.enabled = false;
        navChar.gameObject.transform.position = position;
        trapCoroutine = StartCoroutine(DelayedTask(CleanTrap, seconds));
    }

    public void CleanTrap()
    {
        if (trap)
        {
            if (trapCoroutine != null)
            {
                StopCoroutine(trapCoroutine);
            }
            navChar.enabled = true;
            trapCoroutine = null;
            trap = false;
        }
    }

    public void ApplySlow(float multiplier, float seconds)
    {
        photonView.RPC("ApplySlowRPC", PhotonTargets.All, multiplier, seconds);
    }

    [PunRPC]
    public void ApplySlowRPC(float multiplier, float seconds)
    {
        CleanSpeed();
        slow = true;
        navChar.speed = originalWalkingSpeed * multiplier;
        slowEffect.SetActive(true);
        slowCoroutine = StartCoroutine(DelayedTask(CleanSlow, seconds));
    }

    public void CleanSlow()
    {
        if (slow)
        {
            if (slowCoroutine != null)
            {
                StopCoroutine(slowCoroutine);
            }
            navChar.speed = originalWalkingSpeed;
            slowCoroutine = null;
            slow = false;
            slowEffect.SetActive(false);
        }
    }

    public void ApplySpeed(float multiplier, float seconds)
    {
        photonView.RPC("ApplySpeedRPC", PhotonTargets.All, multiplier, seconds);
    }

    [PunRPC]
    public void ApplySpeedRPC(float multiplier, float seconds)
    {
        CleanSlow();
        speed = true;
        navChar.speed = originalWalkingSpeed * multiplier;
        speedEffect.SetActive(true);
        speedCoroutine = StartCoroutine(DelayedTask(CleanSpeed, seconds));
    }

    public void CleanSpeed()
    {
        if (speed)
        {
            if (speedCoroutine != null)
            {
                StopCoroutine(speedCoroutine);
            }
            navChar.speed = originalWalkingSpeed;
            speedCoroutine = null;
            speed = false;
            speedEffect.SetActive(false);
        }
    }

    public void ApplyBasicAttackSpeed(float multiplier, float seconds)
    {
        photonView.RPC("ApplyBasicAttackSpeedRPC", PhotonTargets.All, multiplier, seconds);
    }

    [PunRPC]
    public void ApplyBasicAttackSpeedRPC(float multiplier, float seconds)
    {
        basicAttackSpeed = true;
        basicAttackController.debounceTime /= multiplier;
        basicAttackSpeedCoroutine = StartCoroutine(DelayedTask(CleanBasicAttackSpeed, seconds));
    }

    public void CleanBasicAttackSpeed()
    {
        if (basicAttackSpeed)
        {
            if (basicAttackSpeedCoroutine != null)
            {
                StopCoroutine(basicAttackSpeedCoroutine);
            }
            basicAttackController.debounceTime = originalBasicAttackSpeed;
            basicAttackSpeedCoroutine = null;
            basicAttackSpeed = false;
        }
    }

    public void ApplyBless(float seconds)
    {
        photonView.RPC("ApplyBlessRPC", PhotonTargets.All, seconds);
    }

    [PunRPC]
    public void ApplyBlessRPC(float seconds)
    {
        CleanAllRPC();
        bless = true;
        navChar.speed = originalWalkingSpeed * 1.5f;
        damageMultiplier = 1.5f;
        blessEffect.SetActive(true);
        blessCoroutine = StartCoroutine(DelayedTask(CleanBless, seconds));
    }

    public void CleanBless()
    {
        if (bless)
        {
            if (blessCoroutine != null)
            {
                StopCoroutine(blessCoroutine);
            }
            navChar.speed = originalWalkingSpeed;
            damageMultiplier = 1;
            blessCoroutine = null;
            bless = false;
            blessEffect.SetActive(false);
        }
    }

    public void ApplyParry(float seconds, int parryHits)
    {
        photonView.RPC("ApplyParryRPC", PhotonTargets.All, seconds, parryHits);
    }

    [PunRPC]
    public void ApplyParryRPC(float seconds, int parryHits)
    {
        parry = true;
        parryEffect.SetActive(true);
        parryHitsLeft = parryHits;
        parryCoroutine = StartCoroutine(DelayedTask(CleanParry, seconds));
    }

    public void CleanParry()
    {
        if (parry)
        {
            if (parryCoroutine != null)
            {
                StopCoroutine(parryCoroutine);
            }
            parryHitsLeft = 0;
            parryCoroutine = null;
            parry = false;
            parryEffect.SetActive(false);
        }
    }

    public void ParryHit()
    {
        parryHitsLeft--;
        if (parryHitsLeft <= 0)
        {
            CleanParry();
        }
    }

    public void ApplyInvisible(float seconds)
    {
        photonView.RPC("ApplyInvisibleRPC", PhotonTargets.All, seconds);
    }

    [PunRPC]
    public void ApplyInvisibleRPC(float seconds)
    {
        invisible = true;
        if (photonView.isMine)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
        }
        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
            effects.SetActive(false);
            canvas.SetActive(false);
        }
        invisibleCoroutine = StartCoroutine(DelayedTask(CleanInvisible, seconds));
    }

    public void CleanInvisible()
    {
        if (invisible)
        {
            if (invisibleCoroutine != null)
            {
                StopCoroutine(invisibleCoroutine);
            }
            invisibleCoroutine = null;
            invisible = false;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
            effects.SetActive(true);
            canvas.SetActive(true);
        }
    }

    public void ApplyHeal(float amount)
    {
        photonView.RPC("ApplyHealRPC", PhotonTargets.All, amount);
    }

    [PunRPC]
    public void ApplyHealRPC(float amount)
    {
        healEffect.SetActive(true);
        if (photonView.isMine)
        {
            healthController.Heal(amount);
        }
        healCoroutine = StartCoroutine(DelayedTask(CleanHeal, 2));
    }

    public void CleanHeal()
    {
        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
        }
        healCoroutine = null;
        healEffect.SetActive(false);
    }

    public void ApplyForce(Vector2 force)
    {
        photonView.RPC("ApplyForceRPC", PhotonTargets.All, force);
    }

    [PunRPC]
    public void ApplyForceRPC(Vector2 force)
    {
        if (navChar.enabled)
        {
            navChar.cancelarMovimiento();
        }
        navChar.enabled = false;
        rb.AddForce(force, ForceMode2D.Impulse);
        navChar.enabled = true;
    }

    public void CleanAll()
    {
        photonView.RPC("CleanAllRPC", PhotonTargets.All, null);
    }

    [PunRPC]
    public void CleanAllRPC()
    {
        CleanFreeze();
        CleanStun();
        CleanPoison();
        CleanSlow();
        CleanSpeed();
        CleanBless();
        CleanParry();
        CleanInvisible();
    }

    public void CleanAllNegative()
    {
        photonView.RPC("CleanAllNegativeRPC", PhotonTargets.All, null);
    }

    [PunRPC]
    public void CleanAllNegativeRPC()
    {
        CleanFreeze();
        CleanStun();
        CleanPoison();
        CleanSlow();
    }

    private IEnumerator DelayedTask(UnityAction callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback();
        yield break;
    }
	

}
