using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

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
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {CurrentHealth}/{maxHealth}");

        OnDamageTaken.Invoke(damage);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }
}
