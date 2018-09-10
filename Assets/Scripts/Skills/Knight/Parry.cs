using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Skill
{
    public int parryHits;
    public float seconds;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        transform.parent.parent.GetComponent<StatusController>().ApplyParry(seconds, parryHits);
        return true;
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
