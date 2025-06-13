using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.orthographicSize = SplitScreenManager.IsSplit ? 5f : 7f;
        }

        Vector3 targetPosition = target.position;
        targetPosition.z = -10f;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}

