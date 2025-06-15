using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    private int playersInArea = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        
        if (EnemiesExistInScene())
        {
            Debug.Log("Tüm düşmanlar temizlenmeden çıkış yapılamaz!");
            return;
        }

        other.gameObject.SetActive(false); 

        string currentScene = SceneManager.GetActiveScene().name;
        string nextSceneName = GetNextSceneName(currentScene);

        if (string.IsNullOrEmpty(nextSceneName)) return;

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

    bool EnemiesExistInScene()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length > 0;
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
