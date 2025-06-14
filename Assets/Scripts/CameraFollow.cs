using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float lookAheadDistance = 4f;
    public float lookSmoothTime = 0.1f;
    public float verticalOffset = 2f; 

    private Vector3 currentVelocity;
    private Vector3 lookAheadOffset;

    private float lastTargetX;

    void Start()
    {
        if (target != null)
            lastTargetX = target.position.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float deltaX = target.position.x - lastTargetX;

        if (Mathf.Abs(deltaX) > 0.01f)
        {
            lookAheadOffset = new Vector3(Mathf.Sign(deltaX) * lookAheadDistance, 0f, 0f);
        }

        
        Vector3 targetPos = target.position + lookAheadOffset + new Vector3(0f, verticalOffset, 0f);
        targetPos.z = -10f;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, lookSmoothTime);

        lastTargetX = target.position.x;
    }
}
