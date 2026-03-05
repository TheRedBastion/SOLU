using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private GroundDetector groundDetector;
    private KnockbackReceiver knockback;

    [Header("Gravity")]
    [SerializeField] private float riseGravity = 1f;
    [SerializeField] private float fallGravity = 3.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float terminalVelocity = 20f;
    [SerializeField] private float gravityScale = 3f;

    [Header("Running")]
    [SerializeField] float acceleration = 60f;
    [SerializeField] float deceleration = 15f;
    public bool sprintActive;    
    private float moveSpeed;
    //[SerializeField] float maxSpeed = 50f; //might need later for speed caps

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    private bool isDashing;
    private bool canDash = true;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private float dashDirection;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    private float jumpBufferTimer;
    private bool isJumping;
    private bool jumpCut;
    private float jumpForce;

    //slope gravity
    private bool isGrounded;
    private Vector2 groundNormal = Vector2.up;
    private ContactPoint2D[] contacts = new ContactPoint2D[10];
    [SerializeField] private float maxGroundAngle = 45f;

    private Vector2 moveInput;

    public void Init(float moveSpeed, float jumpForce)
    {
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        groundDetector = GetComponent<GroundDetector>();
        knockback = GetComponent<KnockbackReceiver>();

        rb.gravityScale = 0f; //disable default gravity
        rb.freezeRotation = true;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void JumpPressed()
    {
        jumpBufferTimer = jumpBufferTime;
    }

    public void JumpReleased()
    {
        if (isJumping && rb.linearVelocity.y > 0)
        {

            rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            rb.linearVelocity.y * jumpCutMultiplier
            );

            jumpCut = true;
        }
    }

    public void DashPressed()
    {
        if (!canDash || isDashing)
            return;

        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;

        //dash in facing direction
        dashDirection = Mathf.Sign(transform.localScale.x);
    }

    private void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockedBack)
            return;

        if (jumpBufferTimer > 0f)
            jumpBufferTimer -= Time.fixedDeltaTime;

        UpdateGrounded();

        Vector2 velocity = rb.linearVelocity;

        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

            dashTimeLeft -= Time.fixedDeltaTime;

            if (dashTimeLeft <= 0f)
                isDashing = false;

            return; //skip movement logic while dashing
        }

        //HORIZ
        float targetVelocityX = moveInput.x * moveSpeed;
        if (sprintActive) 
            targetVelocityX *= 1.5f;

        //movement along slope
        Vector2 slopeDir = new Vector2(groundNormal.y, -groundNormal.x); // perpendicular to slope
        slopeDir.Normalize();
        float velocityDiff = targetVelocityX - Vector2.Dot(rb.linearVelocity, slopeDir);
        float accelRate = Mathf.Abs(targetVelocityX) > 0.01f ? acceleration : deceleration;
        rb.AddForce(slopeDir * velocityDiff * accelRate, ForceMode2D.Force);

        //custon gravity
        ApplySlopeGravity();

        //JUMPING

        if (jumpBufferTimer > 0f && groundDetector.CanJump())
        {
            jumpBufferTimer = 0f;
            groundDetector.ConsumeCoyoteTime();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            isJumping = true;
            jumpCut = false;
        }



        //FALL SPEED CAP

        Vector2 v = rb.linearVelocity;

        if (v.y < -terminalVelocity)
        {
            v.y = -terminalVelocity;
            rb.linearVelocity = v;
        }

        //rb.linearVelocity = velocity;

        if (rb.linearVelocity.y <= 0)
        {
            isJumping = false;
            jumpCut = false;
        }

        //FLIP

        if (moveInput.x > 0.05f)
        {
            transform.localScale = new Vector3(.5f, .5f, 1);
        }
        else if (moveInput.x < -0.05f)
        {
            transform.localScale = new Vector3(-.5f, .5f, 1);
        }

        //DASH COOLDOWN
        if (!canDash)
        {
            dashCooldownTimer += Time.fixedDeltaTime;

            if (dashCooldownTimer >= dashCooldown)
            {
                canDash = true;
                dashCooldownTimer = 0f;
            }
        }

    }

    private void UpdateGrounded()
    {
        isGrounded = false;
        groundNormal = Vector2.up;

        int contactCount = rb.GetContacts(contacts);
        for (int i = 0; i < contactCount; i++)
        {
            ContactPoint2D c = contacts[i];
            float angle = Vector2.Angle(c.normal, Vector2.up);
            if (angle <= maxGroundAngle)
            {
                isGrounded = true;
                groundNormal = c.normal;
                break;
            }
        }
        groundDetector.SetGrounded(isGrounded);
        groundDetector.player.OnGround = isGrounded;
    }

    private void ApplySlopeGravity()
    {
        Vector2 gravityDir;

        if (isGrounded)
        {
            //gravity perpendicular to slope stopsliding
            gravityDir = -groundNormal;
        }
        else
        {
            gravityDir = Vector2.down;
        }

        float gravityMultiplier;

        if (rb.linearVelocity.y > 0)
        {
            //going up
            gravityMultiplier = jumpCut ? fallGravity : riseGravity;
        }
        else
        {
            //falling
            gravityMultiplier = fallGravity;
        }

        //apply custom gravity along the direction
        Vector2 customGravity = gravityDir * Physics2D.gravity.magnitude * gravityScale * gravityMultiplier;

        rb.AddForce(customGravity, ForceMode2D.Force);
    }
}