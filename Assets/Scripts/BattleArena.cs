using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleArena : Photon.PunBehaviour
{
    public Text bluePointsText;
    public Text redPointsText;
    public Text resultText;
    public int maxPoints = 5;
    public GameManager gameManager;

    private int bluePoints;
    private int redPoints;

    private void Start ()
    {
        bluePointsText.text = "0";
        redPointsText.text = "0";
    }

    public void CheckGame()
    {
        HealthController[] healths = FindObjectsOfType<HealthController>();
        int blueAlive = 0;
        int redAlive = 0;
        for (var i = 0; i < healths.Length; i++)
        {
            if (healths[i].isAlive)
            {
                if ((string)healths[i].photonView.owner.CustomProperties["team"] == "blue")
                {
                    blueAlive++;
                }
                else
                {
                    redAlive++;
                }
            }
        }
        if (blueAlive == 0)
        {
            IncresePoints("red");
        }
        else
        {
            if (redAlive == 0)
            {
                IncresePoints("blue");
            }

        }
    }

    public void IncresePoints(string team)
    {
        photonView.RPC("IncresePointsRPC", PhotonTargets.All, team);
    }

    [PunRPC]
    public void IncresePointsRPC(string team)
    {
        if (team == "blue")
        {
            bluePoints++;
            bluePointsText.text = bluePoints.ToString();
        }
        else
        {
            redPoints++;
            redPointsText.text = redPoints.ToString();
        }
        ResetPlay();
    }

    public void ResetPlay()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (redPoints == maxPoints || bluePoints == maxPoints)
            {
                photonView.RPC("ShowResultRPC", PhotonTargets.All, null);
                StartCoroutine(BackToRoom());
            } 
            else
            {
                RespawnPlayers();
            }
        }
    }

    public void RespawnPlayers()
    {
        HealthController[] healths = FindObjectsOfType<HealthController>();
        for (var i = 0; i < healths.Length; i++)
        {
            healths[i].Revive();
            gameManager.ResetPosition();
        }
    }

    [PunRPC]
    public void ShowResultRPC()
    {
        if (bluePoints > redPoints)
        {
            resultText.text = "Blue team wins!";
            resultText.color = Color.blue;
        }
        else
        {
            resultText.text = "Red team wins!";
            resultText.color = Color.red;
        }
    }

    public IEnumerator BackToRoom()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.LoadLevel("Menu");
    }
}
