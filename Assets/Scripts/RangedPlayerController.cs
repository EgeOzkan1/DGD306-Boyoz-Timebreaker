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
    private bool facingRight = true;

    public float slowMotionScale = 0.3f;
    public float slowMotionDuration = 1.5f;
    public float slowMotionCooldown = 5f;

    private bool isSlowingTime = false;
    private float slowMotionTimer = 0f;
    private float slowMotionCooldownTimer = 0f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private bool jumpQueued = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        slowMotionCooldownTimer -= Time.unscaledDeltaTime;

        if (Input.GetButtonDown("Jump1") && isGrounded)
        {
            jumpQueued = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        // Joystick 1 için slow motion: button 2 (örneğin Xbox X)
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) && !isSlowingTime && slowMotionCooldownTimer <= 0f)
        {
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isSlowingTime = true;
            slowMotionTimer = slowMotionDuration;
            slowMotionCooldownTimer = slowMotionDuration + slowMotionCooldown;
        }

        if (isSlowingTime)
        {
            slowMotionTimer -= Time.unscaledDeltaTime;
            if (slowMotionTimer <= 0f)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                isSlowingTime = false;
            }
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float move = Input.GetAxis("Horizontal"); // Joy 1 için tanımlı
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (jumpQueued)
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

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
