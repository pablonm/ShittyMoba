using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoManager : MonoBehaviour
{
    public Text characterName;
    public Text health;
    public Text walkingSpeed;
    public Text attackSpeed;
    public Image qIcon;
    public Text qName;
    public Text qDesc;
    public Image wIcon;
    public Text wName;
    public Text wDesc;
    public Image eIcon;
    public Text eName;
    public Text eDesc;
    public Image rIcon;
    public Text rName;
    public Text rDesc;

    public void ShowCharacter(CharacterInfo info)
    {
        characterName.text = info.characterName;
        health.text = info.health;
        walkingSpeed.text = info.walkingSpeed;
        attackSpeed.text = info.attackSpeed;
        qIcon.sprite = info.Q_image;
        qName.text = info.Q_name;
        qDesc.text = info.Q_description;
        wIcon.sprite = info.W_image;
        wName.text = info.W_name;
        wDesc.text = info.W_description;
        eIcon.sprite = info.E_image;
        eName.text = info.E_name;
        eDesc.text = info.E_description;
        rIcon.sprite = info.R_image;
        rName.text = info.R_name;
        rDesc.text = info.R_description;
    }

    public void SelectCharacter(string charName)
    {
        FindObjectOfType<MenuManager>().mySlot.GetComponent<PlayerSlot>().ChangeCharacter(charName);
        gameObject.SetActive(false);
    }
}
