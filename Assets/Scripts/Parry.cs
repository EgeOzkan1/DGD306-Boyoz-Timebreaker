using UnityEngine;
using System.Collections;

public class Parry : MonoBehaviour
{
    [Header("Parry Settings")]
    public float parryWindow = 0.5f;
    public float parryCooldown = 1f;
    public GameObject playerBulletPrefab;
    public Transform firePoint;
    public float reflectBulletSpeed = 20f;
    public int reflectBulletDamage = 2;

    [Header("Melee Attack Settings")]
    public float meleeRange = 1f;
    public int meleeDamage = 1;
    public LayerMask enemyLayer;
    public float meleeCooldown = 0.5f;

    [Header("Sprite Effects")]
    public Sprite defaultSprite;
    public Sprite parrySprite;
    public Sprite meleeSprite;
    public float meleeSpriteDuration = 0.2f;

    private float meleeCooldownTimer = 0f;
    private bool parryActive = false;
    private float parryTimer = 0f;
    private float parryCooldownTimer = 0f;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        parryCooldownTimer -= Time.deltaTime;
        meleeCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Joystick2Button1) && parryCooldownTimer <= 0f)
        {
            parryActive = true;
            parryTimer = 0f;
            parryCooldownTimer = parryCooldown;
            ShowParryEffect();
        }

        if (Input.GetButtonDown("Fire2") && meleeCooldownTimer <= 0f)
        {
            MeleeAttack();
            meleeCooldownTimer = meleeCooldown;
        }

        if (parryActive)
        {
            parryTimer += Time.deltaTime;
            if (parryTimer > parryWindow)
            {
                parryActive = false;
                HideParryEffect();
            }
        }

        RotateFirePoint();
    }

    void RotateFirePoint()
    {
        if (firePoint == null) return;
        facingRight = transform.localScale.x > 0f;
        Vector3 scale = firePoint.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        firePoint.localScale = scale;
    }

    public bool TryParry(GameObject bullet)
    {
        if (!parryActive) return false;
        DeflectBullet(bullet);
        return true;
    }

    void ShowParryEffect()
    {
        if (spriteRenderer != null && parrySprite != null)
        {
            spriteRenderer.sprite = parrySprite;
        }
    }

    void HideParryEffect()
    {
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    void DeflectBullet(GameObject bullet)
    {
        if (playerBulletPrefab && firePoint)
        {
            GameObject newBullet = Instantiate(playerBulletPrefab, firePoint.position, firePoint.rotation);
            newBullet.transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(facingRight ? Vector2.right : Vector2.left);
                bulletScript.speed = reflectBulletSpeed;
                bulletScript.damage = reflectBulletDamage;
            }
        }

        Destroy(bullet);
    }

    public void MeleeAttack()
    {
        Vector2 attackDirection = facingRight ? Vector2.right : Vector2.left;
        Vector2 attackOrigin = (Vector2)transform.position + attackDirection * meleeRange * 0.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, meleeRange * 0.5f, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyAI enemy = hit.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(meleeDamage);
            }

            Boss1 boss = hit.GetComponent<Boss1>();
            if (boss != null)
            {
                boss.TakeDamage(meleeDamage);
            }

            HelicopterAI heli = hit.GetComponent<HelicopterAI>();
            if (heli != null)
            {
                heli.TakeDamage(meleeDamage);
            }
        }

        StartCoroutine(FlashMeleeSprite());
    }

    IEnumerator FlashMeleeSprite()
    {
        if (spriteRenderer != null && meleeSprite != null)
        {
            spriteRenderer.sprite = meleeSprite;
            yield return new WaitForSeconds(meleeSpriteDuration);
            spriteRenderer.sprite = defaultSprite;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 attackDirection = facingRight ? Vector2.right : Vector2.left;
        Vector2 attackOrigin = (Vector2)transform.position + attackDirection * meleeRange * 0.5f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackOrigin, meleeRange * 0.5f);
    }
}
