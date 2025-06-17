using UnityEngine;

//Oyncuların sahnede doğmasını yöneten script.
public class PlayerSpawner : MonoBehaviour
{
    public GameObject rangedPlayerPrefab;
    public GameObject meleePlayerPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    void Start()
    {
        var mode = GameManager.Instance.CurrentMode;

        if (mode == GameManager.GameMode.SinglePlayer)
        {
            Instantiate(rangedPlayerPrefab, spawnPoint1.position, Quaternion.identity);
        }
        else if (mode == GameManager.GameMode.TwoPlayer)
        {
            Instantiate(rangedPlayerPrefab, spawnPoint1.position, Quaternion.identity);
            Instantiate(meleePlayerPrefab, spawnPoint2.position, Quaternion.identity);
        }
    }
}
