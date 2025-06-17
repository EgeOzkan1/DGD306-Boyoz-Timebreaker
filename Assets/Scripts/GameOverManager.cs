using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour

    //Bunu game manager içinde yapmam gerekiyodu, game manager scripti olduğunu unutmuşum - Ege
{
    public static GameOverManager Instance;

    public GameObject gameOverUI;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; 
    }

    void Update()
    {
        if (isGameOver && Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
