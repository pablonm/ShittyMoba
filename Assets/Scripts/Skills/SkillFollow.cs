using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFollow : MonoBehaviour
{

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Vector3 offset;

	private void Update ()
    {
        if (target != null && offset != null)
        {
            transform.position = target.position + offset;
        }
	}
}
