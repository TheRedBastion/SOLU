using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    [Header("Enemy Vars")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 5f;
    [SerializeField] private PatrolPath patrolPath;

    [SerializeField] private float moveSpeed = 2f;

    private bool movingRight = false;

    protected override void Start()
    {
        base.Start();
        SnapToGround();
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

        Patrol();
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
