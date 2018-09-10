using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloak : Skill
{
    public float seconds;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        transform.parent.parent.GetComponent<StatusController>().ApplyInvisible(seconds);
        return true;
    }

    public override void PrepareBehaviour()
    {
        // Nothing
    }

    public override void UpdateBehaviour()
    {
        
    }

    public override void StartBehaviour()
    {
        // Nothing
    }
}
