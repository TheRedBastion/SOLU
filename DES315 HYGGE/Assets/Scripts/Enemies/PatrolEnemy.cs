using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    enum EnemyState
    {
        Patrol,
        Chase
    }

    private EnemyState currentState = EnemyState.Patrol;

    [Header("Enemy Vars")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 5f;
    [SerializeField] private PatrolPath patrolPath;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameObject exclamation;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float reactionTime = 0.2f;
    private float reactionTimer = 0f;

    private bool movingRight = false;
    private bool justSpottedPlayer = false;
    private bool didReactJump = false;

    protected override void Start()
    {
        base.Start();
        SnapToGround();
    }

    bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    bool IsGrounded()
    {
        Collider2D col = GetComponent<Collider2D>();

        Vector2 origin = new Vector2(col.bounds.center.x, col.bounds.min.y);

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            0.2f,
            groundLayer
        );

        return hit.collider != null;
    }

    void SnapToGround()
    {
        Collider2D col = GetComponent<Collider2D>();

        RaycastHit2D hit = Physics2D.Raycast(
            col.bounds.center,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (hit.collider != null)
        {
            float halfHeight = col.bounds.extents.y;

            Vector3 pos = transform.position;
            pos.y = hit.point.y + halfHeight;
            transform.position = pos;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        HandleState();
    }

    void HandleState()
    {
        if (knockback != null && (knockback.IsKnockedBack || knockback.IsStunned)) return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();

                if (PlayerInRange())
                {
                    currentState = EnemyState.Chase;
                    justSpottedPlayer = true;
                    reactionTimer = reactionTime;
                    didReactJump = false;
                }
                break;

            case EnemyState.Chase:
                if (justSpottedPlayer)
                {
                    exclamation.SetActive(true);
                    ReactToPlayer();

                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                    reactionTimer -= Time.deltaTime;
                    if (reactionTimer <= 0f && IsGrounded())
                    {
                        exclamation.SetActive(false);
                        justSpottedPlayer = false;
                    }

                    return;
                }
                ChasePlayer();

                if (!PlayerInRange())
                    currentState = EnemyState.Patrol;
                break;
        }
    }

    void Patrol()
    {
        if (knockback != null && (knockback.IsKnockedBack || knockback.IsStunned)) return;

        if (patrolPath == null) return;

        if (movingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            UpdateFacingDirection(1);

            if (transform.position.x >= patrolPath.endPosition.x)
            {
                movingRight = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            UpdateFacingDirection(-1);

            if (transform.position.x <= patrolPath.startPosition.x)
            {
                movingRight = true;
            }
        }
    }

    void ChasePlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        //stoP at patrol edges
        if (dir > 0 && transform.position.x >= patrolPath.endPosition.x)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        if (dir < 0 && transform.position.x <= patrolPath.startPosition.x)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
        UpdateFacingDirection(dir);

        //jump if player is higher
        float heightDiff = player.position.y - transform.position.y;

        if (heightDiff > 1f && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void ReactToPlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        UpdateFacingDirection(dir);

        if (!didReactJump && IsGrounded())
        {
            rb.linearVelocity = new Vector2(0, jumpForce * 0.5f);
            didReactJump = true;
        }
    }

    void UpdateFacingDirection(float direction)
    {
        Vector3 scale = transform.localScale;

        if (direction > 0)
            scale.x = Mathf.Abs(scale.x);
        else if (direction < 0)
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
