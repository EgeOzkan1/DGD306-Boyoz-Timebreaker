using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    private Transform player1;
    private Transform player2;

    public float screenEdgeBuffer = 0.1f;
    public float checkInterval = 0.1f;

    private bool isSplit = false;

    void Start()
    {
        Invoke(nameof(SetupPlayersAndCameras), 0.2f); // Oyuncular spawn olsun
        InvokeRepeating(nameof(CheckSplitCondition), 0.3f, checkInterval);
    }

    void SetupPlayersAndCameras()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1)
        {
            // Tek oyunculu mod
            player1 = players[0].transform;
            if (cam1 != null && player1 != null)
                cam1.GetComponent<CameraFollow>().target = player1;

            cam1.rect = new Rect(0f, 0f, 1f, 1f);
            cam2.enabled = false;
            isSplit = false;
            return;
        }

        if (players.Length >= 2)
        {
            player1 = players[0].transform;
            player2 = players[1].transform;

            if (cam1 != null) cam1.GetComponent<CameraFollow>().target = player1;
            if (cam2 != null) cam2.GetComponent<CameraFollow>().target = player2;
        }
    }

    void CheckSplitCondition()
    {
        if (player1 == null || player2 == null) return;

        Vector3 screenPosP2 = cam1.WorldToViewportPoint(player2.position);

        bool outRight = screenPosP2.x > 1f + screenEdgeBuffer;
        bool outLeft = screenPosP2.x < 0f - screenEdgeBuffer;
        bool outUp = screenPosP2.y > 1f + screenEdgeBuffer;
        bool outDown = screenPosP2.y < 0f - screenEdgeBuffer;

        if (!isSplit && (outRight || outLeft || outUp || outDown))
        {
            if (outUp || outDown)
                EnableVerticalSplit(outUp);
            else
                EnableHorizontalSplit(outRight);
        }

        if (isSplit &&
            screenPosP2.x > 0f + screenEdgeBuffer && screenPosP2.x < 1f - screenEdgeBuffer &&
            screenPosP2.y > 0f + screenEdgeBuffer && screenPosP2.y < 1f - screenEdgeBuffer)
        {
            MergeScreen();
        }
    }

    void EnableHorizontalSplit(bool p2IsRight)
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
            cam2.rect = new Rect(0f, 0f, 0.5f, 1f);    // sol
        }

        cam2.enabled = true;
    }

    void EnableVerticalSplit(bool p2IsAbove)
    {
        isSplit = true;

        if (p2IsAbove)
        {
            cam1.rect = new Rect(0f, 0f, 1f, 0.5f);   // alt
            cam2.rect = new Rect(0f, 0.5f, 1f, 0.5f); // üst
        }
        else
        {
            cam1.rect = new Rect(0f, 0.5f, 1f, 0.5f); // üst
            cam2.rect = new Rect(0f, 0f, 1f, 0.5f);   // alt
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
