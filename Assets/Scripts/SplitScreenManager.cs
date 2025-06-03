using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera cam1;
    public Camera cam2;

    public float screenEdgeBuffer = 0.1f; // kenardan biraz boþluk payý
    public float checkInterval = 0.1f;

    private bool isSplit = false;

    void Start()
    {
        InvokeRepeating(nameof(CheckSplitCondition), 0f, checkInterval);
    }

    void CheckSplitCondition()
    {
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
            cam1.rect = new Rect(0f, 0f, 0.5f, 1f); // sol
            cam2.rect = new Rect(0.5f, 0f, 0.5f, 1f); // sað
        }
        else
        {
            cam1.rect = new Rect(0.5f, 0f, 0.5f, 1f); // sað
            cam2.rect = new Rect(0f, 0f, 0.5f, 1f); // sol
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
