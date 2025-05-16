using UnityEngine;

public class PlayerBlockParry : MonoBehaviour
{
    public float parryWindow = 0.5f;
    public float parryCooldown = 1f;
    public GameObject parryDeflectEffect;
    public GameObject playerBulletPrefab;
    public Transform firePoint;
    public float reflectBulletSpeed = 20f;
    public int reflectBulletDamage = 2;

    private bool parryActive = false;
    private float parryTimer = 0f;
    private float cooldownTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private GameObject activeEffect;

    public bool IsParryActive() => parryActive;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0f)
        {
            parryActive = true;
            parryTimer = 0f;
            cooldownTimer = parryCooldown;
            ShowParryEffect();
        }

        if (parryActive)
        {
            parryTimer += Time.deltaTime;
            if (parryTimer > parryWindow)
            {
                parryActive = false;
                HideParryEffect();
            }
            else if (activeEffect != null)
            {
                Vector3 offset = spriteRenderer.flipX ? Vector3.left : Vector3.right;
                activeEffect.transform.position = transform.position + offset * 0.5f;
                activeEffect.transform.rotation = Quaternion.Euler(0, 0, spriteRenderer.flipX ? 180 : 0);
            }
        }
    }

    public bool TryParry(GameObject bullet)
    {
        if (!parryActive) return false;
        DeflectBullet(bullet);
        return true;
    }

    private void ShowParryEffect()
    {
        if (parryDeflectEffect && activeEffect == null)
        {
            Vector3 offset = spriteRenderer.flipX ? Vector3.left : Vector3.right;
            activeEffect = Instantiate(parryDeflectEffect, transform.position + offset * 0.5f, Quaternion.identity);
            activeEffect.transform.SetParent(transform);
            activeEffect.transform.rotation = Quaternion.Euler(0, 0, spriteRenderer.flipX ? 180 : 0);
        }
    }

    private void HideParryEffect()
    {
        if (activeEffect != null)
        {
            Destroy(activeEffect);
        }
    }

    private void DeflectBullet(GameObject bullet)
    {
        if (playerBulletPrefab && firePoint)
        {
            GameObject newBullet = Instantiate(playerBulletPrefab, firePoint.position, firePoint.rotation);

            Vector3 scale = newBullet.transform.localScale;
            scale.x = spriteRenderer.flipX ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            newBullet.transform.localScale = scale;

            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(spriteRenderer.flipX ? Vector2.left : Vector2.right);
                bulletScript.speed = reflectBulletSpeed;
                bulletScript.damage = reflectBulletDamage;
            }
        }

        Destroy(bullet);
    }
}
