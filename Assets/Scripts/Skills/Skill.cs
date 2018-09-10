using System.Collections;
using UnityEngine;

public abstract class Skill : Photon.PunBehaviour
{
    public bool controlUI;
    public string key;
    public Sprite icon;
    public float cooldownSeconds;
    public string skillName;
    public float range = float.MaxValue;

    [HideInInspector]
    public bool preparing;

    private bool inCooldown = false;
    private SkillBar skillbar;

    void Start()
    {
        if (controlUI && photonView.isMine)
        {
            skillbar = FindObjectOfType<SkillBar>();
            skillbar.InitializeSkill(key, icon);
        }
        StartBehaviour();
    }

    void Update () {
        if (preparing && !inCooldown && Input.GetButtonDown("Fire1"))
        {
            Cast();
        }
        UpdateBehaviour();
    }

    public void Prepare()
    {
        if (!preparing && !inCooldown)
        {
            preparing = true;
            PrepareBehaviour();
        }
    }

    public void Cancel()
    {
        if (preparing)
        {
            preparing = false;
            CancelBehavirour();
        }
    }
    
    public void Cast()
    {
        if (!inCooldown)
        {
            preparing = false;
            if (CastBehavirour())
            {
                transform.parent.parent.GetComponent<SyncAnimator>().SetTrigger("attack");
                transform.parent.parent.GetComponent<SyncSFX>().PlaySound(key.ToLower(), false);
                if (controlUI && photonView.isMine)
                {
                    skillbar.StartCooldown(key, cooldownSeconds);
                }
            }
        }
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        inCooldown = false;
        yield break;
    }

    public abstract void StartBehaviour();

    public abstract void PrepareBehaviour();

    public abstract void CancelBehavirour();

    public abstract bool CastBehavirour();

    public abstract void UpdateBehaviour();

}
