using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public MenuManager menuManager;
    public InputField nameInput;
    public InputField roomInput;

    public void Connect()
    {
        if (nameInput.text != "" && nameInput.text != "")
        {
            menuManager.Connect(nameInput.text, roomInput.text);
        }
    }
}
