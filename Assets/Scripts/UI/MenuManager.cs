using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Photon.PunBehaviour
{
    public GameObject roomCanvas;
    public GameObject startButton;
    public GameObject charSelectionCanvas;

    [HideInInspector]
    public GameObject mySlot;
    private string playerName;

    private void Start()
    {
        if (PhotonNetwork.inRoom)
        {
            roomCanvas.SetActive(true);
            mySlot = PhotonNetwork.Instantiate("UI/PlayerSlot", Vector3.zero, Quaternion.identity, 0);
        }
    }

    public void Connect(string name)
    {
        playerName = name;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings("ShittyMoba");
    }

    public override void OnConnectedToMaster()
    {
        var roomOptions = new RoomOptions()
        {
            IsVisible = true,
            MaxPlayers = 12
        };
        PhotonNetwork.JoinOrCreateRoom("ShittyMobaRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        roomCanvas.SetActive(true);
        mySlot = PhotonNetwork.Instantiate("UI/PlayerSlot", Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "name", playerName } });
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "blue" } });
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "character", "Wizard" } });
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private void Update()
    {
        startButton.SetActive(PhotonNetwork.isMasterClient);
    }
}
