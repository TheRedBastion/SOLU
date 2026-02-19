using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct KnockbackData
{
    public Vector2 direction;
    public float knockbackForce;
    public float duration;

    public KnockbackData(Vector2 direction, float knockbackForce, float duration)
    {
        this.direction = direction.normalized;
        this.knockbackForce = knockbackForce;
        this.duration = duration;
    }

}

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    [SerializeField] private float invincibilityDuration = 0f;

    private bool isInvincible = false;

    public int CurrentHealth { get; private set; }

    public UnityEvent<int, KnockbackData> OnDamageTaken; //passes damage
    public UnityEvent OnDeath;
    
    private void Awake()
    {
        CurrentHealth = maxHealth;

        OnDamageTaken ??= new UnityEvent<int, KnockbackData>();
        OnDeath ??= new UnityEvent();
    }

    public void TakeDamage(int damage, KnockbackData knockback = default)
    {
        if (isInvincible) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {CurrentHealth}/{maxHealth}");

        OnDamageTaken.Invoke(damage, knockback);

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        if (invincibilityDuration > 0f)
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
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public void ResetInvincibility()
    {
        isInvincible = false;
        StopAllCoroutines();
    }
}
