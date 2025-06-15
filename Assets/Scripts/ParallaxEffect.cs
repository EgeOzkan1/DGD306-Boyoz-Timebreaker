using UnityEngine;

public class ParallaxEffect: MonoBehaviour

    //Yiğit parallax efekti yapmak istemiş yapay zekaya yaptırdığı için içine dokunmadım. - ege
{
    [Tooltip("Ana kamera nesnesi (boþ býrakýlýrsa otomatik atanýr).")]
    public Transform cameraTransform;

    [Tooltip("Bu katmanýn hareket hýzý. Küçük deðerler daha uzaktaymýþ gibi görünür.")]
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
