using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Detection")]
    public float detectionRange = 8f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;

    [Header("Projectile")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Patrol")]
    public float patrolTime = 2f;

    [Header("Ledge Detection")]
    public Transform ledgeCheck;
    public float ledgeCheckDistance = 0.5f;
    public LayerMask groundLayer;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Transform player;
    private float attackTimer = 0f;
    private bool facingRight = true;
    private float patrolTimer = 0f;
    private int patrolDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolTimer = patrolTime;
        currentHealth = maxHealth;
    }

    void Update()
    {
        UpdateNearestPlayer();
        attackTimer -= Time.deltaTime;

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                if (distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                Chase();
                if (distanceToPlayer < attackRange)
                    currentState = EnemyState.Attack;
                else if (distanceToPlayer > detectionRange + 2f)
                    currentState = EnemyState.Patrol;
                break;

            case EnemyState.Attack:
                rb.velocity = new Vector2(0f, rb.velocity.y);
                FacePlayer();

                if (attackTimer <= 0f && HasLineOfSight())
                {
                    ShootAtPlayer();
                    attackTimer = attackCooldown;
                }

                if (distanceToPlayer > attackRange)
                    currentState = EnemyState.Chase;
                break;
        }

        HandleFlip();
    }

    void UpdateNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearest = p.transform;
            }
        }

        if (nearest != null)
        {
            player = nearest;
        }
    }

    void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (!IsGroundAhead())
        {
            patrolDirection *= -1;
            patrolTimer = patrolTime;
        }

        if (patrolTimer <= 0f)
        {
            patrolDirection *= -1;
            patrolTimer = patrolTime;
        }

        rb.velocity = new Vector2(patrolDirection * moveSpeed, rb.velocity.y);
    }

    void Chase()
    {
        if (!IsGroundAhead())
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void ShootAtPlayer()
    {
        if (enemyBulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                bulletRb.velocity = dir * bulletSpeed;
            }
        }
    }

    bool HasLineOfSight()
    {
        if (player == null || firePoint == null) return false;

        Vector2 origin = firePoint.position;
        Vector2 direction = (player.position - firePoint.position).normalized;
        float distance = Vector2.Distance(firePoint.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundLayer);
        return hit.collider == null;
    }

    bool IsGroundAhead()
    {
        if (ledgeCheck == null) return true;
        Vector2 checkPos = ledgeCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, ledgeCheckDistance, groundLayer);

        Debug.DrawRay(checkPos, Vector2.down * ledgeCheckDistance, hit.collider ? Color.green : Color.red);

        return hit.collider != null;
    }

    void HandleFlip()
    {
        if (rb.velocity.x > 0.05f && !facingRight)
            Flip();
        else if (rb.velocity.x < -0.05f && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        
        if (ledgeCheck != null)
        {
            Vector3 localPos = ledgeCheck.localPosition;
            localPos.x *= -1;
            ledgeCheck.localPosition = localPos;
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x && !facingRight)
            Flip();
        else if (player.position.x < transform.position.x && facingRight)
            Flip();
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
