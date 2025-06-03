using UnityEngine;

public class MeleePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private bool jumpQueued = false;

    private Rigidbody2D rb;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Zıplama sadece yere temas ediyorsa
        if (Input.GetButtonDown("Jump2") && isGrounded)
        {
            jumpQueued = true;
        }
    }

    void FixedUpdate()
    {
        // Yerde mi kontrolü
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Hareket girişi: önce analog stick, sonra D-Pad yedeği
        float moveInput = Input.GetAxis("Joystick2Horizontal"); // sol analog stick
        if (Mathf.Approximately(moveInput, 0f))
            moveInput = Input.GetAxis("DPadHorizontal2"); // D-pad (eğer tanımlıysa)

        // Hareket uygula
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Yön çevir
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            Flip();
        }

        // Zıplama uygula
        if (jumpQueued)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpQueued = false;
        }
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
