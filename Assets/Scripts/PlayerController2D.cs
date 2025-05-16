using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.2f;

    [Header("Time Slow Settings")]
    public float slowMoScale = 0.3f;
    public float slowMoDuration = 2f;
    public float slowMoCooldown = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private float horizontalInput;

    private bool isSlowMo = false;
    private float slowMoTimer = 0f;
    private float slowMoCooldownTimer = 0f;

    private float shootTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        GroundCheck();
        UpdateTimers();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButton("Fire1") && shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSlowMo && slowMoCooldownTimer <= 0f)
        {
            ActivateSlowMo();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Vector3 scale = bullet.transform.localScale;
        scale.x = spriteRenderer.flipX ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        bullet.transform.localScale = scale;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(spriteRenderer.flipX ? Vector2.left : Vector2.right);
        }
    }

    void ActivateSlowMo()
    {
        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isSlowMo = true;
        slowMoTimer = slowMoDuration;
        slowMoCooldownTimer = slowMoCooldown;
    }

    void UpdateTimers()
    {
        if (isSlowMo)
        {
            slowMoTimer -= Time.unscaledDeltaTime;
            if (slowMoTimer <= 0f)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                isSlowMo = false;
            }
        }
        else if (slowMoCooldownTimer > 0f)
        {
            slowMoCooldownTimer -= Time.unscaledDeltaTime;
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
