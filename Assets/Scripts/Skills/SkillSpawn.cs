using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawn : Skill
{
    public enum SPAWNPOS { HAND, PLAYER, CURSOR };

    public SPAWNPOS spawnPlace = SPAWNPOS.CURSOR;
    public bool follow;
    public GameObject skillPrefab;
    public bool applyRotation;
    public bool showLine;

    private LineRenderer line;
    private Material lineMat;
    private Transform bassicAttackPoint;

    public override void PrepareBehaviour()
    {
        if (showLine)
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.startColor = Color.green;
            line.endColor = Color.green;
            line.startWidth = 0.03f;
            lineMat = new Material(Shader.Find("Particles/Additive"));
            line.material = lineMat;
        }
        bassicAttackPoint = transform.parent.parent.Find("BasicAttackPoint");
    }

    public override void CancelBehavirour()
    {
        if (showLine)
        {
            DestroyImmediate(line);
            DestroyImmediate(lineMat);
        }
    }

    public override bool CastBehavirour()
    {
        if (showLine)
        {
            Destroy(line);
            Destroy(lineMat);
        }
        Vector3 spawnPosition;
        switch (spawnPlace)
        {
            case SPAWNPOS.HAND:
                spawnPosition = bassicAttackPoint.position;
                break;
            case SPAWNPOS.CURSOR:
                spawnPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                break;
            case SPAWNPOS.PLAYER:
                spawnPosition = transform.parent.parent.GetComponent<SpriteRenderer>().bounds.center;
                break;
            default:
                spawnPosition = Vector3.zero;
                break;
        }
        GameObject instancedSkill;
        if (applyRotation)
        {
            instancedSkill = PhotonNetwork.Instantiate(
                "Skills/" + skillPrefab.name, 
                spawnPosition, 
                Quaternion.FromToRotation(Vector2.right, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)bassicAttackPoint.position), 
                0);
        }
        else
        {
            instancedSkill = PhotonNetwork.Instantiate(
                "Skills/" + skillPrefab.name,
                spawnPosition,
                Quaternion.identity,
                0);
        }
        if (follow)
        {
            instancedSkill.AddComponent<SkillFollow>().target = transform.parent.parent;
            instancedSkill.GetComponent<SkillFollow>().offset = spawnPosition - transform.parent.parent.position;
        }
        return true;
    }

    public override void UpdateBehaviour()
    {
        if (line)
        {
            line.SetPositions( new Vector3[2] { bassicAttackPoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)});
        }
    }

    public override void StartBehaviour()
    {
        // Nothing
    }
}
