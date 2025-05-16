using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float shootCooldown = 1.5f;
    public float chaseRange = 6f;
    public float stopDistance = 2f;
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public int maxHealth = 4;

    private Transform player;
    private float shootTimer;
    private Rigidbody2D rb;
    private Collider2D col;
    private int currentHealth;
    private int moveDirection = 1;
    private float lastDirectionChangeTime;
    private float directionChangeCooldown = 0.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.sharedMaterial = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (distanceToPlayer > stopDistance)
            {
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }

            if (shootTimer <= 0f)
            {
                Shoot(direction);
                shootTimer = shootCooldown;
            }
        }
        else
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }

        shootTimer -= Time.fixedDeltaTime;
    }

    void Shoot(Vector2 direction)
    {
        if (enemyBulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
            {
                rbBullet.velocity = direction.normalized * 10f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("PlayerBullet") && !collision.collider.CompareTag("EnemyBullet"))
        {
            if (Time.time - lastDirectionChangeTime > directionChangeCooldown)
            {
                moveDirection *= -1;
                lastDirectionChangeTime = Time.time;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
