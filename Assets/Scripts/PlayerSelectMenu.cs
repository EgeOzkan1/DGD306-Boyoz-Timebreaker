using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectMenu : MonoBehaviour
{
    public Button[] menuButtons;
    private int currentIndex = 0;
    private float inputCooldown = 0.2f;
    private float cooldownTimer = 0f;

    void Start()
    {
        HighlightButton(currentIndex);
    }

    void Update()
    {
        cooldownTimer -= Time.unscaledDeltaTime;

        float vertical = Input.GetAxis("DPadVertical");

        if (cooldownTimer <= 0f)
        {
            if (vertical > 0.5f)
            {
                MoveSelection(-1);
                cooldownTimer = inputCooldown;
            }
            else if (vertical < -0.5f)
            {
                MoveSelection(1);
                cooldownTimer = inputCooldown;
            }
        }

        if  (Input.GetButtonDown("Fire1")) 
            {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    void MoveSelection(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = menuButtons.Length - 1;
        if (currentIndex >= menuButtons.Length) currentIndex = 0;
        HighlightButton(currentIndex);
    }

    void HighlightButton(int index)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            var colors = menuButtons[i].colors;
            colors.normalColor = (i == index) ? Color.yellow : Color.white;
            menuButtons[i].colors = colors;
        }
    }
}
