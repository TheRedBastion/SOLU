using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    [SerializeField] private float invincibilityDuration = 0f;

    private bool isInvincible = false;

    public int CurrentHealth { get; private set; }

    public UnityEvent<int> OnDamageTaken; //passes damage
    public UnityEvent OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        OnDamageTaken ??= new UnityEvent<int>();
        OnDeath ??= new UnityEvent();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {CurrentHealth}/{maxHealth}");

        OnDamageTaken.Invoke(damage);

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
