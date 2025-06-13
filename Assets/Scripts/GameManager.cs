using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameMode
    {
        None,
        SinglePlayer,
        TwoPlayer
    }

    public GameMode CurrentMode { get; set; } = GameMode.None;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
