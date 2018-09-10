using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : Photon.PunBehaviour
{
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (((string)photonView.owner.CustomProperties["team"]) == "red")
        {
            transform.Find("Team").GetComponent<Image>().color = Color.red;
        }
        else
        {
            transform.Find("Team").GetComponent<Image>().color = Color.blue;
        }
        transform.Find("PlayerName").GetComponent<Text>().text = (string)photonView.owner.CustomProperties["name"];
        // Show the character picture
        foreach (Transform child in transform.Find("CharPic"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("CharPic").Find((string)photonView.owner.CustomProperties["character"]).gameObject.SetActive(true);
        transform.Find("CharName").GetComponent<Text>().text = (string)photonView.owner.CustomProperties["character"];
        transform.SetParent(GameObject.Find("TeamLayoutGroup").transform);
    }

    public void ChangeCharacter(string charName)
    {
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "character", charName } });
        photonView.RPC("ChangeCharacterRPC", PhotonTargets.All, charName);
    }

    [PunRPC]
    public void ChangeCharacterRPC(string charName)
    {
        // Show the character picture
        foreach (Transform child in transform.Find("CharPic"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("CharPic").Find(charName).gameObject.SetActive(true);

        // Show the character name
        transform.Find("CharName").GetComponent<Text>().text = charName;
    }

    public void ToggleTeam()
    {
        if (photonView.isMine)
        {
            if (transform.Find("Team").GetComponent<Image>().color == Color.blue)
            {
                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "red" } });
            }
            else
            {
                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "blue" } });
            }
            photonView.RPC("ToggleTeamRPC", PhotonTargets.All, null);
        }
    }

    [PunRPC]
    public void ToggleTeamRPC()
    {
        Image teamImage = transform.Find("Team").GetComponent<Image>();
        if (teamImage.color == Color.blue)
        {
            teamImage.color = Color.red;
            //PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "red" } });
        }
        else
        {
            teamImage.color = Color.blue;
            //PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "blue" } });
        }
    }

    public void OpenCharacterSelection()
    {
        if (photonView.isMine)
        {
            FindObjectOfType<MenuManager>().charSelectionCanvas.SetActive(true);
        }
    }
}
