using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector2 attackOffset = new Vector2(1.3f, -0.3f);
    public int attackDamage = 10;

    [Header("Attack KB Info")]
    [SerializeField] private float attackKnockbackDuration = 0.2f;
    [SerializeField] private float attackKnockbackForce = 5f;
    [SerializeField] private float stunDuration = 0f;

    [Header("Moon Projectile")]
    [SerializeField] private GameObject moonProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float moonProjCooldown = 3f;
    private float moonProjTimer = 0f;

    [Header("Sun AOE")]
    [SerializeField] private GameObject sunAOEPrefab;
    [SerializeField] private float sunAOECooldown = 3f;
    private float sunAOETimer = 0f;

    private Animator m_animator;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player.character == 0)
        {
            if(moonProjTimer > 0f)
                moonProjTimer -= Time.deltaTime;
        }
        else
        {
            if (sunAOETimer > 0f)
                sunAOETimer -= Time.deltaTime;
        }
    }

    public void Attack()
    {

        m_animator.SetBool("Punching", true);

        //if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        //{
        //    m_animator.SetBool("Punching", false);
        //}


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
            if (moonProjTimer > 0f) return;
        }
        else
        {
            if (sunAOETimer > 0f) return;
        }

        m_animator.SetBool("Special", true);
    }

    public void PerformSpecialAttack()
    {
        Transform active = player.GetActiveCharacterTransform();

        if (player.character == 0)
        {
            float dir = Mathf.Sign(active.localScale.x);

            Vector3 spawnPos = projectileSpawnPoint != null
                ? projectileSpawnPoint.position
                : active.position;

            GameObject proj = Instantiate(
                moonProjectilePrefab,
                spawnPos,
                Quaternion.identity
            );

            MoonProjectile mp = proj.GetComponent<MoonProjectile>();
            if (mp != null)
            {
                mp.SetDirection(dir);
            }

            moonProjTimer = moonProjCooldown;
        }
        else
        {
            Instantiate(sunAOEPrefab, active.position, Quaternion.identity);
            sunAOETimer = sunAOECooldown;
        }

        StartCoroutine(showAttack(0.2f));
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
        m_animator.SetBool("Punching", false);
        m_animator.SetBool("Special", false);
    }

    
}