using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour
{
    public Transform blueSpawn;
    public Transform redSpawn;
    public GameObject mainCamera;
    private GameObject localPlayer;


    public IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        photonView.RPC("StartGame", PhotonTargets.All, null);
        yield break;
    }

    [PunRPC]
    public void StartGame()
    {
        var players = FindObjectsOfType<HealthController>();
        for (int i = 0; i < players.Length; i++)
        {
            if ((string)players[i].photonView.owner.CustomProperties["team"] != (string)PhotonNetwork.player.CustomProperties["team"])
            {
                players[i].gameObject.tag = "Enemy";
            }
        }
        
    }

    private void Start()
    {
        localPlayer = PhotonNetwork.Instantiate(
            "Chars/" + (string)PhotonNetwork.player.CustomProperties["character"],
            ((string)PhotonNetwork.player.CustomProperties["team"] == "blue")? blueSpawn.transform.position : redSpawn.transform.position,
            Quaternion.identity,
            0);
        mainCamera.GetComponent<CameraFollow>().SetTarget(localPlayer.transform);
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(DelayedStart());
        }
    }

    public void ResetPosition()
    {
        photonView.RPC("ResetPositionRPC", PhotonTargets.All, null);
    }

    [PunRPC]
    public void ResetPositionRPC()
    {
        localPlayer.transform.position = ((string)PhotonNetwork.player.CustomProperties["team"] == "blue") ? blueSpawn.transform.position : redSpawn.transform.position;
    }

    public void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }

}
