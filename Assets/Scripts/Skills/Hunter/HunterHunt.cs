using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterHunt : Skill
{
    public float speedMultiplier = 1.2f;
    public float basicAttackSpeedMultiplier = 1.2f;
    public float time = 5;
    public StatusController statusController;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        statusController.ApplySpeed(speedMultiplier, time);
        statusController.ApplyBasicAttackSpeed(basicAttackSpeedMultiplier, time);
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
