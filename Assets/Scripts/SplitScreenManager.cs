using UnityEngine;
using UnityEngine.SceneManagement;

public class SplitScreenManager : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    private Transform player1; 
    private Transform player2; 

    public float screenEdgeBuffer = 5f;
    public float checkInterval = 0.1f;

    public static bool IsSplit { get; private set; }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        Invoke(nameof(SetupPlayersAndCameras), 0.2f);
        InvokeRepeating(nameof(CheckSplitCondition), 0.3f, checkInterval);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke(nameof(SetupPlayersAndCameras), 0.3f);
    }

    public void SetupPlayersAndCameras()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length < 1)
        {
            Invoke(nameof(SetupPlayersAndCameras), 0.2f);
            return;
        }

        if (players.Length == 1)
        {
            player1 = players[0].transform;
            cam1.GetComponent<CameraFollow>().target = player1;
            cam1.rect = new Rect(0f, 0f, 1f, 1f);
            cam2.enabled = false;
            IsSplit = false;
            return;
        }

        if (players.Length >= 2)
        {
            player1 = players[0].transform;
            player2 = players[1].transform; 

            cam1.GetComponent<CameraFollow>().target = player2; 
            cam2.GetComponent<CameraFollow>().target = player1;

            cam1.rect = new Rect(0f, 0f, 1f, 0.5f);
            cam2.rect = new Rect(0f, 0.5f, 1f, 0.5f);
            cam2.enabled = false;
            IsSplit = false;
        }
    }

    void CheckSplitCondition()
    {
        if (player1 == null || player2 == null) return;

        float xDistance = Mathf.Abs(player1.position.x - player2.position.x);

        if (!IsSplit && xDistance > screenEdgeBuffer)
        {
            EnableVerticalSplit();
        }

        
    }

    void EnableVerticalSplit()
    {
        IsSplit = true;

        cam1.rect = new Rect(0f, 0f, 1f, 0.5f);   
        cam2.rect = new Rect(0f, 0.5f, 1f, 0.5f); 

        cam1.GetComponent<CameraFollow>().target = player2;
        cam2.GetComponent<CameraFollow>().target = player1;

        cam2.enabled = true;
    }
}
