using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCanvas : Photon.PunBehaviour
{
    public Sprite blueHealthBar;
    public Sprite redHealthBar;

    void Update ()
    {
        transform.rotation = Quaternion.identity;
	}

    private void Start()
    {
        transform.Find("HealthBar").Find("Health").GetComponent<Image>().sprite = ((string)photonView.owner.CustomProperties["team"] == "blue") ? blueHealthBar : redHealthBar;
        transform.Find("Name").GetComponent<Text>().text = (string)photonView.owner.CustomProperties["name"];
    }
}
