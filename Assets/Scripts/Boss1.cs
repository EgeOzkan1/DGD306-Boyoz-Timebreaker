using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [Header("Attack")]
    public GameObject bossProjectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 3f;
    public float projectileSpeed = 8f;

    [Header("Health")]
    public int maxHealth = 20;
    private int currentHealth;

    private float shootTimer = 0f;
    private Transform currentTarget;

    private int shotCount = 0; 

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        currentTarget = FindNearestPlayer();
        FaceTarget(currentTarget);

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f && currentTarget != null)
        {
            ShootAt(currentTarget.position);
            shootTimer = shootCooldown;
        }
    }

    Transform FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = p.transform;
            }
        }

        return nearest;
    }

    void ShootAt(Vector3 targetPosition)
    {
        if (bossProjectilePrefab && firePoint)
        {
            shotCount++; 

            float currentSpeed = (shotCount % 3 == 0) ? projectileSpeed * 2f : projectileSpeed;

            GameObject projectile = Instantiate(bossProjectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (targetPosition - firePoint.position).normalized;
                rb.velocity = direction * currentSpeed;
            }
        }
    }

    void FaceTarget(Transform target)
    {
        if (target == null) return;

        Vector3 scale = transform.localScale;
        scale.x = target.position.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        GetComponent<HitIndication>()?.Flash();
        Debug.Log("Boss took damage: " + amount + " | Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }
}
