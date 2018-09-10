using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesDance : Skill
{
    public int hits;
    public float intervalSeconds;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        transform.parent.parent.GetComponent<NavChar>().cancelarMovimiento();
        transform.parent.parent.GetComponent<NavChar>().enabled = false;
        transform.parent.parent.GetComponent<BasicAttackController>().enabled = false;
        transform.parent.parent.GetComponent<SkillController>().enabled = false;
        StartCoroutine(EnableMovement());
        return true;
    }

    private IEnumerator EnableMovement()
    {
        for (int i = 0; i < hits; i++)
        {
            yield return new WaitForSeconds(intervalSeconds);
        }
        transform.parent.parent.GetComponent<NavChar>().enabled = true;
        transform.parent.parent.GetComponent<BasicAttackController>().enabled = true;
        transform.parent.parent.GetComponent<SkillController>().enabled = true;
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
