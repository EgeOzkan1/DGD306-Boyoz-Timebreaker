using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour

{
    public GameObject panel1;
    public GameObject panel2;



    void Start()
    {
        Time.timeScale = 1f;

        
        float _ = Input.GetAxis("DPadHorizontal1");

        
        Debug.Log("DPadHorizontal1 wake attempt: " + _);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (panel1.activeSelf)
            {
                panel1.SetActive(false);
                panel2.SetActive(true);

            }
           
     
        }
    }
}
