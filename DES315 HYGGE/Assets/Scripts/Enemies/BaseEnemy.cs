using System.Collections;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected Transform player;
    protected Rigidbody2D rb;
    protected Health health;
    protected CharacterSwap characterSwap;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        characterSwap = GetComponentInParent<CharacterSwap>();
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
            player = characterSwap.character;
    }

    protected virtual void HandleDamage(int damage)
    {
        StartCoroutine(Flash());
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
