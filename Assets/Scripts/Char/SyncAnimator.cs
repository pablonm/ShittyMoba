using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncAnimator : Photon.PunBehaviour
{
    private Animator anim;

	private void Start ()
    {
        anim = GetComponent<Animator>();
	}

    public void SetTrigger(string trigger)
    {
        photonView.RPC("SetTriggerRPC", PhotonTargets.All, trigger);
    }

    [PunRPC]
    public void SetTriggerRPC(string trigger)
    {
        if (anim)
        {
            anim.SetTrigger(trigger);
        }
    }

}
