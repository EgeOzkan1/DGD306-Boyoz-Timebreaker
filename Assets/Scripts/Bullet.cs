using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1;
    public float maxDistance = 15f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector2 startPosition;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        float traveled = Vector2.Distance(startPosition, transform.position);
        if (traveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Boss1 boss = collision.collider.GetComponent<Boss1>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        HelicopterAI heli = collision.collider.GetComponent<HelicopterAI>();
        if (heli != null)
        {
            heli.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (!collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
