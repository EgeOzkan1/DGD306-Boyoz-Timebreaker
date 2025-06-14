using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    public GameObject player1UI;
    public GameObject player2UI;

    void Start()
    {
        if (GameManager.Instance.CurrentMode == GameManager.GameMode.SinglePlayer)
        {
            player1UI.SetActive(true);
            player2UI.SetActive(false);
        }
        else if (GameManager.Instance.CurrentMode == GameManager.GameMode.TwoPlayer)
        {
            player1UI.SetActive(true);
            player2UI.SetActive(true);
        }
    }
}
