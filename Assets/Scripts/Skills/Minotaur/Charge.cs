using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Skill
{
    public float impulse;
    public float chargeSeconds;

    private ConstantForce2D constantForce;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        transform.parent.parent.gameObject.layer = LayerMask.NameToLayer("OwnCharacter");
        transform.parent.parent.GetComponent<NavChar>().cancelarMovimiento();
        transform.parent.parent.GetComponent<BasicAttackController>().enabled = false;
        transform.parent.parent.GetComponent<SkillController>().enabled = false;
        constantForce = transform.parent.parent.gameObject.AddComponent<ConstantForce2D>();
        constantForce.relativeForce = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * impulse;
        StartCoroutine(RemoveConstantForce());
        return true;
    }

    private IEnumerator RemoveConstantForce()
    {
        yield return new WaitForSeconds(chargeSeconds);
        transform.parent.parent.gameObject.layer = LayerMask.NameToLayer("Character");
        transform.parent.parent.GetComponent<BasicAttackController>().enabled = true;
        transform.parent.parent.GetComponent<SkillController>().enabled = true;
        DestroyImmediate(constantForce);
    }

    public override void PrepareBehaviour()
    {
        // Nothing
    }

    public override void UpdateBehaviour()
    {
        // Nothing
    }

    public override void StartBehaviour()
    {
        // Nothing
    }
}
