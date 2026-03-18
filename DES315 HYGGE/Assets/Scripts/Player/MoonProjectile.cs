using UnityEngine;

public class MoonProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private int damage = 10;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [Header("Layers")]
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask obstacleLayers;

    private Vector2 startPosition;
    private float direction = 1f;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & obstacleLayers) != 0)
        {
            Destroy(gameObject);
            return;
        }

        if (((1 << other.gameObject.layer) & enemyLayers) != 0)
        {
            Health health = other.GetComponentInParent<Health>();
            if (health != null)
            {
                Vector2 knockDir = (other.transform.position - transform.position).normalized;
                KnockbackData kb = new KnockbackData(
                    knockDir,
                    knockbackForce,
                    knockbackDuration,
                    false
                );

                health.TakeDamage(damage, kb);
            }
            Destroy(gameObject);
        }
    }
}