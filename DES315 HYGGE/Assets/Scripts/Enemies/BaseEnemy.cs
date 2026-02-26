using System.Collections;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected int contactDamage = 10;
    [SerializeField] protected float attackKnockbackDuration = 0.2f;
    [SerializeField] protected float attackKnockbackForce = 5f;

    protected Transform player;
    protected Rigidbody2D rb;
    protected Health health;
    protected CharacterSwap characterSwap;

    protected KnockbackReceiver knockback;

    protected bool isKnockback = false;
    protected Vector2 knockbackForce = new Vector2(5f, 5f);
    protected float knockbackDuration = 0.2f;



    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        characterSwap = GetComponentInParent<CharacterSwap>();
        knockback = GetComponent<KnockbackReceiver>();
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
        if (characterSwap != null && characterSwap.swappedThisFrame)
        {
            player = characterSwap.character;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(characterSwap.player != null && !characterSwap.player.isGodmode)
                TryDamagePlayer(other.gameObject);
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (characterSwap.player != null && !characterSwap.player.isGodmode)
                TryDamagePlayer(collision.gameObject);
        }
    }

    protected void TryDamagePlayer(GameObject playerObj)
    {
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
        StartCoroutine(Flash());

        knockback?.ApplyKnockback(data);
    }

    protected virtual IEnumerator Flash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color original = sr.color;
        sr.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        sr.color = original;
    }

}
