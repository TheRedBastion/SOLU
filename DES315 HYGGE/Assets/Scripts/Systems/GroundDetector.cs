using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Player player;
    public LayerMask ground;

    public int groundContacts = 0;

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
        Vector2 size = capsule.bounds.size;
        Vector2 origin = capsule.bounds.center;

        //shrink the capsule for the cast only
        //size.y *= 0.95f;  //slightly shorter

        RaycastHit2D hit = Physics2D.CapsuleCast(
            origin,
            size,
            CapsuleDirection2D.Vertical,
            0f,
            Vector2.down,
            castDistance,
            ground
        );

        if (hit.collider != null && hit.normal.y > slopeLimit)
        {
            isGrounded = true;
            player.OnGround = true;
            coyoteTimer = coyoteTime;
        }
        else
        {
            isGrounded = false;
        }

        // Coyote countdown
        if (!isGrounded)
        {
            coyoteTimer -= Time.fixedDeltaTime;

            if (coyoteTimer <= 0f)
            {
                player.OnGround = false;
            }
        }
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
