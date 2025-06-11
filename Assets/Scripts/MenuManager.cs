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
                Debug.Log("Bir tuþa basýldý.");
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Debug.Log("X tuþu algýlandý");
            }
        }
    }
}
