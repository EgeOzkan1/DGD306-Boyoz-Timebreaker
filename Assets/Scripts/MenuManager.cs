using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            if (panel1.activeSelf)
            {
                panel1.SetActive(false);
                panel2.SetActive(true);

            }
            if (Input.anyKeyDown)
            {
                Debug.Log("Bir tu�a bas�ld�.");
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Debug.Log("X tu�u alg�land�");
            }
        }
    }
}
