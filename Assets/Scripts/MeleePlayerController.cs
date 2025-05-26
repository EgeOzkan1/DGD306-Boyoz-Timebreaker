using UnityEngine;

public class MeleePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool jumpQueued;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpQueued = true;
        }
    }

    void FixedUpdate()
    {
        moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;

        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            Flip();
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (jumpQueued && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpQueued = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
