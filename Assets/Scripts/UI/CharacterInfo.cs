using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{

    public string characterName;
    public string health;
    public string walkingSpeed;
    public string attackSpeed;

    [Space(20)]
    public Sprite Q_image;
    public string Q_name;
    [TextArea]
    public string Q_description;

    [Space(20)]
    public Sprite W_image;
    public string W_name;
    [TextArea]
    public string W_description;

    [Space(20)]
    public Sprite E_image;
    public string E_name;
    [TextArea]
    public string E_description;

    [Space(20)]
    public Sprite R_image;
    public string R_name;
    [TextArea]
    public string R_description;

    public void ShowInfo()
    {
        transform.parent.parent.GetComponent<CharacterInfoManager>().ShowCharacter(this);
    }

}
