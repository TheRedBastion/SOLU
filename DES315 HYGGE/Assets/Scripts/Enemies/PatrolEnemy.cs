using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 5f;
    public PatrolPath patrolPath;

    public float moveSpeed = 2f;

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
        if (patrolPath == null) return;

        if (movingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

            if (transform.position.x >= patrolPath.endPosition.x)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            if (transform.position.x <= patrolPath.startPosition.x)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
