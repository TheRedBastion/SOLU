using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Player player;
    public LayerMask ground;

    [Header("Ground Check")]
    [SerializeField] private float castDistance = 0.05f;
    [SerializeField] private float slopeLimit = 0.7f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    private float coyoteTimer;

    private CapsuleCollider2D capsule;
    private bool isGrounded;

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider2D>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        PhysicsMaterial2D mat = new PhysicsMaterial2D();
        mat.friction = 0f;
        mat.bounciness = 0f;

        rb.sharedMaterial = mat;
    }

    public bool CanJump()
    {
        return isGrounded || coyoteTimer > 0f;
    }
    public void ConsumeCoyoteTime()
    {
        coyoteTimer = 0f;
        //player.OnGround = false;
    }

    private void FixedUpdate()
    {
        Bounds bounds = capsule.bounds;

        float width = bounds.size.x * 0.8f;
        float height = 0.05f;

        Vector2 boxSize = new Vector2(width, height);

        Vector2 origin = new Vector2(
            bounds.center.x,
            bounds.min.y
        );

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            boxSize,
            0f,
            Vector2.down,
            castDistance,
            ground
        );

        bool groundedNow =
            hit.collider != null &&
            hit.normal.y > slopeLimit;

        if (groundedNow)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        isGrounded = groundedNow;
        player.OnGround = isGrounded;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {
            player.OnSwap = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {
            player.OnSwap = false;
        }
    }
}
