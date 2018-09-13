using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : Photon.PunBehaviour
{

    public InputField input;
    public GameObject messagePrefab;
    public Transform messagesPanel;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && input.text != "")
        {
            SendMessage();
        }
    }

    private void SendMessage()
    {
        photonView.RPC(
            "SendMessageRPC",
            PhotonTargets.All,
            (string)PhotonNetwork.player.CustomProperties["name"],
            input.text
            );
        input.text = "";
    }

    [PunRPC]
    private void SendMessageRPC(string username, string message)
    {
        if (messagesPanel.childCount > 5)
        {
            Destroy(messagesPanel.GetChild(0).gameObject);
        }
        var msg = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, messagesPanel);
        msg.GetComponent<Text>().text = username + ": " + message;
        StartCoroutine(DestroyMessage(msg));
    }

    public IEnumerator DestroyMessage(GameObject msg)
    {
        yield return new WaitForSeconds(30f);
        Destroy(msg);
        yield break;
    }
}
