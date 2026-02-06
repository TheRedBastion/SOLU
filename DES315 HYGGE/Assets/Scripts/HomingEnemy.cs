using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;

    private CharacterSwap characterSwap;
    private Rigidbody2D rb;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDamageTaken.AddListener(HandleDamage);
    }

    private void OnDisable()
    {
        health.OnDamageTaken.RemoveListener(HandleDamage);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Find player if not assigned in inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        characterSwap = GetComponentInParent<CharacterSwap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if(characterSwap.swappedThisFrame)
            player = characterSwap.character;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if(distanceToPlayer <= detectionRange)
        {
            //move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            //stop if player is out of range
            rb.linearVelocity = Vector2.zero;
        }

    }

    private void HandleDamage(int damage)
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color original = sr.color;
        sr.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        sr.color = original;
    }
}
