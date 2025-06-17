using UnityEngine;

public class PlayerHealth : MonoBehaviour

// Oyuncuların can durumunu yöneten script.
{
    public int maxHealth = 5;
    private int currentHealth;

    public bool isRanged;
    public PlayerHealthUI healthUI; 

    void Start()
    {
        currentHealth = maxHealth;

        
        if (healthUI == null)
        {
            var uis = FindObjectsOfType<PlayerHealthUI>();
            foreach (var ui in uis)
            {
                if ((isRanged && ui.ownerType == OwnerType.Ranged) ||
                    (!isRanged && ui.ownerType == OwnerType.Melee))
                {
                    healthUI = ui;
                    break;
                }
            }
        }

        if (healthUI != null)
            healthUI.SetHealth(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        GetComponent<HitIndication>()?.Flash();

        if (healthUI != null)
            healthUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);

        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.TriggerGameOver();
        }
    }
}
