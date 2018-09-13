using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Photon.PunBehaviour
{
    public GameObject roomCanvas;
    public GameObject startButton;
    public GameObject charSelectionCanvas;
    public GameObject chat;

    [HideInInspector]
    public GameObject mySlot;
    private string playerName;
    private string roomName;

    private void Start()
    {
        if (PhotonNetwork.inRoom)
        {
            roomCanvas.SetActive(true);
            mySlot = PhotonNetwork.Instantiate("UI/PlayerSlot", Vector3.zero, Quaternion.identity, 0);
        }
    }

    public void Connect(string name, string room)
    {
        playerName = name;
        roomName = room;
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
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        roomCanvas.SetActive(true);
        mySlot = PhotonNetwork.Instantiate("UI/PlayerSlot", Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "name", playerName } });
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "blue" } });
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "character", "Wizard" } });
        chat.SetActive(true);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }

    private void Update()
    {
        startButton.SetActive(PhotonNetwork.isMasterClient);
    }
}
