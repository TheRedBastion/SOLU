using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector2 attackOffset = new Vector2(1.3f, -0.3f);

    [SerializeField] private float attackKnockbackDuration = 0.2f;
    [SerializeField] private float attackKnockbackForce = 5f;

    [SerializeField] private GameObject moonProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    public int attackDamage = 10;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void Attack()
    {
        UpdateAttackPoint();

        StartCoroutine(showAttack(0.2f));

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
                Vector2 hitDirection = (hit.transform.position - attackPoint.transform.position).normalized;
                //Vector2 hitDirection = (hit.transform.position - player.GetActiveCharacterTransform().position).normalized;
                KnockbackData kb = new KnockbackData(
                    hitDirection,
                    attackKnockbackForce,
                    attackKnockbackDuration,
                    false
                );

                health.TakeDamage(attackDamage, kb);
            }
        }
    }

    public void SpecialAttack1()
    {
        if (player.character == 0)
        {
            Transform active = player.GetActiveCharacterTransform();

            float dir = Mathf.Sign(active.localScale.x);

            Vector3 spawnPos = projectileSpawnPoint != null
                ? projectileSpawnPoint.position
                : active.position;

            GameObject proj = Instantiate(
                moonProjectilePrefab,
                spawnPos,
                Quaternion.identity
            );

            proj.transform.localScale = new Vector3(dir, 1, 1);

            MoonProjectile mp = proj.GetComponent<MoonProjectile>();
            if (mp != null)
            {
                mp.SetDirection(dir);
            }
        }
        else
        {
            //sun
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

    IEnumerator showAttack(float t)
    {
        SpriteRenderer sr = attackPoint.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        sr.enabled = true;
        yield return new WaitForSeconds(t);
        sr.enabled = false;
    }
}