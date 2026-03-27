using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private GroundDetector groundDetector;
    private KnockbackReceiver knockback;
    private Stamina stamina;
    private Animator m_animator;

    [Header("Gravity")]
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float riseGravity = 1f;
    [SerializeField] private float fallGravity = 3.5f;
    [SerializeField] private float waterGravityMultiplier = 0.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float terminalVelocity = 20f;

    [Header("Running")]
    [SerializeField] float acceleration = 60f;
    [SerializeField] float deceleration = 15f;

    public bool sprintActive;    
    private float moveSpeed;
    //[SerializeField] float maxSpeed = 50f; //might need later for speed caps
    [SerializeField] private float FootstepSpeed = 0.4f;
    
    private float WalkingCount = 0.0f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private bool canDash = true;
    private float originalGravity;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private float dashDirection;

    [Header("Jump Vars")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float jumpHeight = 6.5f;

    private float jumpBufferTimer;
    public bool isJumping;
    private bool jumpCut;
    private bool jumpHeld = false;
    private bool jumpBufferedRelease;
    private float jumpForce;

    [Header("Jump Bools")]
    [SerializeField] private bool fallGravityOnRelease = false;
    [SerializeField] private bool doubleJumpActive = false;

    private bool doubleJumpReady;
    private bool wasGrounded;

    private Vector2 moveInput;

    public AK.Wwise.Event footstepSound = new AK.Wwise.Event();

    public void Init(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetector = GetComponent<GroundDetector>();
        knockback = GetComponent<KnockbackReceiver>();
        m_animator = GetComponent<Animator>();
        stamina = GetComponent<Stamina>();

        originalGravity = rb.gravityScale;
        jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetector = GetComponent<GroundDetector>();
        knockback = GetComponent<KnockbackReceiver>();
        stamina = GetComponent<Stamina>();

        originalGravity = rb.gravityScale;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void JumpPressed()
    {
        jumpBufferTimer = jumpBufferTime;
        jumpHeld = true;
        jumpBufferedRelease = false;
    }

    public void JumpReleased()
    {
        jumpHeld = false;

        Vector2 velocity = rb.linearVelocity;
        if (isJumping && velocity.y > 0f && !jumpCut)
        {
            //midair release

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
            jumpCut = true;
        }
        else
        {
            //release before jump
            jumpBufferedRelease = true;
        }
    }

    public void DashPressed()
    {
        if (!canDash || isDashing || stamina.SprintLocked)
            return;

        stamina.CurrentStamina -= 10f;

        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;

        //dash in facing direction
        dashDirection = Mathf.Sign(transform.localScale.x);

        rb.gravityScale = 0f; //disable gravity
    }

    private void FixedUpdate()
    {
        if (knockback != null && (knockback.IsKnockedBack || knockback.IsStunned))
        {
            isDashing = false;
            return;
        }

        bool grounded = groundDetector.IsGrounded;

        if (wasGrounded && !grounded)
        {
            //player walked off ledge
            doubleJumpReady = true;
        }

        wasGrounded = grounded;

        if (jumpBufferTimer > 0f)
            jumpBufferTimer -= Time.fixedDeltaTime;

        Vector2 velocity = rb.linearVelocity;

        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

            dashTimeLeft -= Time.fixedDeltaTime;

            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
                rb.gravityScale = originalGravity;
            }
            return; //skip movement logic while dashing
        }

        //HORIZ
        {
            float targetVelocityX = moveInput.x * moveSpeed;

            if (sprintActive)
                targetVelocityX *= 1.5f;

            float velocityDiff = targetVelocityX - velocity.x;

            //weaker force when stopping
            float accelRate = (Mathf.Abs(targetVelocityX) > 0.01f) ? acceleration : deceleration;

            float force = velocityDiff * accelRate;

            rb.AddForce(Vector2.right * force, ForceMode2D.Force);
        }
        //JUMPING

        if (jumpBufferTimer > 0f && groundDetector.CanJump())//first jump
        {
            jumpBufferTimer = 0f;
            groundDetector.ConsumeCoyoteTime();

            float jumpVelocity = jumpBufferedRelease ? jumpForce * jumpCutMultiplier : jumpForce;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                Mathf.Max(rb.linearVelocity.y, 0f) + jumpVelocity
            );

            m_animator.SetBool("Jumping", true);

            isJumping = true;
            jumpCut = jumpBufferedRelease;
            jumpBufferedRelease = false;
            doubleJumpReady = true;
        }
        else if (jumpBufferTimer > 0f && doubleJumpActive && doubleJumpReady && !groundDetector.CanJump())//double jump
        {
            jumpBufferTimer = 0f;
            doubleJumpReady = false;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isJumping = true;
            jumpCut = false;
            m_animator.SetBool("Moving", true);
        }

        if (isJumping == false && grounded)
        {
            m_animator.SetBool("Jumping", false);
        }

        velocity = rb.linearVelocity;

        //GRAVITY

        float gravityAccel = gravity;
        if (groundDetector.InWater)
            gravityAccel *= waterGravityMultiplier;

        if (velocity.y > 0)
        {
            //rising
            if (fallGravityOnRelease)
            {
                if (!jumpCut && jumpHeld)
                    velocity.y += gravityAccel * riseGravity * Time.fixedDeltaTime;
                else
                    velocity.y += gravityAccel * fallGravity * Time.fixedDeltaTime;
            }
            else
            {
                velocity.y += gravityAccel * riseGravity * Time.fixedDeltaTime;
            }
        }
        else
        {
            //falling
            velocity.y += gravityAccel * fallGravity * Time.fixedDeltaTime;
        }

        float maxFallSpeed = groundDetector.InWater
            ? terminalVelocity * waterGravityMultiplier
            : terminalVelocity;

        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        rb.linearVelocity = velocity;

        if (grounded && rb.linearVelocity.y <= 0f)
        {
            isJumping = false;
            jumpCut = false;
            doubleJumpReady = false;
        }

        //FLIP
        {
            Player p = GetComponentInParent<Player>();
            if (moveInput.x > 0.05f)
            {
                transform.localScale = new Vector3(.5f, .5f, 1);
                m_animator.SetBool("Moving", true);

                p.isWalking = true;

                WalkingCount += Time.deltaTime * (moveSpeed / 10.0f);

                if (WalkingCount > FootstepSpeed)
                {
                    footstepSound.Post(gameObject);

                    WalkingCount = 0.0f;
                }
            }
            else if (moveInput.x < -0.05f)
            {
                transform.localScale = new Vector3(-.5f, .5f, 1);
                m_animator.SetBool("Moving", true);

                p.isWalking = true;

                WalkingCount += Time.deltaTime * (moveSpeed / 10.0f);

                if (WalkingCount > FootstepSpeed)
                {
                    footstepSound.Post(gameObject);

                    WalkingCount = 0.0f;
                }
            }
            else if (moveInput.x == 0)// could mess up float == 0!
            {
                m_animator.SetBool("Moving", false);

                p.isWalking = false;

                WalkingCount = FootstepSpeed;
            }
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
}