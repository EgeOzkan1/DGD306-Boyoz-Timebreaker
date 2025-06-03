using UnityEngine;

public class Parry : MonoBehaviour
{
    [Header("Parry Settings")]
    public float parryWindow = 0.5f;
    public float parryCooldown = 1f;
    public GameObject parryDeflectEffect;
    public GameObject playerBulletPrefab;
    public Transform firePoint;
    public float reflectBulletSpeed = 20f;
    public int reflectBulletDamage = 2;

    [Header("Melee Attack Settings")]
    public float meleeRange = 1f;
    public int meleeDamage = 1;
    public LayerMask enemyLayer;
    public GameObject meleeEffect;

    private bool parryActive = false;
    private float parryTimer = 0f;
    private float parryCooldownTimer = 0f;
    private GameObject activeEffect;
    private Rigidbody2D rb;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        parryCooldownTimer -= Time.deltaTime;

        // 🎯 Parry (Joystick 2, örn. Üçgen tuşu)
        if (Input.GetKeyDown(KeyCode.Joystick2Button2) && parryCooldownTimer <= 0f)
        {
            parryActive = true;
            parryTimer = 0f;
            parryCooldownTimer = parryCooldown;
            ShowParryEffect();
        }

        // 🎯 Melee attack (Joystick 2'nin "Fire2" input'una bağlı)
        if (Input.GetButtonDown("Fire2"))
        {
            MeleeAttack();
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
        UpdateParryEffect();
    }

    void RotateFirePoint()
    {
        if (firePoint == null) return;
        facingRight = transform.localScale.x > 0f;
        Vector3 scale = firePoint.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        firePoint.localScale = scale;
    }

    void UpdateParryEffect()
    {
        if (activeEffect != null && firePoint != null)
        {
            activeEffect.transform.position = firePoint.position;
            activeEffect.transform.rotation = Quaternion.Euler(0, 0, facingRight ? 0 : 180);
        }
    }

    public bool TryParry(GameObject bullet)
    {
        if (!parryActive) return false;
        DeflectBullet(bullet);
        return true;
    }

    void ShowParryEffect()
    {
        if (parryDeflectEffect && activeEffect == null && firePoint != null)
        {
            activeEffect = Instantiate(parryDeflectEffect, firePoint.position, Quaternion.identity);
            activeEffect.transform.SetParent(firePoint);
            activeEffect.transform.rotation = Quaternion.Euler(0, 0, facingRight ? 0 : 180);
        }
    }

    void HideParryEffect()
    {
        if (activeEffect != null)
        {
            Destroy(activeEffect);
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
        }

        if (meleeEffect && firePoint != null)
        {
            GameObject effect = Instantiate(meleeEffect, firePoint.position, Quaternion.identity);
            effect.transform.rotation = Quaternion.Euler(0, 0, facingRight ? 0 : 180);
            Destroy(effect, 0.5f);
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
