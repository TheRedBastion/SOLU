using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponentInParent<Health>();
        if (health != null)
        {
            health.TakeDamage(health.CurrentHealth);
        }
    }
}

