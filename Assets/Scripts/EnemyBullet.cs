using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var playerHealth = collision.collider.GetComponent<PlayerHealth>();
        var blockParry = collision.collider.GetComponent<PlayerBlockParry>();

        if (playerHealth != null)
        {
            if (blockParry != null && blockParry.TryParry(gameObject))
            {
                return; // parried
            }

            playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
        else if (!collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}