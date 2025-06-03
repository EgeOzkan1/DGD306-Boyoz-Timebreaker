using UnityEngine;

public class InputDebugTester : MonoBehaviour
{
    void Start()
    {
        string[] joysticks = Input.GetJoystickNames();
        for (int i = 0; i < joysticks.Length; i++)
        {
            Debug.Log($"Joystick {i + 1}: {joysticks[i]}");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) Debug.Log("FIRE1 týklandý (joystick 1 button 0)");
        if (Input.GetButtonDown("Fire2")) Debug.Log("FIRE2 týklandý (joystick 2 button 1)");
        if (Input.GetButtonDown("Jump")) Debug.Log("Jump týklandý (buton 3)");

        Debug.Log("Ranged hareket (Joystick 1): " + Input.GetAxis("Horizontal"));
        Debug.Log("Melee hareket (Joystick 2): " + Input.GetAxis("Joystick2Horizontal"));
    }
}
