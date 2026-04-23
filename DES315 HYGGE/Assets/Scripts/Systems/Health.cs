using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct KnockbackData
{
    public Vector2 direction;
    public float knockbackForce;
    public float duration;
    public bool noKnockback;
    public float stunDuration;

    public KnockbackData(Vector2 direction, float knockbackForce, float duration, bool noKnockback = false, float stunDuration = 0f)
    {
        this.direction = direction.normalized;
        this.knockbackForce = knockbackForce;
        this.duration = duration;
        this.noKnockback = noKnockback;
        this.stunDuration = stunDuration <= duration ? duration : stunDuration;
    }

}

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 0f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private float blinkAlpha = 0.5f;
    [SerializeField] private float brightnessMultiplier = 1.5f;

    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    public int CurrentHealth { get; private set; }

    public UnityEvent<int, KnockbackData> OnDamageTaken; //passes damage
    public UnityEvent OnDeath;

    public AK.Wwise.Event DamageTakenSound = new AK.Wwise.Event();
    public AK.Wwise.Event Slain = new AK.Wwise.Event();

    private void Awake()
    {
        CurrentHealth = maxHealth;


        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        OnDamageTaken ??= new UnityEvent<int, KnockbackData>();
        OnDeath ??= new UnityEvent();
    }

    public void TakeDamage(int damage, KnockbackData knockback = default)
    {
        if (isInvincible) return;

        Player player = GetComponentInParent<Player>();

        if (player != null)
        {
            if (player.isGodmode)
                return;
        }

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {CurrentHealth}/{maxHealth}");

        OnDamageTaken.Invoke(damage, knockback);

        if (CurrentHealth > 0)
            DamageTakenSound.Post(gameObject);

        if (CurrentHealth <= 0)
        {
            Slain.Post(gameObject);
            Die();
            return;
        }

        if (invincibilityDuration > 0f || !isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float timer = 0f;

        Color originalColor = spriteRenderer.color;

        //brighter full color
        Color brightColor = new Color(
            originalColor.r * brightnessMultiplier,
            originalColor.g * brightnessMultiplier,
            originalColor.b * brightnessMultiplier,
            1f
        );

        Color brightHalfColor = brightColor;
        brightHalfColor.a = blinkAlpha;

        while (timer < invincibilityDuration)
        {
            float t = Mathf.PingPong(timer / blinkInterval, 1f);
            spriteRenderer.color = Color.Lerp(brightColor * blinkAlpha, brightColor, t);

            timer += Time.deltaTime;
            yield return null;
        }

        //original color
        spriteRenderer.color = originalColor;

        isInvincible = false;
    }

    public void ResetInvincibility()
    {
        isInvincible = false;
        StopAllCoroutines();
    }
}
