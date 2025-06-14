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

        
        RotateToDirection(direction);
    }

    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var playerHealth = collision.collider.GetComponent<PlayerHealth>();
        var parry = collision.collider.GetComponent<Parry>();

        if (playerHealth != null)
        {
            if (parry != null && parry.TryParry(gameObject))
            {
                return; 
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
