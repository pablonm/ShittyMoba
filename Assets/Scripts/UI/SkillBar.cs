using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour {

	private void Start () {
		
	}

    public void InitializeSkill(string key, Sprite image)
    {
        transform.Find(key).GetComponent<Image>().sprite = image;
    }

    public void StartCooldown(string key, float seconds)
    {
        StartCoroutine(StartCooldownCoroutine(
            key,
            seconds
            ));
    }

    public IEnumerator StartCooldownCoroutine(string key, float seconds)
    {
        transform.Find(key).Find("Cooldown").gameObject.SetActive(true);
        for (int i = 0; i < seconds; i++)
        {
            transform.Find(key).Find("Cooldown").Find("Seconds").GetComponent<Text>().text = (seconds - i).ToString();
            yield return new WaitForSeconds(1);
        }
        transform.Find(key).Find("Cooldown").gameObject.SetActive(false);
        yield break;
    }
}

