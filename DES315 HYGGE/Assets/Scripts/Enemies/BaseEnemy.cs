using System.Collections;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int contactDamage = 10;
    [SerializeField] private float attackKnockbackDuration = 0.2f;
    [SerializeField] private float attackKnockbackForce = 5f;
    [SerializeField] private float stunDuration = 0f;
    [SerializeField] private float damageRate = 0.2f;

    [Header("Hit")]
    [SerializeField] private float flashDuration = 0.1f;

    protected Transform player;
    protected Rigidbody2D rb;
    protected Health health;
    protected CharacterSwap characterSwap;

    protected KnockbackReceiver knockback;

    protected bool isKnockback = false;
    protected Vector2 knockbackForce = new Vector2(5f, 5f);
    protected float knockbackDuration = 0.2f;

    private float damageTimer = 0f;
    private SpriteRenderer sr;
    private Color originalColor;
    private float flashTimer = 0f;

    public AK.Wwise.Event AttackingSound = new AK.Wwise.Event();

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        characterSwap = GetComponentInParent<CharacterSwap>();
        knockback = GetComponent<KnockbackReceiver>();

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

    }

    protected virtual void OnEnable()
    {
        if (health != null)
            health.OnDamageTaken.AddListener(HandleDamage);
    }

    protected virtual void OnDisable()
    {
        if (health != null)
            health.OnDamageTaken.RemoveListener(HandleDamage);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (sr == null) return;

        if (characterSwap != null && characterSwap.swappedThisFrame)
        {
            player = characterSwap.character;
        }

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else if (sr.color != originalColor)
        {
            sr.color = originalColor;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryDamagePlayer(other.gameObject);
            AttackingSound.Post(gameObject);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            damageTimer -= Time.fixedDeltaTime;
            if (damageTimer <= 0f)
            {
                TryDamagePlayer(other.gameObject);
                damageTimer = damageRate;
                AttackingSound.Post(gameObject);
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            damageTimer -= Time.fixedDeltaTime;
            if (damageTimer <= 0f)
            {
                TryDamagePlayer(collision.gameObject);
                damageTimer = damageRate;
                AttackingSound.Post(gameObject);
            }
        }
    }

    protected void TryDamagePlayer(GameObject playerObj)
    {
        if (characterSwap.player != null && characterSwap.player.isGodmode)
            return;

        Health playerHealth = playerObj.GetComponent<Health>();

        if (playerHealth != null)
        {
            Vector2 hitDirection = (playerObj.transform.position - transform.position).normalized;
            KnockbackData kb = new KnockbackData(
                hitDirection,
                attackKnockbackForce,
                attackKnockbackDuration
            );
            playerHealth.TakeDamage(contactDamage, kb);
        }
    }

    protected virtual void HandleDamage(int damage, KnockbackData data)
    {
        flashTimer = flashDuration;

        knockback?.ApplyKnockback(data);
    }

    public void SetCharacterSwap(CharacterSwap cs)
    {
        characterSwap = cs;
    }

}
