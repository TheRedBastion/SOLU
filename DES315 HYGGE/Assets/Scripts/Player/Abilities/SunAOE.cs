using UnityEngine;

public class SunAOE : MonoBehaviour
{
    [SerializeField] private float radius = 7f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float stunDuration = 0.4f;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float lifetime = 0.2f;

    private void Start()
    {
        ScaleToRadius();
        DealDamage();
        Destroy(gameObject, lifetime);
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            enemies
        );

        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponentInParent<Health>();
            if (health != null)
            {
                Vector2 dir = (hit.transform.position - transform.position).normalized;

                KnockbackData kb = new KnockbackData(
                    dir,
                    knockbackForce,
                    knockbackDuration,
                    default,
                    stunDuration
                );

                health.TakeDamage(damage, kb);
            }
        }
    }
    private void ScaleToRadius()
    {
        float diameter = radius * 2f;
        transform.localScale = new Vector3(diameter, diameter, 1f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
