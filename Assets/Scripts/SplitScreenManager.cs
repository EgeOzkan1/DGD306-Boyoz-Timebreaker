using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera cam1;
    public Camera cam2;

    public float screenEdgeBuffer = 0.1f;
    public float checkInterval = 0.1f;

    private bool isSplit = false;
    private int playerCount = 0;

    void Start()
    {
        Invoke(nameof(SetupCameras), 0.1f); // oyuncular instantiate edildiyse 0.1 saniye sonra kontrol et
        InvokeRepeating(nameof(CheckSplitCondition), 0.2f, checkInterval);
    }

    void SetupCameras()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerCount = players.Length;

        if (playerCount == 1)
        {
            player1 = players[0].transform;
            cam1.rect = new Rect(0f, 0f, 1f, 1f);
            cam2.enabled = false;
            cam1.GetComponent<CameraFollow>().target = player1;
        }
        else if (playerCount == 2)
        {
            player1 = players[0].transform;
            player2 = players[1].transform;
            cam1.GetComponent<CameraFollow>().target = player1;
            cam2.GetComponent<CameraFollow>().target = player2;
            cam1.rect = new Rect(0f, 0f, 0.5f, 1f);
            cam2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
            cam2.enabled = true;
        }
    }

    void CheckSplitCondition()
    {
        if (playerCount != 2 || player1 == null || player2 == null) return;

        Vector3 screenPosP2 = cam1.WorldToViewportPoint(player2.position);

        bool p2OutRight = screenPosP2.x > 1f + screenEdgeBuffer;
        bool p2OutLeft = screenPosP2.x < 0f - screenEdgeBuffer;

        if (!isSplit && (p2OutLeft || p2OutRight))
        {
            EnableSplitScreen(p2OutRight);
        }
        else if (isSplit && screenPosP2.x > 0f + screenEdgeBuffer && screenPosP2.x < 1f - screenEdgeBuffer)
        {
            MergeScreen();
        }
    }

    void EnableSplitScreen(bool p2IsRight)
    {
        isSplit = true;

        if (p2IsRight)
        {
            cam1.rect = new Rect(0f, 0f, 0.5f, 1f);   // sol
            cam2.rect = new Rect(0.5f, 0f, 0.5f, 1f);  // sağ
        }
        else
        {
            cam1.rect = new Rect(0.5f, 0f, 0.5f, 1f);  // sağ
            cam2.rect = new Rect(0f, 0f, 0.5f, 1f);   // sol
        }

        cam2.enabled = true;
    }

    void MergeScreen()
    {
        isSplit = false;
        cam1.rect = new Rect(0f, 0f, 1f, 1f);
        cam2.enabled = false;
    }
}
