using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncAnimator : Photon.PunBehaviour
{
    public Animator anim;

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

    public void SetBool(string boolean, bool value)
    {
        photonView.RPC("SetBoolRPC", PhotonTargets.All, boolean, value);
    }

    [PunRPC]
    public void SetBoolRPC(string boolean, bool value)
    {
        if (anim)
        {
            anim.SetBool(boolean, value);
        }
    }

}
