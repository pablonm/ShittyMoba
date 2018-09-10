using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSFX : Photon.PunBehaviour
{

    public AudioClip basic;
    public AudioClip q;
    public AudioClip w;
    public AudioClip e;
    public AudioClip r;
    public AudioClip freeze;
    public AudioClip stun;
    public AudioClip trap;

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> audioDictionary;

    private void Start ()
    {
        audioDictionary = new Dictionary<string, AudioClip>();
        audioDictionary.Add("basic", basic);
        audioDictionary.Add("q", q);
        audioDictionary.Add("w", w);
        audioDictionary.Add("e", e);
        audioDictionary.Add("r", r);
        audioDictionary.Add("freeze", freeze);
        audioDictionary.Add("stun", stun);
        audioDictionary.Add("trap", trap);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound, bool loop)
    {
        photonView.RPC("PlaySoundRPC", PhotonTargets.All, sound, loop);
    }

    [PunRPC]
    public void PlaySoundRPC(string sound, bool loop)
    {
        if (audioDictionary.ContainsKey(sound) && audioDictionary[sound] != null) {
            audioSource.loop = loop;
            audioSource.clip = audioDictionary[sound];
            audioSource.Play();
        }
    }

    public void Stop(string sound)
    {
        photonView.RPC("StopRPC", PhotonTargets.All, sound);
    }

    [PunRPC]
    public void StopRPC(string sound)
    {
        if (audioDictionary.ContainsKey(sound) && audioDictionary[sound] != null)
        {
            if (audioSource.clip == audioDictionary[sound])
            {
                audioSource.Stop();
            }
        }
    }
}
