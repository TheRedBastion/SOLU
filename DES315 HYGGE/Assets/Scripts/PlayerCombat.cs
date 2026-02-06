using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector2 attackOffset = new Vector2(1f, 1f);
    public int attackDamage = 10;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Attack()
    {
        UpdateAttackPoint();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.transform.position,
            radius,
            enemies
        );

        foreach (Collider2D hit in hits)
        {
            //Debug.Log("Hit " + hit.name);

            Health health = hit.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    private void UpdateAttackPoint()
    {
        Transform active = player.GetActiveCharacterTransform();
        float dir = Mathf.Sign(active.localScale.x);

        attackPoint.transform.position =
            active.position + new Vector3(attackOffset.x * dir, attackOffset.y, 0f);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }
}