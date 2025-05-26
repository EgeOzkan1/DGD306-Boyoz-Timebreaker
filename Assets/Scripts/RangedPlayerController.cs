using UnityEngine;

public class RangedPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.3f;

    private Rigidbody2D rb;
    private float shootTimer;
    private bool jumpQueued;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpQueued = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        float move = 0f;
        if (Input.GetKey(KeyCode.A)) move = -1f;
        if (Input.GetKey(KeyCode.D)) move = 1f;

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (jumpQueued && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpQueued = false;
        }

        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
        {
            Flip();
        }
    }

    void Shoot()
    {
        if (shootTimer > 0f) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(facingRight ? Vector2.right : Vector2.left);
        }

        shootTimer = shootCooldown;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
    }
}
