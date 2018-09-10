using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    public float healAmount = 10;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = transform.position.z;
        Collider2D hitCollider = Physics2D.OverlapCircle(clickPos, 0.5F, LayerMask.GetMask("Character"));
        if (hitCollider && !hitCollider.gameObject.CompareTag("Enemy"))
        {
            hitCollider.GetComponent<StatusController>().ApplyHeal(healAmount);
            return true;
        }
        return false;
    }

    public override void PrepareBehaviour()
    {
        // Nothing
    }

    public override void StartBehaviour()
    {
        // Nothing
    }

    public override void UpdateBehaviour()
    {
        // Nothing
    }
}
