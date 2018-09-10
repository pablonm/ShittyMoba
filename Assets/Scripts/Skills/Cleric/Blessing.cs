using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    public float seconds;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        StatusController[] charStatus = FindObjectsOfType<StatusController>();
        for (int i = 0; i < charStatus.Length; i++)
        {
            if (!charStatus[i].gameObject.CompareTag("Enemy"))
            {
                charStatus[i].ApplyBless(seconds);
            }
        }
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
