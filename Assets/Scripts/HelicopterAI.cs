using UnityEngine;

public class HelicopterAI : MonoBehaviour
{
    [Header("Movement")]
    public float hoverAmplitude = 1f;
    public float hoverSpeed = 2f;
    public float retreatDistance = 4f;
    public float retreatSpeed = 2f;

    [Header("Follow if too far")]
    public float followThreshold = 10f;
    public float followSpeed = 1.5f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float minShootDelay = 2f;
    public float maxShootDelay = 4f;
    public float projectileSpeed = 8f;

    [Header("Tilt")]
    public float tiltAngle = 5f;

    [Header("Health")]
    public int maxHealth = 10;
    private int currentHealth;

    private Vector3 startPosition;
    private Transform[] players;
    private float shootTimer;
    private bool facingRight = true;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        players = GetAllPlayers();
        ResetShootTimer();
    }

    void Update()
    {
        players = GetAllPlayers();
        Hover();
        RetreatIfNeeded();
        ApproachIfTooFar();
        FaceClosestPlayer();

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootAtAllPlayers();
            ResetShootTimer();
        }
    }

    void Hover()
    {
        float offsetY = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + offsetY, transform.position.z);
    }

    void RetreatIfNeeded()
    {
        foreach (Transform player in players)
        {
            if (player == null) continue;

            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < retreatDistance)
            {
                Vector2 dir = (transform.position - player.position).normalized;
                transform.position += (Vector3)(dir * retreatSpeed * Time.deltaTime);
            }
        }
    }

    void ApproachIfTooFar()
    {
        Transform nearest = FindNearestPlayer();
        if (nearest == null) return;

        float distance = Vector2.Distance(transform.position, nearest.position);
        if (distance > followThreshold)
        {
            Vector2 dir = (nearest.position - transform.position).normalized;
            transform.position += (Vector3)(dir * followSpeed * Time.deltaTime);
        }
    }

    void ShootAtAllPlayers()
    {
        foreach (Transform player in players)
        {
            if (player == null) continue;

            int bulletCount = Random.Range(1, 2);
            for (int i = 0; i < bulletCount; i++)
            {
                ShootAt(player.position);
            }
        }
    }

    void ShootAt(Vector3 target)
    {
        if (projectilePrefab && firePoint)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 baseDir = (target - firePoint.position).normalized;
                float angleOffset = Random.Range(-10f, 10f);
                Vector2 spreadDir = Quaternion.Euler(0, 0, angleOffset) * baseDir;
                rb.velocity = spreadDir * projectileSpeed;
            }
        }
    }

    void FaceClosestPlayer()
    {
        Transform closest = FindNearestPlayer();
        if (closest == null) return;

        bool shouldFaceRight = closest.position.x > transform.position.x;

        if (shouldFaceRight != facingRight)
        {
            Flip();
        }

        float angle = facingRight ? -tiltAngle : tiltAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ResetShootTimer()
    {
        shootTimer = Random.Range(minShootDelay, maxShootDelay);
    }

    Transform[] GetAllPlayers()
    {
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        Transform[] targets = new Transform[playerObjs.Length];
        for (int i = 0; i < playerObjs.Length; i++)
        {
            targets[i] = playerObjs[i].transform;
        }
        return targets;
    }

    Transform FindNearestPlayer()
    {
        float closestDist = Mathf.Infinity;
        Transform nearest = null;
        foreach (Transform p in players)
        {
            if (p == null) continue;
            float dist = Vector2.Distance(transform.position, p.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                nearest = p;
            }
        }
        return nearest;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        GetComponent<HitIndication>()?.Flash();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
