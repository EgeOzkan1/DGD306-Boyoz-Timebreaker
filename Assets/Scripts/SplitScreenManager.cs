using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera cam1;
    public Camera cam2;
    public float splitDistance = 8f;

    private bool isSplit = false;

    void Update()
    {
        float dist = Vector2.Distance(player1.position, player2.position);

        if (!isSplit && dist > splitDistance)
        {
            EnableSplitScreen();
        }
        else if (isSplit && dist <= splitDistance)
        {
            MergeScreen();
        }
    }

    void EnableSplitScreen()
    {
        isSplit = true;
        cam1.rect = new Rect(0f, 0f, 0.5f, 1f);
        cam2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        cam2.enabled = true;
    }

    void MergeScreen()
    {
        isSplit = false;
        cam1.rect = new Rect(0f, 0f, 1f, 1f);
        cam2.enabled = false;
    }
}
