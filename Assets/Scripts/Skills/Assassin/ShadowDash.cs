using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDash : Skill
{
    public float impulse;

    private LineRenderer line;
    private Material lineMat;
    private Transform basicAttackPoint;

    public override void CancelBehavirour()
    {
        DestroyImmediate(line);
        DestroyImmediate(lineMat);
    }

    public override bool CastBehavirour()
    {
        DestroyImmediate(line);
        DestroyImmediate(lineMat);
        transform.parent.parent.GetComponent<StatusController>().ApplyForce(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * impulse);
        return true;
    }

    public override void PrepareBehaviour()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startColor = Color.green;
        line.endColor = Color.green;
        line.startWidth = 0.03f;
        lineMat = new Material(Shader.Find("Particles/Additive"));
        line.material = lineMat;
    }

    public override void StartBehaviour()
    {
        basicAttackPoint = transform.parent.parent.Find("BasicAttackPoint");
    }

    public override void UpdateBehaviour()
    {
        if (line)
        {
            line.SetPositions(new Vector3[2] { basicAttackPoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) });
        }
    }
}
