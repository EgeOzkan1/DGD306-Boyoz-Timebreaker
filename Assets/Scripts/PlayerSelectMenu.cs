using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectMenu : MonoBehaviour
{
    public Button[] menuButtons;                  // [0] = 1P, [1] = 2P
    public RectTransform arrowIndicator;          // Assign arrow image here
    public Vector3 arrowOffset = new Vector3(-100f, 0f, 0f); // Offset to position arrow

    private int currentIndex = 0;
    private float inputCooldown = 0.2f;
    private float cooldownTimer = 0f;

    void OnEnable()
    {
        currentIndex = 0;
        MoveArrowTo(currentIndex);
        Time.timeScale = 1f;
    }

    void Update()
    {
        cooldownTimer -= Time.unscaledDeltaTime;

        // Match gameplay input: prefer analog stick, fallback to D-pad
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(horizontal, 0f))
            horizontal = Input.GetAxis("DPadHorizontal1");

        bool moveRight = horizontal > 0.5f;
        bool moveLeft = horizontal < -0.5f;

        if (cooldownTimer <= 0f)
        {
            if (moveRight)
            {
                MoveSelection(1);
                cooldownTimer = inputCooldown;
            }
            else if (moveLeft)
            {
                MoveSelection(-1);
                cooldownTimer = inputCooldown;
            }
        }

       
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            ConfirmSelection();
        }
    }

    void MoveSelection(int direction)
    {
        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = menuButtons.Length - 1;
        if (currentIndex >= menuButtons.Length)
            currentIndex = 0;

        MoveArrowTo(currentIndex);
    }

    void MoveArrowTo(int index)
    {
        if (arrowIndicator != null && menuButtons.Length > index)
        {
            RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();
            arrowIndicator.position = buttonRect.position + arrowOffset;
        }
    }

    void ConfirmSelection()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager bulunamadı. Sahneye bir GameManager prefabı yerleştirildiğinden emin olun.");
            return;
        }

        if (currentIndex == 0)
        {
            Debug.Log("Starting 1 Player Game");
            GameManager.Instance.CurrentMode = GameManager.GameMode.SinglePlayer;
        }
        else if (currentIndex == 1)
        {
            Debug.Log("Starting 2 Player Game");
            GameManager.Instance.CurrentMode = GameManager.GameMode.TwoPlayer;
        }

        SceneManager.LoadScene("GameScene");
    }
}
