using UnityEngine;

public class ParallaxEffect: MonoBehaviour
{
    [Tooltip("Ana kamera nesnesi (bo� b�rak�l�rsa otomatik atan�r).")]
    public Transform cameraTransform;

    [Tooltip("Bu katman�n hareket h�z�. K���k de�erler daha uzaktaym�� gibi g�r�n�r.")]
    [Range(0f, 1f)]
    public float parallaxMultiplier = 0.5f;

    private Vector3 previousCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        // Sadece X ve Y ekseninde parallax etkisi uygula
        transform.position += new Vector3(
            deltaMovement.x * parallaxMultiplier,
            deltaMovement.y * parallaxMultiplier,
            0f
        );

        previousCameraPosition = cameraTransform.position;
    }
}
