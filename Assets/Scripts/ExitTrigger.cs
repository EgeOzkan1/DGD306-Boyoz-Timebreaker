using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    private int playersInArea = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.gameObject.SetActive(false); // oyuncuyu sahneden kaldır

        string currentScene = SceneManager.GetActiveScene().name;
        string nextSceneName = GetNextSceneName(currentScene);

        if (string.IsNullOrEmpty(nextSceneName)) return; // geçiş tanımsızsa hiçbir şey yapma

        if (GameManager.Instance.CurrentMode == GameManager.GameMode.SinglePlayer)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (GameManager.Instance.CurrentMode == GameManager.GameMode.TwoPlayer)
        {
            playersInArea++;

            if (playersInArea >= 2)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    string GetNextSceneName(string currentScene)
    {
        if (currentScene == "Tutorial") return "Level1";
        if (currentScene == "Level1") return "Level2";
        if (currentScene == "Level2") return "Level3";
        if (currentScene == "Level3") return "MainMenu";

        return null;
    }
}
