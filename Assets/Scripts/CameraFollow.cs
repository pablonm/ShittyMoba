using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private Transform target;
    private HealthController healthController;

    public void SetTarget(Transform t)
    {
        target = t;
        healthController = t.GetComponent<HealthController>();
    }

    void FixedUpdate()
    {
        if (target && healthController && healthController.isAlive)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal"), transform.position.y + Input.GetAxis("Vertical"), transform.position.z);
        }
    }

}