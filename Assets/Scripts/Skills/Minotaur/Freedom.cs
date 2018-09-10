using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freedom : Skill
{
    public GameObject effect;

    public override void CancelBehavirour()
    {
        // Nothing
    }

    public override bool CastBehavirour()
    {
        // Nothing
        transform.parent.parent.GetComponent<StatusController>().CleanAllNegative();
        PhotonNetwork.Instantiate(
                "Skills/" + effect.name,
                transform.parent.parent.position + new Vector3(0, 2.5f, 0),
                Quaternion.identity,
                0);
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
